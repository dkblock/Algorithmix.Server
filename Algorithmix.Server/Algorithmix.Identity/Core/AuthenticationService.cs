using Algorithmix.Configuration;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
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
        private readonly SymmetricSecurityKey _secret;
        private readonly string _signingAlgorithm;
        private readonly int _accessTokenLifetime;
        private readonly int _refreshTokenLifetime;

        public AuthenticationService(IConfig configuration)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.IdentitySettings.Secret));
            _signingAlgorithm = SecurityAlgorithms.HmacSha256Signature;
            _accessTokenLifetime = configuration.IdentitySettings.AccessTokenLifetimeInMinutes;
            _refreshTokenLifetime = configuration.IdentitySettings.RefreshTokenLifetimeInDays;
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

        public JwtSecurityToken ValidateToken(string token)
        {
            try
            {
                _tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = _secret,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch
            {
                return null;
            }
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
                SigningCredentials = new SigningCredentials(_secret, _signingAlgorithm)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}
