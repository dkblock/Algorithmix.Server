using Algorithmix.Configuration;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Algorithmix.Identity.Core
{
    public interface IUserContextHandler
    {
        ApplicationUser CurrentUser { get; }
        string AccessToken { get; }

        void AttachUser(JwtSecurityToken jwtToken);
        void AttachUser(UserAccount userAccount);
        void DetachUser();
    }

    public class UserContextHandler : IUserContextHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _refreshTokenLifetime;

        public UserContextHandler(IHttpContextAccessor httpContextAccessor, IConfig configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenLifetime = configuration.IdentitySettings.RefreshTokenLifetimeInDays;
        }

        public ApplicationUser CurrentUser { get; private set; }
        public string AccessToken { get; private set; }

        public void AttachUser(JwtSecurityToken jwtToken)
        {
            CurrentUser = GetUser(jwtToken);
            AccessToken = jwtToken.RawData;
        }

        public void AttachUser(UserAccount userAccount)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("REFRESH_TOKEN", userAccount.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_refreshTokenLifetime)
            });

            CurrentUser = userAccount.CurrentUser;
            AccessToken = userAccount.AccessToken;
            userAccount.RefreshToken = null;
        }

        public void DetachUser()
        {
            CurrentUser = null;
            AccessToken = null;
        }

        private ApplicationUser GetUser(JwtSecurityToken accessToken)
        {
            return new ApplicationUser
            {
                Id = accessToken.Claims.FirstOrDefault(c => c.Type == "primarysid").Value,
                Email = accessToken.Claims.FirstOrDefault(c => c.Type == "email").Value,
                Role = accessToken.Claims.FirstOrDefault(c => c.Type == "role").Value
            };
        }
    }
}
