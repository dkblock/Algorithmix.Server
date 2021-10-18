using Algorithmix.Identity;
using Algorithmix.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Algorithmix.Api.Core
{
    public interface IUserContextManager
    {
        ApplicationUser CurrentUser { get; }
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

        public ApplicationUser CurrentUser
        {
            get { return GetCurrentUser(); }
        }

        private ApplicationUser GetCurrentUser()
        {
            if (_httpContextAccessor == null)
                return null;

            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorization);

            if (authorization != "null")
                return _identityHelper.GetUser(authorization.ToString());

            return null;
        }
    }
}
