namespace Campus_Connect.Models
{
    public class User
    {
        public string? UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Faculty { get; set; } //this is the communityID
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }

    }
}
