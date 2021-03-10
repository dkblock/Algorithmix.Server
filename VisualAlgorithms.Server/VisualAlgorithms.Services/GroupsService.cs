using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class GroupsService
    {
        private readonly GroupsRepository _groupsRepository;

        public GroupsService(GroupsRepository groupsRepository)
        {
            _groupsRepository = groupsRepository;
        }
    }
}
