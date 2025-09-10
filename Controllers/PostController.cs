using Campus_Connect.Models;
using Campus_Connect.Services;
using Campus_Connect.ViewModels;
using Firebase.Database;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Firebase.Storage;
using Google.Cloud.Storage.V1;
using Firebase.Database.Query;
using Google.Apis.Auth.OAuth2;

namespace Campus_Connect.Controllers
{
    public class PostController : Controller
    {
        private readonly FirebaseAuth _auth;
        private readonly FirebaseClient _db;
        private readonly CommunityFeedService _feedService;

        public PostController(CommunityFeedService feedService)
        {
            _auth = FirebaseAuth.DefaultInstance;
            _db = new FirebaseClient("https://campus-connect-14d4f-default-rtdb.firebaseio.com/");
            _feedService = feedService; 
        }
        public async Task<IActionResult> Feed()
        {
            string userID = HttpContext.Session.GetString("uId");
            var communityViewModel = new CommunityViewModel();
            
            //filtering the user interest groups as welll the community
            communityViewModel = await _feedService.FilterCommunitiesAndInterestGroups(userID);

            return View(communityViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPost(Post newPost)
        {
            var returnUrl = Request.Headers["Referer"].ToString();
            if (ModelState.IsValid)
            {
                string uId = HttpContext.Session.GetString("uId");
                newPost.UserID = uId;
                newPost.PostDate = DateTime.Now;
                if(newPost.PostFile != null)
                {
                    var storage = StorageClient.Create(GoogleCredential.FromFile("keys/campus-connect-key.json"));
                    var bucket = "campus-connect-14d4f.firebasestorage.app";


                    using var stream = newPost.PostFile.OpenReadStream();

                    var fileName = $"posts/{Guid.NewGuid()}_{newPost.PostFile.FileName}";
                    //save to firebase storage
                    await storage.UploadObjectAsync(bucket, fileName, null, stream);

                    newPost.FileUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucket}/o/{Uri.EscapeDataString(fileName)}?alt=media";
                    
                }
                var fbPost = new FirebasePost();
                fbPost.UserID = uId;
                fbPost.AdditionalMessage = newPost.AdditionalMessage;
                fbPost.Title = newPost.Title;
                fbPost.FileUrl = newPost.FileUrl;
                fbPost.Visibility = newPost.Visibility;
                fbPost.PostDate = DateTime.Now;


               
                await _db.Child("Posts")
                    .PostAsync(fbPost);
            }


            return RedirectToAction("Feed");
        }

    }
}
