using Campus_Connect.Models;
using Microsoft.AspNetCore.Mvc;

namespace Campus_Connect.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Feed()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(Post newPost)
        {
            var returnUrl = Request.Headers["Referer"].ToString();
            if (ModelState.IsValid)
            {
                if(newPost.PostFile != null)
                {
                    //save to firebase storage
                }
            }


            return Redirect(returnUrl);
        }
    }
}
