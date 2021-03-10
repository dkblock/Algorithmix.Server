using VisualAlgorithms.Domain;

namespace VisualAlgorithms.Models.Account
{
    public class AuthModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public string AccessToken { get; set; }
    }
}
