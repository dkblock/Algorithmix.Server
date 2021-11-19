using Algorithmix.Api.Core.Helpers;
using Algorithmix.Models;
using Algorithmix.Models.Account;
using Algorithmix.Models.Users;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class ApplicationUserManager
    {
        private readonly ApplicationUserService _userService;
        private readonly GroupService _groupService;
        private readonly QueryHelper _queryHelper;

        public ApplicationUserManager(ApplicationUserService userService, GroupService groupService, QueryHelper queryHelper)
        {
            _userService = userService;
            _groupService = groupService;
            _queryHelper = queryHelper;
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

        public async Task<PageResponse<ApplicationUser>> GetUsers(ApplicationUserQuery query)
        {
            var users = await _userService.GetUsers();
            return await PrepareUsers(users, query);
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

        public async Task<bool> ConfirmEmail(string id, string code)
        {
            return await _userService.ConfirmEmail(id, code);
        }

        public async Task<bool> ChangePassword(string id, ChangePasswordPayload changePasswordPayload)
        {
            return await _userService.ChangePassword(id, changePasswordPayload);
        }

        public async Task<bool> ResetPassword(string id, ResetPasswordPayload resetPasswordPayload)
        {
            return await _userService.ResetPassword(id, resetPasswordPayload);
        }

        public async Task<string> GenerateEmailConfirmationToken(string id)
        {
            return await _userService.GenerateEmailConfirmationToken(id);
        }

        public async Task<string> GeneratePasswordResetToken(string id)
        {
            return await _userService.GeneratePasswordResetToken(id);
        }

        private async Task<ApplicationUser> PrepareUser(ApplicationUser user)
        {
            if (user == null)
                return null;

            user.Group = await _groupService.GetGroup(user.Group.Id);
            return user;
        }

        private async Task<PageResponse<ApplicationUser>> PrepareUsers(IEnumerable<ApplicationUser> users, ApplicationUserQuery query)
        {
            var preparedUsers = new List<ApplicationUser>();
            
            foreach(var user in users)
            {
                var preparedUser = await PrepareUser(user);

                var filters = new[]
                {
                    preparedUser.Email,
                    preparedUser.FirstName,
                    preparedUser.LastName,
                    $"{preparedUser.FirstName} {preparedUser.LastName}",
                    $"{preparedUser.LastName} {preparedUser.FirstName}",
                    preparedUser.Group.Name,
                    preparedUser.Role
                };

                if (!_queryHelper.IsMatch(query.SearchText, filters))
                    continue;

                if (query.GroupId != -1 && query.GroupId != preparedUser.Group.Id)
                    continue;

                if (query.Role != "all" && query.Role != preparedUser.Role)
                    continue;

                preparedUsers.Add(preparedUser);
            }

            var sortedUsers = query.SortByDesc
                ? preparedUsers.OrderByDescending(_queryHelper.ApplicationUserSortModel[query.SortBy])
                : preparedUsers.OrderBy(_queryHelper.ApplicationUserSortModel[query.SortBy]);

            var result = sortedUsers.Skip(query.PageSize * (query.PageIndex - 1));

            return new PageResponse<ApplicationUser>
            {
                Page = result.Take(query.PageSize),
                TotalCount = sortedUsers.Count()
            };
        }
    }
}
