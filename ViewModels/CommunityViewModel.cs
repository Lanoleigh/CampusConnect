using Campus_Connect.Models;
using FirebaseAdmin.Auth;

namespace Campus_Connect.ViewModels
{
    public class CommunityViewModel
    {
        public UserRecord User { get; set; }
        public List<InterestGroup>? InterestGroups { get; set; }
        public Community CommunityJoined { get; set; }
        public List<FirebasePost>? Posts { get; set; }
        public List<MeetUp> meetUps { get; set; }
    }
}
