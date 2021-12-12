using Algorithmix.Models.Users;

namespace Algorithmix.Models.Account
{
    public class UserAccount
    {
        public ApplicationUser CurrentUser { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
