namespace Algorithmix.Domain
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
        public string Role { get; set; }
    }
}
