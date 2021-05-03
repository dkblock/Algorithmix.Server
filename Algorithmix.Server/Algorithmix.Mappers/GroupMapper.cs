using Algorithmix.Entities;
using Algorithmix.Models.Groups;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class GroupMapper
    {
        public GroupEntity ToEntity(GroupPayload groupPayload, int? id = null)
        {
            return new GroupEntity
            {
                Id = id ?? 0,
                Name = groupPayload.Name,
                IsAvailableForRegister = groupPayload.IsAvailableForRegister
            };
        }

        public Group ToModel(GroupEntity groupEntity)
        {
            return new Group
            {
                Id = groupEntity.Id,
                Name = groupEntity.Name,
                IsAvailableForRegister = groupEntity.IsAvailableForRegister
            };
        }

        public IEnumerable<Group> ToModelsCollection(IEnumerable<GroupEntity> groupEntities)
        {
            return groupEntities.Select(entity => ToModel(entity));
        }
    }
}
