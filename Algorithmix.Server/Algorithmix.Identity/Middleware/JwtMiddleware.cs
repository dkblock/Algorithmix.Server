using Algorithmix.Identity.Core;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Identity.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
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
