﻿using Algorithmix.Mappers;
using Algorithmix.Models;
using Algorithmix.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class GroupService
    {
        private readonly GroupMapper _groupMapper;
        private readonly GroupRepository _groupRepository;

        public GroupService(GroupMapper groupMapper, GroupRepository groupRepository)
        {
            _groupMapper = groupMapper;
            _groupRepository = groupRepository;
        }

        public async Task<Group> CreateGroup(GroupPayload groupPayload)
        {
            var groupEntity = _groupMapper.ToEntity(groupPayload);
            var createdGroup = await _groupRepository.CreateGroup(groupEntity);

            return _groupMapper.ToModel(createdGroup);
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            var groupEntities = await _groupRepository.GetAllGroups();
            return _groupMapper.ToModelsCollection(groupEntities);
        }

        public async Task DeleteGroup(int id)
        {
            await _groupRepository.DeleteGroup(id);
        }

        public async Task<Group> UpdateGroup(int id, GroupPayload groupPayload)
        {
            var groupEntity = _groupMapper.ToEntity(groupPayload, id);
            var updatedGroup = await _groupRepository.UpdateGroup(groupEntity);

            return _groupMapper.ToModel(updatedGroup);
        }
    }
}
