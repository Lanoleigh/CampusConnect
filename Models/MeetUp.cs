namespace Campus_Connect.Models
{
    public class MeetUp
    {
        public string MeetUpId { get; set; }
        public string Name { get; set; }
        public string  Purpose { get; set; }
        public string relatedID { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public Dictionary<string,bool>? Members { get; set; }
    }
}
