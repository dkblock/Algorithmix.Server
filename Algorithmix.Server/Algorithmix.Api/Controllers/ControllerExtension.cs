using Algorithmix.Identity;
using Algorithmix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Algorithmix.Api.Controllers
{
    public static class ControllerExtension
    {
        private static readonly IdentityHelper _identityHelper = new IdentityHelper();

        public static ApplicationUser GetUser(this Controller controller)
        {
            controller.Request.Headers.TryGetValue("Authorization", out StringValues authorization);

            if (authorization != "null")
                return _identityHelper.GetUser(authorization.ToString());

            return null;
        }
    }
}
