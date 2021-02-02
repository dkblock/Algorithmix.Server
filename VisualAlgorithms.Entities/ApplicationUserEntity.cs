using Microsoft.AspNetCore.Identity;

namespace VisualAlgorithms.Entities
{
    public class ApplicationUserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
