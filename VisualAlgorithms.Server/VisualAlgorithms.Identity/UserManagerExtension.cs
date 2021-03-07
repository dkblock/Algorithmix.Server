using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Identity
{
    public static class UserManagerExtension
    {
        public static async Task<string> GetRoleAsync(this UserManager<ApplicationUserEntity> userManager, ApplicationUserEntity user)
        {
            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.Administrator))
                return Roles.Administrator;

            if (roles.Contains(Roles.Moderator))
                return Roles.Administrator;

            return Roles.User;
        }
    }
}
