using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class TestAlgorithmService
    {
        private readonly TestAlgorithmRepository _testAlgorithmRepository;
        private readonly TestAlgorithmMapper _testAlgorithmMapper;

        public TestAlgorithmService(TestAlgorithmRepository testAlgorithmRepository, TestAlgorithmMapper testAlgorithmMapper)
        {
            _testAlgorithmRepository = testAlgorithmRepository;
            _testAlgorithmMapper = testAlgorithmMapper;
        }

        public async Task CreateTestAlgorithms(int testId, IEnumerable<string> algorithmIds)
        {
            foreach (var algorithmId in algorithmIds)
            {
                var entity = _testAlgorithmMapper.ToEntity(testId, algorithmId);
                await _testAlgorithmRepository.CreateTestAlgorithm(entity);
            }
        }

        public async Task<IEnumerable<TestAlgorithm>> GetTestAlgorithms(int testId)
        {
            var testAlgorithmEntities = await _testAlgorithmRepository.GetTestAlgorithms(ta => ta.TestId == testId);
            return _testAlgorithmMapper.ToModelsCollection(testAlgorithmEntities);
        }

        public async Task<IEnumerable<TestAlgorithm>> GetTestAlgorithms(string algorithmId)
        {
            var testAlgorithmEntities = await _testAlgorithmRepository.GetTestAlgorithms(ta => ta.AlgorithmId == algorithmId);
            return _testAlgorithmMapper.ToModelsCollection(testAlgorithmEntities);
        }

        public async Task DeleteTestAlgorithms(int testId)
        {
            var testAlgorithms = await GetTestAlgorithms(testId);

            foreach (var testAlgorithm in testAlgorithms)
                await _testAlgorithmRepository.DeleteTestAlgorithm(testAlgorithm.Id);
        }

        public async Task DeleteTestAlgorithms(string algorithmId)
        {
            var testAlgorithms = await GetTestAlgorithms(algorithmId);

            foreach (var testAlgorithm in testAlgorithms)
                await _testAlgorithmRepository.DeleteTestAlgorithm(testAlgorithm.Id);
        }

        public async Task UpdateTestAlgorithms(int testId, IEnumerable<string> algorithmIds)
        {
            var currentTestAlgorithms = await GetTestAlgorithms(testId);
            var currentAlgorithmIds = currentTestAlgorithms.Select(ta => ta.AlgorithmId);

            if (!currentAlgorithmIds.SequenceEqual(algorithmIds))
            {
                await DeleteTestAlgorithms(testId);
                await CreateTestAlgorithms(testId, algorithmIds);
            }
        }
    }
}
