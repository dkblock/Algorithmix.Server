using Algorithmix.Repository;

namespace Algorithmix.Services
{
    public class GroupsService
    {
        private readonly GroupRepository _groupRepository;

        public GroupsService(GroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
    }
}
