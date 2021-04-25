using Algorithmix.Models;
using Algorithmix.Models.Account;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Algorithmix.Identity
{
    public class AuthenticationService
    {
        private readonly IdentityHelper _identityHelper;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly byte[] _secret;

        public AuthenticationService(IOptions<IdentitySettings> identitySettings)
        {
            _identityHelper = new IdentityHelper();
            _tokenHandler = new JwtSecurityTokenHandler();
            _secret = Encoding.ASCII.GetBytes(identitySettings.Value.Secret);
        }

        public string Authenticate(ApplicationUser user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public AuthModel CheckAuth(string authorization)
        {
            var accessToken = _identityHelper.GetAccessToken(authorization);
            var jwtToken = _identityHelper.GetAccessJwtToken(authorization);
            var user = _identityHelper.GetUser(jwtToken);

            if (jwtToken.Payload.Exp > DateTimeOffset.Now.ToUnixTimeSeconds())
                return new AuthModel { AccessToken = accessToken, CurrentUser = user };

            return new AuthModel { AccessToken = Authenticate(user), CurrentUser = user };
        }
    }
}
