using Algorithmix.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Algorithmix.Identity
{
    public class IdentityHelper
    {
        public string GetAccessToken(string authorization)
        {
            return authorization.Replace("Bearer ", "");
        }

        public JwtSecurityToken GetAccessJwtToken(string authorization)
        {
            var accessToken = GetAccessToken(authorization);
            return new JwtSecurityToken(accessToken);
        }

        public ApplicationUser GetUser(JwtSecurityToken accessToken)
        {
            return new ApplicationUser
            {
                Id = accessToken.Claims.FirstOrDefault(c => c.Type == "primarysid").Value,
                Email = accessToken.Claims.FirstOrDefault(c => c.Type == "email").Value,
                Role = accessToken.Claims.FirstOrDefault(c => c.Type == "role").Value
            };
        }

        public ApplicationUser GetUser(string authorization)
        {
            var accessToken = GetAccessJwtToken(authorization);

            return new ApplicationUser
            {
                Id = accessToken.Claims.FirstOrDefault(c => c.Type == "primarysid").Value,
                Email = accessToken.Claims.FirstOrDefault(c => c.Type == "email").Value,
                Role = accessToken.Claims.FirstOrDefault(c => c.Type == "role").Value
            };
        }
    }
}
