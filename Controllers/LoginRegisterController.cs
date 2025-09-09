using Campus_Connect.Models;
using FirebaseAdmin.Auth;
using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database.Query;
using System.Net.Http.Json;
using Campus_Connect.Services;

namespace Campus_Connect.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly FirebaseAuth auth;
        private readonly FirebaseClient _db;
        private readonly FirebaseServices _firebaseService;

        public LoginRegisterController(FirebaseServices firebaseServices)
        {
            auth = FirebaseAuth.DefaultInstance;
            _db = new FirebaseClient("https://campus-connect-14d4f-default-rtdb.firebaseio.com/");
            _firebaseService = firebaseServices;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginBto)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var token = await _firebaseService.LoginAsync(loginBto.Email, loginBto.Password);

            if(token == null)
            {
                return Redirect(returnUrl);
            }
            var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            FirebaseAuth.DefaultInstance.GetUserAsync(decoded.Uid);
            var LoggedInUserId = decoded.Uid;
            HttpContext.Session.SetString("uId", LoggedInUserId);

            return RedirectToAction("Index", "Home");

        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user) {
            bool userAddedCommunity = false;
            var returnUrl = Request.Headers["Referer"].ToString();
            if (ModelState.IsValid) 
            {
                if (user.Password.Equals(user.ConfirmedPassword))
                {
                    string userNumber = user.PhoneNumber.Insert(1, "+27");
                    userNumber = userNumber.Remove(0, 1);
                    try
                    {
                        var userRecordArgs = new UserRecordArgs()
                        {
                            Email = user.Email,
                            EmailVerified = false,
                            Password = user.Password,
                            PhoneNumber = userNumber,
                            DisplayName = String.Concat(user.Name," ", user.Surname),
                            Disabled = false
                        };
                        var registerdUser = await auth.CreateUserAsync(userRecordArgs);

                        if (registerdUser != null)
                        {

                            await _db
                                .Child("Users")
                                .Child(registerdUser.Uid)
                                .PutAsync(new
                                {
                                    Name = user.Name,
                                    Surname = user.Surname,
                                    Email = user.Email,
                                    PhoneNumber = userNumber,
                                    Faculty = user.Faculty,
                                    CreatedAt = DateTime.UtcNow
                                });

                            //add user to the community before redirection
                            userAddedCommunity = await _firebaseService.AddUserToCommmunity(registerdUser.Uid, user.Faculty);

                            if (registerdUser != null & userAddedCommunity == true)
                            {
                                return RedirectToAction("Login", "LoginRegister");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    TempData["PasswordError"] = "Passwords did not match";
                    return Redirect(returnUrl);
                }
            }

            return Redirect(returnUrl);


        }
    }

    
}
