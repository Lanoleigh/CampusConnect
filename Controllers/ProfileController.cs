using Microsoft.AspNetCore.Mvc;

namespace Campus_Connect.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
