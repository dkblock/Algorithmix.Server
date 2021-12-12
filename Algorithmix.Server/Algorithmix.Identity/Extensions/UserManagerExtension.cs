using Algorithmix.Common.Constants;
using Algorithmix.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Algorithmix.Identity.Extensions
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

        public static async Task ChangeRoleAsync(this UserManager<ApplicationUserEntity> userManager, ApplicationUserEntity user, string newRole)
        {
            if (string.IsNullOrEmpty(newRole))
                return;

            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles);
            await userManager.AddToRoleAsync(user, newRole);
        }
    }
}
