using Algorithmix.Mappers;
using Algorithmix.Models.Algorithms;
using Algorithmix.Repository;
using Algorithmix.Services.TestDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class AlgorithmService
    {
        private readonly AlgorithmMapper _algorithmMapper;
        private readonly AlgorithmTimeComplexityMapper _algorithmTimeComplexityMapper;
        private readonly AlgorithmRepository _algorithmRepository;
        private readonly AlgorithmTimeComplexityRepository _algorithmTimeComplexityRepository;
        private readonly TestService _testService;

        public AlgorithmService(
            AlgorithmMapper algorithmMapper,
            AlgorithmTimeComplexityMapper algorithmTimeComplexityMapper,
            AlgorithmRepository algorithmRepository,
            AlgorithmTimeComplexityRepository algorithmTimeComplexityRepository,
            TestService testService)
        {
            _algorithmMapper = algorithmMapper;
            _algorithmTimeComplexityMapper = algorithmTimeComplexityMapper;
            _algorithmRepository = algorithmRepository;
            _algorithmTimeComplexityRepository = algorithmTimeComplexityRepository;
            _testService = testService;
        }

        public async Task<bool> Exists(string algorithmId)
        {
            return await _algorithmRepository.GetAlgorithmById(algorithmId) != null;
        }

        public async Task<Algorithm> GetAlgorithm(string algorithmId)
        {
            var algorithmEntity = await _algorithmRepository.GetAlgorithmById(algorithmId);
            var timeComplexityEntity = await _algorithmTimeComplexityRepository.GetAlgorithmTimeComplexityById(algorithmEntity.TimeComplexityId);
            var timeComplexity = _algorithmTimeComplexityMapper.ToDomain(timeComplexityEntity);
            var tests = await _testService.GetTests(algorithmId);

            return _algorithmMapper.ToDomain(algorithmEntity, timeComplexity, tests);
        }

        public async Task<IEnumerable<Algorithm>> GetAllAlgorithms()
        {
            var algorithmEntities = await _algorithmRepository.GetAllAlgorithms();
            var timeComplexityEntities = await _algorithmTimeComplexityRepository.GetAllAlgorithmTimeComplexities();
            var timeComplexities = _algorithmTimeComplexityMapper.ToDomainCollection(timeComplexityEntities);
            var algorithmIds = algorithmEntities.Select(a => a.Id);
            var tests = await _testService.GetTests(algorithmIds);

            return _algorithmMapper.ToDomainCollection(algorithmEntities, timeComplexities, tests)
                .OrderBy(a => a.TimeComplexityId);
        }
    }
}
