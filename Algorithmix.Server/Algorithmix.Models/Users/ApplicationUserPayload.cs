namespace Algorithmix.Models.Users
{
    public class ApplicationUserPayload
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CurrentPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? GroupId { get; set; }
        public string Role { get; set; }
    }
}
