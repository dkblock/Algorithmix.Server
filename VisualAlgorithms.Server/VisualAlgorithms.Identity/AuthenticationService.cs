using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Models.Account;

namespace VisualAlgorithms.Identity
{
    public class AuthenticationService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly byte[] _secret;

        public AuthenticationService(IOptions<IdentitySettings> identitySettings)
        {
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

        public AuthModel CheckAuth(string accessToken)
        {
            var token = new JwtSecurityToken(accessToken);
            var user = new ApplicationUser
            {
                Id = token.Claims.FirstOrDefault(c => c.Type == "primarysid").Value,
                Email = token.Claims.FirstOrDefault(c => c.Type == "email").Value,
                Role = token.Claims.FirstOrDefault(c => c.Type == "role").Value
            };

            if (token.Payload.Exp > DateTimeOffset.Now.ToUnixTimeSeconds())
                return new AuthModel { AccessToken = accessToken, CurrentUser = user };

            return new AuthModel { AccessToken = Authenticate(user), CurrentUser = user };
        }
    }
}
