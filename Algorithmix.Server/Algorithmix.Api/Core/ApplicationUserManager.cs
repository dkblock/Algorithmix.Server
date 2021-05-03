using Algorithmix.Common.Extensions;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class ApplicationUserManager
    {
        private readonly ApplicationUserService _userService;
        private readonly GroupService _groupService;

        public ApplicationUserManager(ApplicationUserService userService, GroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }

        public async Task<ApplicationUser> CreateUser(RegisterPayload registerPayload)
        {
            var user = await _userService.CreateUser(registerPayload);
            return await PrepareUser(user);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            return await PrepareUser(user);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            return await PrepareUser(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return await PrepareUsers(users);
        }

        public async Task<bool> Exists(string id)
        {
            return await _userService.Exists(id);
        }

        public async Task DeleteUser(string id)
        {
            await _userService.DeleteUser(id);
        }

        public async Task<ApplicationUser> UpdateUser(string id, ApplicationUserPayload userPayload)
        {
            return await _userService.UpdateUser(id, userPayload);
        }

        private async Task<ApplicationUser> PrepareUser(ApplicationUser user)
        {
            if (user == null)
                return null;

            user.Group = await _groupService.GetGroup(user.Group.Id);
            return user;
        }

        private async Task<IEnumerable<ApplicationUser>> PrepareUsers(IEnumerable<ApplicationUser> users)
        {
            var preparedUsers = new List<ApplicationUser>();
            await users.ForEachAsync(async user => preparedUsers.Add(await PrepareUser(user)));

            return preparedUsers;
        }
    }
}
