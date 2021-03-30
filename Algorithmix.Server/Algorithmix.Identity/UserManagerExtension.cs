using Algorithmix.Common.Constants;
using Algorithmix.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Algorithmix.Identity
{
    public static class UserManagerExtension
    {
        public static async Task<string> GetRoleAsync(this UserManager<ApplicationUserEntity> userManager, ApplicationUserEntity user)
        {
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.Administrator))
                return Roles.Administrator;

            if (roles.Contains(Roles.Moderator))
                return Roles.Moderator;

            return Roles.User;
        }
    }
}
