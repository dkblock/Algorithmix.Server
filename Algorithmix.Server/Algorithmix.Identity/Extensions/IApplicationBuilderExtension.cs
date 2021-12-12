using Algorithmix.Identity.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Algorithmix.Identity.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static void UseIdentityMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
        }
    }
}
