using Algorithmix.Identity.Middleware;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Algorithmix.Identity.Core
{
    public class AuthenticationService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        private readonly byte[] _secret;
        private readonly int _accessTokenLifetime;
        private readonly int _refreshTokenLifetime;

        public AuthenticationService(IOptions<IdentitySettings> identitySettings)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _secret = Encoding.ASCII.GetBytes(identitySettings.Value.Secret);
            _accessTokenLifetime = identitySettings.Value.AccessTokenLifetimeInMinutes;
            _refreshTokenLifetime = identitySettings.Value.RefreshTokenLifetimeInDays;
        }

        public UserAccount Authenticate(ApplicationUser user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user);

            return new UserAccount
            {
                CurrentUser = user,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateAccessToken(ApplicationUser user)
        {
            return GenerateToken(user, DateTime.Now.AddDays(_accessTokenLifetime));
        }

        private string GenerateRefreshToken(ApplicationUser user)
        {
            return GenerateToken(user, DateTime.Now.AddDays(_refreshTokenLifetime));
        }

        private string GenerateToken(ApplicationUser user, DateTime tokenLifetime)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = tokenLifetime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}
