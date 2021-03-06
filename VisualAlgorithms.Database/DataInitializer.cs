using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Database
{
    public static class DataInitializer
    {
        public static async Task Initialize(ApplicationContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUserEntity> userManager)
        {
            await InitializeRoles(roleManager);
            await InitializeGroups(context);

            // Only on initial launch
            // await InitializeAdministratorAccount(userManager);
        }

        private static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            var applicationRoles = new List<string> 
            { 
                Roles.Administrator,
                Roles.Moderator,
                Roles.User
            };

            foreach (var role in applicationRoles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task InitializeAdministratorAccount(UserManager<ApplicationUserEntity> userManager)
        {
            const string administratorEmail = "ADMINISTRATOR_EMAIL";
            const string administratorPassword = "ADMINISTRATOR_PASSWORD";

            if (await userManager.FindByNameAsync(administratorEmail) == null)
            {
                var administrator = new ApplicationUserEntity { Email = administratorEmail, UserName = administratorEmail };
                var result = await userManager.CreateAsync(administrator, administratorPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(administrator, Roles.Administrator);
            }
        }

        private static async Task InitializeGroups(ApplicationContext context)
        {
            if (!context.Groups.Any())
            {
                var groups = new List<GroupEntity>
                {
                    new GroupEntity { Name = "Не назначено", IsAvailableForRegister = false },
                    new GroupEntity { Name = "Администраторы", IsAvailableForRegister = false },
                    new GroupEntity { Name = "Модераторы", IsAvailableForRegister = false }
                };

                await context.Groups.AddRangeAsync(groups);
                await context.SaveChangesAsync();
            }
        }
    }
}
