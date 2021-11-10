using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository.TestDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services.TestDesign
{
    public class TestService
    {
        private readonly TestMapper _testMapper;
        private readonly TestRepository _testRepository;

        public TestService(TestMapper testMapper, TestRepository testRepository)
        {
            _testMapper = testMapper;
            _testRepository = testRepository;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            var testEntity = _testMapper.ToEntity(testPayload);
            var createdTest = await _testRepository.CreateTest(testEntity);

            return _testMapper.ToModel(createdTest);
        }

        public async Task<bool> Exists(int id)
        {
            return await _testRepository.GetTestById(id) != null;
        }

        public async Task<Test> GetTest(int id)
        {
            var testEntity = await _testRepository.GetTestById(id);
            return _testMapper.ToModel(testEntity);
        }

        public async Task<IEnumerable<Test>> GetTests(IEnumerable<int> ids)
        {
            var testEntities = await _testRepository.GetTests(t => ids.Contains(t.Id));
            return _testMapper.ToModelsCollection(testEntities);
        }

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            var testEntities = await _testRepository.GetAllTests();
            return _testMapper.ToModelsCollection(testEntities);
        }

        public async Task DeleteTest(int id)
        {
            await _testRepository.DeleteTest(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            var testEntity = await _testRepository.GetTestById(id);
            var updatedTestEntity = _testMapper.UpdateEntity(testEntity, testPayload);
            var updatedTest = await _testRepository.UpdateTest(updatedTestEntity);

            return _testMapper.ToModel(updatedTest);
        }

        public async Task UpdateTest(int id, bool isPublished = false)
        {
            var testEntity = await _testRepository.GetTestById(id);
            testEntity.IsPublished = isPublished;
            testEntity.UpdatedDate = DateTime.Now;
            await _testRepository.UpdateTest(testEntity);
        }
    }
}
