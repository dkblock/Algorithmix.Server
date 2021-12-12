using Algorithmix.Common.Constants;
using Algorithmix.Common.Extensions;
using Algorithmix.Entities;
using Algorithmix.Identity.Extensions;
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

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser> UpdateUser(string id, ApplicationUserPayload userPayload)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var updatedUserEntity = _userMapper.UpdateEntity(userEntity, userPayload);

            if (await _userManager.GetRoleAsync(userEntity) != userPayload.Role)
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

        public async Task<bool> IsPasswordValid(string email, string password)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);

            if (userEntity != null)
                return await _userManager.CheckPasswordAsync(userEntity, password);

            return false;
        }

        public async Task<bool> IsEmailConfirmed(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            return await _userManager.IsEmailConfirmedAsync(userEntity);
        }

        public async Task<bool> ConfirmEmail(string id, string code)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ConfirmEmailAsync(userEntity, code);

            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(string id, ChangePasswordPayload changePasswordPayload)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ChangePasswordAsync(userEntity, changePasswordPayload.CurrentPassword, changePasswordPayload.NewPassword);

            return result.Succeeded;
        }

        public async Task<bool> ResetPassword(string id, ResetPasswordPayload resetPasswordPayload)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ResetPasswordAsync(userEntity, resetPasswordPayload.Code, resetPasswordPayload.NewPassword);

            return result.Succeeded;
        }

        public async Task<string> GenerateEmailConfirmationToken(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            return await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
        }

        public async Task<string> GeneratePasswordResetToken(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            return await _userManager.GeneratePasswordResetTokenAsync(userEntity);
        }

        private async Task<ApplicationUser> PrepareUser(ApplicationUserEntity userEntity)
        {
            var user = _userMapper.ToModel(userEntity);
            user.Role = await _userManager.GetRoleAsync(userEntity);

            return user;
        }
    }
}
