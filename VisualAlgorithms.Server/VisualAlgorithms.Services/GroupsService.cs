using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
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
