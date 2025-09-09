using Microsoft.AspNetCore.Mvc;

namespace Campus_Connect.Controllers
{
    public class CommunitiesController : Controller
    {
        public IActionResult CommunityPage()
        {
            return View();
        }
    }
}
