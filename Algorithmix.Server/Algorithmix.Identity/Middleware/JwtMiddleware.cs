using Algorithmix.Common.Settings;
using Algorithmix.Identity.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithmix.Identity.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IdentitySettings _identitySettings;

        public JwtMiddleware(RequestDelegate next, IOptions<IdentitySettings> identitySettings)
        {
            _next = next;
            _identitySettings = identitySettings.Value;
        }

        public async Task Invoke(HttpContext context, AuthenticationService authService, IUserContextHandler userContextHandler)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validatedToken = authService.ValidateToken(token);

            if (validatedToken != null)
                userContextHandler.AttachUser(validatedToken);
            else
                userContextHandler.DetachUser();

            await _next(context);
        }
    }
}
