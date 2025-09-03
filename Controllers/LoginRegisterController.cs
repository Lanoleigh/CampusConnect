using Campus_Connect.Models;
using FirebaseAdmin.Auth;
using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database.Query;

namespace Campus_Connect.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly FirebaseAuth auth;
        private readonly FirebaseClient _db;

        public LoginRegisterController()
        {
            auth = FirebaseAuth.DefaultInstance;
            _db = new FirebaseClient("https://campus-connect-14d4f-default-rtdb.firebaseio.com/");
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user) {
            var returnUrl = Request.Headers["Referer"].ToString();
            if (ModelState.IsValid) 
            {
                string userNumber = user.PhoneNumber.Insert(1, "+27");
                userNumber = userNumber.Remove(0,1);
                try
                {
                    var userRecordArgs = new UserRecordArgs()
                    {
                        Email = user.Email,
                        EmailVerified = false,
                        Password = user.Password,
                        PhoneNumber = userNumber,
                        DisplayName = String.Concat(user.Name, user.Surname),
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
                                CreatedAt = DateTime.UtcNow
                            });

                       return RedirectToAction("Login","LoginRegister");
                    }


                    
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return Redirect(returnUrl);
                }
            }

            return Redirect(returnUrl);


        }
    }
    class AuthUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
    
}
