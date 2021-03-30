using Microsoft.AspNetCore.Identity;

namespace Algorithmix.Entities
{
    public class ApplicationUserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GroupId { get; set; }
    }
}
