namespace Campus_Connect.Models
{
    public class Community
    {
        public string CommunityID { get; set; }
        public string FacultyName { get; set; }
        public List<User> Memebers { get; set; }
    }
}
