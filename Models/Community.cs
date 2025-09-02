namespace Campus_Connect.Models
{
    public class Community
    {
        public string CommunityID { get; set; }
        public string Name { get; set; }
        public string CreatorID { get; set; }
        public string Category { get; set; }
        public List<User> Memebers { get; set; }
    }
}
