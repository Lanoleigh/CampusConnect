using Campus_Connect.Models;
using Campus_Connect.ViewModels;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Auth;

namespace Campus_Connect.Services
{
    public class CommunityFeedService
    {
        private readonly FirebaseClient _db;
        private readonly FirebaseAuth _auth;

        public CommunityFeedService()
        {
            _db = new FirebaseClient("https://campus-connect-14d4f-default-rtdb.firebaseio.com/");
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async Task<CommunityViewModel> FilterCommunitiesAndInterestGroups(string userID)
        {
            
            var currentUser = await _auth.GetUserAsync(userID);
            var commVM = new CommunityViewModel();
            var communities = await _db
                .Child("Communities")
                .OnceAsync<Community>();
            var joinedCommunity = new Community();

            foreach(var community in communities)
            {
                if (community.Object.Users != null)
                {
                    foreach (var user in community.Object.Users)
                    {
                        if (user.Value && user.Key.Equals(userID)){
                            joinedCommunity.CommunityID = community.Object.CommunityID;
                            joinedCommunity.FacultyName = community.Object.FacultyName;
                        }
                    }
                }
            }

            var interstGroups = await _db
                .Child("InterestGroups")
                .OnceAsListAsync<InterestGroup>();
           List<InterestGroup> iGroups = interstGroups
                .Select(group => group.Object)
                .ToList();

            List<InterestGroup> myGroups = new List<InterestGroup>();
            foreach(var intGroup in iGroups)
            {
                if (intGroup != null && intGroup.Users != null && intGroup.Users.ContainsKey(userID))
                {
                    myGroups.Add(intGroup);
                }
            }
            myGroups.RemoveAll(x => x == null);
           
                var posts = await _db
                    .Child("Posts")
                    .OnceAsync<FirebasePost>();
                List<FirebasePost> allPosts = posts
                    .Select(post => post.Object)
                    .ToList();


            var allMeetUps = await _db.Child("Meetup").OnceAsync<MeetUp>();
            
            var meetUps = allMeetUps
                .Select(m => m.Object)
                .Where(m =>
                m.relatedID == joinedCommunity.CommunityID ||
                myGroups.Any(g => g.ID == m.relatedID)
                )
                .ToList();

            

            commVM.User = currentUser;
            commVM.CommunityJoined = joinedCommunity;
            commVM.InterestGroups = myGroups;//return all interest groups and we can filter it afterwards
            commVM.Posts = allPosts;
            commVM.meetUps = meetUps;


            return commVM;
        }
    }
}
