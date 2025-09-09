namespace Campus_Connect.Models
{
    public class InterestGroup
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, bool> Users { get; set; }
    }
}
