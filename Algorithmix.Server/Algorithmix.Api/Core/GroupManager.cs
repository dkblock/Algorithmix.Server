using Algorithmix.Api.Core.Helpers;
using Algorithmix.Models;
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
        private readonly QueryHelper _queryHelper;

        public GroupManager(ApplicationUserService userService, GroupService groupService, QueryHelper queryHelper)
        {
            _userService = userService;
            _groupService = groupService;
            _queryHelper = queryHelper;
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

        public async Task<PageResponse<Group>> GetGroups(GroupQuery query)
        {
            var groups = await _groupService.GetAllGroups();
            return await PrepareGroups(groups, query);
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

        private async Task<PageResponse<Group>> PrepareGroups(IEnumerable<Group> groups, GroupQuery query)
        {
            var preparedGroups = new List<Group>();

            foreach (var group in groups)
            {
                var preparedGroup = await PrepareGroup(group);

                if (!_queryHelper.IsMatch(query.SearchText, new[] { preparedGroup.Name }))
                    continue;

                preparedGroups.Add(preparedGroup);
            }

            var sortedGroups = query.SortByDesc
                ? preparedGroups.OrderByDescending(_queryHelper.GroupSortModel[query.SortBy])
                : preparedGroups.OrderBy(_queryHelper.GroupSortModel[query.SortBy]);

            var result = sortedGroups.Skip(query.PageSize * (query.PageIndex - 1));

            return new PageResponse<Group>
            {
                Page = result.Take(query.PageSize),
                TotalCount = sortedGroups.Count()
            };
        }
    }
}
