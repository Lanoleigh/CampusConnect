using Campus_Connect.Models;
using Campus_Connect.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Auth;

namespace Campus_Connect.Services
{
    public class InterestGroupService
    {
        private readonly FirebaseAuth _auth;
        private readonly FirebaseClient _db;
        public InterestGroupService()
        {
            _db = new FirebaseClient("https://campus-connect-14d4f-default-rtdb.firebaseio.com/");
            _auth = FirebaseAuth.DefaultInstance;
        }
        

        public async Task<InterestGroupViewModel> populateInterestGVM(string userID)
        {
            var interestGroupViewModel = new InterestGroupViewModel();
            try
            {
                var iG = (await _db.Child("InterestGroups")
                    .OnceAsListAsync<InterestGroup>()).
                    Where(x => x.Object != null).Select(x => x.Object).ToList();

           

                var currentUser = await _auth.GetUserAsync(userID);

                var communities = await _db
      .Child("Communities")
      .OnceAsync<Community>();
                var joinedCommunity = new Community();

                foreach (var community in communities)
                {
                    if (community.Object.Users != null)
                    {
                        foreach (var user in community.Object.Users)
                        {
                            if (user.Value && user.Key.Equals(userID))
                            {
                                joinedCommunity.CommunityID = community.Object.CommunityID;
                                joinedCommunity.FacultyName = community.Object.FacultyName;
                            }
                        }
                    }
                }


                var meetU = await _db.Child("MeetUps").OnceAsync<MeetUp>();
                List<MeetUp> meetUps = new List<MeetUp>();

                 meetUps = meetU
                    .Select(m => m.Object)
                    .Where(m =>
                    m.relatedID == joinedCommunity.CommunityID ||
                    iG.Any(g => g.ID == m.relatedID)
                    )
                    .ToList();

                interestGroupViewModel.meetUps = meetUps;
                interestGroupViewModel.User = currentUser;
                interestGroupViewModel.InterestGroups = iG;
              

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
   
            }
            return interestGroupViewModel;
        }
        public async Task<bool> userJoinsGroup(string groupId, string userId)
        {
            try 
            {
                await _db.
                    Child("InterestGroups").
                    Child(groupId).
                    Child("Users").
                    Child(userId).
                    PutAsync(true);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }
    }
}
