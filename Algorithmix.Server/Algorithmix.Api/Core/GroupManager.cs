using Algorithmix.Common.Extensions;
using Algorithmix.Models.Groups;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class GroupManager
    {
        private readonly ApplicationUserService _userService;
        private readonly GroupService _groupService;

        public GroupManager(ApplicationUserService userService, GroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }

        public async Task<Group> CreateGroup(GroupPayload groupPayload)
        {
            var createdGroup = await _groupService.CreateGroup(groupPayload);
            return await PrepareGroup(createdGroup);
        }

        public async Task<bool> Exists(int id)
        {
            return await _groupService.Exists(id);
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroups();
            return await PrepareGroups(groups);
        }

        public async Task<Group> GetGroup(int id)
        {
            var group = await _groupService.GetGroup(id);
            return await PrepareGroup(group);
        }

        public async Task DeleteGroup(int id)
        {
            var groups = await _groupService.GetAllGroups();
            var newGroupId = groups.Single(group => group.Name == "Не назначена").Id;

            await _userService.UpdateUsersGroup(id, newGroupId);
            await _groupService.DeleteGroup(id);
        }

        public async Task<Group> UpdateGroup(int id, GroupPayload groupPayload)
        {
            var updatedGroup = await _groupService.UpdateGroup(id, groupPayload);
            return await PrepareGroup(updatedGroup);
        }

        private async Task<Group> PrepareGroup(Group group)
        {
            var users = await _userService.GetUsersByGroup(group.Id);
            group.UsersCount = users.Count();

            return group;
        }

        private async Task<IEnumerable<Group>> PrepareGroups(IEnumerable<Group> groups)
        {
            var preparedGroups = new List<Group>();
            await groups.ForEachAsync(async group => preparedGroups.Add(await PrepareGroup(group)));

            return preparedGroups;
        }
    }
}
