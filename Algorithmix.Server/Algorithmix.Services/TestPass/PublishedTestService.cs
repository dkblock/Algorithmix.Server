using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository.TestPass;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Services.TestPass
{
    public class PublishedTestService
    {
        private readonly TestMapper _testMapper;
        private readonly PublishedTestRepository _testRepository;

        public PublishedTestService(TestMapper testMapper, PublishedTestRepository testRepository)
        {
            _testMapper = testMapper;
            _testRepository = testRepository;
        }

        public async Task<Test> CreateTest(Test test)
        {
            var testEntity = _testMapper.ToEntity(test);
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

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            var testEntities = await _testRepository.GetAllTests();
            return _testMapper.ToModelsCollection(testEntities);
        }

        public async Task DeleteTest(int id)
        {
            await _testRepository.DeleteTest(id);
        }

        public async Task<Test> UpdateTest(Test test)
        {
            var testEntity = _testMapper.ToEntity(test);
            var updatedTest = await _testRepository.UpdateTest(testEntity);

            return _testMapper.ToModel(updatedTest);
        }
    }
}
