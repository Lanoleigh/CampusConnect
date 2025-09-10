using Campus_Connect.Services;
using Campus_Connect.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace Campus_Connect.Controllers
{
    public class CommunitiesController : Controller
    {
        private readonly InterestGroupService _interestGroupService;
        public CommunitiesController(InterestGroupService interestGroupService) 
        {
            _interestGroupService = interestGroupService;
        }
        public async Task<IActionResult> CommunityPage()
        {
            var interestGroupVM = new InterestGroupViewModel();
            string userID = HttpContext.Session.GetString("uId");
            interestGroupVM = await _interestGroupService.populateInterestGVM(userID);
            //interestGroupVM.InterestGroups.RemoveAll(x => x == null);
            return View(interestGroupVM);
        }


        [HttpPost]
        public async Task<IActionResult> joinGroup(string groupId, string userId)
        {
            bool bFlag = false;
            bFlag = await _interestGroupService.userJoinsGroup(groupId, userId);
            if (bFlag)
            {
                TempData["Success"] = "Successfully joined group!";
                return RedirectToAction("CommunityPage");
            }
            else {
                TempData["Failure"] = "Failed to join group, please try again later";
                return RedirectToAction("CommunityPage");

            }
        }
    }
}
