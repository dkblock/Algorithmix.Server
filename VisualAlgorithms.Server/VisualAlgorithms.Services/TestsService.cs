using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestsService
    {
        private readonly TestsMapper _testsMapper;
        private readonly TestsRepository _testsRepository;

        public TestsService(TestsMapper testsMapper, TestsRepository testsRepository)
        {
            _testsMapper = testsMapper;
            _testsRepository = testsRepository;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            var testEntity = _testsMapper.ToEntity(testPayload);
            var createdTest = await _testsRepository.CreateTest(testEntity);

            return _testsMapper.ToModel(createdTest);
        }

        public async Task<bool> Exists(int id)
        {
            return await _testsRepository.GetTestById(id) != null;
        }

        public async Task<Test> GetTest(int id)
        {
            var testEntity = await _testsRepository.GetTestById(id);
            return _testsMapper.ToModel(testEntity);
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var testEntities = await _testsRepository.GetAllTests();
            return _testsMapper.ToModelsCollection(testEntities);
        }

        public async Task<IEnumerable<Test>> GetTests(string algorithmId)
        {
            var testEntities = await _testsRepository.GetTests(t => t.AlgorithmId == algorithmId);
            return _testsMapper.ToModelsCollection(testEntities);
        }

        public async Task<IEnumerable<Test>> GetTests(IEnumerable<string> algorithmIds)
        {
            var testEntities = await _testsRepository.GetTests(t => algorithmIds.Contains(t.AlgorithmId));
            return _testsMapper.ToModelsCollection(testEntities);
        }

        public async Task DeleteTest(int id)
        {
            await _testsRepository.DeleteTest(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            var testEntity = _testsMapper.ToEntity(testPayload, id);
            var updatedTest = await _testsRepository.UpdateTest(testEntity);

            return _testsMapper.ToModel(updatedTest);
        }
    }
}
