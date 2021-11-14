using Algorithmix.Models.Groups;

namespace Algorithmix.Models.Users
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public Group Group { get; set; }
    }
}
