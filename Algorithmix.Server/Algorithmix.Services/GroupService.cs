using Algorithmix.Repository;

namespace Algorithmix.Services
{
    public class GroupService
    {
        private readonly GroupRepository _groupRepository;

        public GroupService(GroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
    }
}
