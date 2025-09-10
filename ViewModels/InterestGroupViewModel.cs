using Campus_Connect.Models;
using FirebaseAdmin.Auth;

namespace Campus_Connect.ViewModels
{
    public class InterestGroupViewModel
    {
        public UserRecord? User { get; set; }
        public List<InterestGroup>? InterestGroups {get;set;}
        public List<MeetUp>? meetUps {get;set;}
    }
}
