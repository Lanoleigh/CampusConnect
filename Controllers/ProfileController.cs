using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Campus_Connect.Controllers
{
    public class ProfileController : Controller
    {
        private readonly FirebaseAuth _auth;
        public ProfileController()
        {
            _auth = FirebaseAuth.DefaultInstance;
        }
        public async Task<IActionResult> Profile()
        {
            string userId = HttpContext.Session.GetString("uId");
            var user = await _auth.GetUserAsync(userId);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
           
            return RedirectToAction("Index", "Home");
        }
    }
}
