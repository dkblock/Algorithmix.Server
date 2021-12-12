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

        public async Task Invoke(HttpContext context, IUserContextHandler userContextHandler)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            AttachUserToContext(token, userContextHandler);
            await _next(context);
        }

        private void AttachUserToContext(string token, IUserContextHandler userContextHandler)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secret = Encoding.ASCII.GetBytes(_identitySettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                userContextHandler.AttachUser(jwtToken);
            }
            catch (Exception e)
            {
                userContextHandler.DetachUser();
            }
        }
    }
}
