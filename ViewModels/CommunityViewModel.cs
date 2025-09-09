using Campus_Connect.Models;

namespace Campus_Connect.ViewModels
{
    public class CommunityViewModel
    {
        public User currentUser { get; set; }
        public List<Community> communities { get; set; }
    }
}
