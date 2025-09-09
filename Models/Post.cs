namespace Campus_Connect.Models
{
    public class Post
    {
        public string? PostID { get; set; }
        public string Title { get; set; }
        public string? UserID { get; set; }
        public string PostType { get; set; }
        public string AdditionalMessage { get; set; }
        public IFormFile? PostFile { get; set; }
        public string? FileUrl { get; set; }
        public string Visibility { get; set; } //public (everyone/the community you belong to or interest groups you joined)
        public DateTime PostDate { get; set; }
    }
}
