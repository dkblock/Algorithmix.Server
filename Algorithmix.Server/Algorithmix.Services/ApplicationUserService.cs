using Algorithmix.Common.Constants;
using Algorithmix.Common.Extensions;
using Algorithmix.Entities;
using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class ApplicationUserService
    {
        private readonly ApplicationUserMapper _userMapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;

        public ApplicationUserService(ApplicationUserMapper userMapper, UserManager<ApplicationUserEntity> userManager)
        {
            _userMapper = userMapper;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> CreateUser(RegisterPayload registerPayload)
        {
            var userEntity = _userMapper.ToEntity(registerPayload);
            await _userManager.CreateAsync(userEntity, registerPayload.Password);
            await _userManager.AddToRoleAsync(userEntity, Roles.User);

            return await PrepareUser(userEntity);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);

            if (userEntity == null)
                return null;

            return await PrepareUser(userEntity);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);

            if (userEntity == null)
                return null;

            return await PrepareUser(userEntity);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var userEntities = await _userManager.Users.ToListAsync();
            var preparedUsers = new List<ApplicationUser>();
            await userEntities.ForEachAsync(async userEntity => preparedUsers.Add(await PrepareUser(userEntity)));

            return preparedUsers;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByGroup(int groupId)
        {
            var userEntities = await _userManager.Users.ToListAsync();
            var usersInGroup = userEntities.Where(user => user.GroupId == groupId);
            var preparedUsers = new List<ApplicationUser>();
            await usersInGroup.ForEachAsync(async user => preparedUsers.Add(await PrepareUser(user)));

            return preparedUsers;
        }

        public async Task<bool> Exists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }

        public async Task<bool> IsPasswordValid(string email, string password)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);

            if (userEntity != null)
                return await _userManager.CheckPasswordAsync(userEntity, password);

            return false;
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser> UpdateUser(string id, ApplicationUserPayload userPayload)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var updatedUserEntity = _userMapper.UpdateEntity(userEntity, userPayload);

            await _userManager.ChangeRoleAsync(userEntity, userPayload.Role);
            await _userManager.UpdateAsync(updatedUserEntity);

            var updatedUser = await _userManager.FindByIdAsync(id);

            return await PrepareUser(updatedUser);
        }

        public async Task UpdateUsersGroup(int oldGroupId, int newGroupId)
        {
            var userEntities = await _userManager.Users.ToListAsync();
            var usersInGroup = userEntities.Where(user => user.GroupId == oldGroupId);

            await usersInGroup.ForEachAsync(async user =>
            {
                user.GroupId = newGroupId;
                await _userManager.UpdateAsync(user);
            });
        }

        private async Task<ApplicationUser> PrepareUser(ApplicationUserEntity userEntity)
        {
            var user = _userMapper.ToModel(userEntity);
            user.Role = await _userManager.GetRoleAsync(userEntity);

            return user;
        }
    }
}
