using Algorithmix.Identity;
using Algorithmix.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Algorithmix.Api.Core
{
    public interface IUserContextManager
    {
        ApplicationUser CurrentUser { get; }
        string AccessToken { get; }
        string Authorization { get; }
    }

    public class UserContextManager : IUserContextManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityHelper _identityHelper;

        public UserContextManager(IHttpContextAccessor httpContextAccessor, IdentityHelper identityHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityHelper = identityHelper;
        }

        public ApplicationUser CurrentUser => GetCurrentUser();
        public string AccessToken => GetAccessToken();
        public string Authorization => GetAuthorization();

        private string GetAccessToken()
        {
            var authorization = GetAuthorization();
            return _identityHelper.GetAccessToken(authorization);
        }

        private ApplicationUser GetCurrentUser()
        {
            var authorization = GetAuthorization();

            if (authorization != "null")
                return _identityHelper.GetUser(authorization);

            return null;
        }

        private string GetAuthorization()
        {
            if (_httpContextAccessor == null)
                return "null";

            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorization);
            return authorization.ToString();
        }
    }
}
