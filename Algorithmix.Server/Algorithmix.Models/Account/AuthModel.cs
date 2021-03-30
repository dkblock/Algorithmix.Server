using Algorithmix.Domain;

namespace Algorithmix.Models.Account
{
    public class AuthModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public string AccessToken { get; set; }
    }
}
