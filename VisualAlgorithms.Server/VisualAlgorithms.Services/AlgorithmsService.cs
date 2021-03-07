using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class AlgorithmsService
    {
        private readonly AlgorithmsMapper _algorithmsMapper;
        private readonly AlgorithmTimeComplexitiesMapper _algorithmTimeComplexitiesMapper;
        private readonly AlgorithmsRepository _algorithmsRepository;
        private readonly AlgorithmTimeComplexitiesRepository _algorithmTimeComplexitiesRepository;
        private readonly TestsService _testsService;

        public AlgorithmsService(
            AlgorithmsMapper algorithmsMapper,
            AlgorithmTimeComplexitiesMapper algorithmTimeComplexitiesMapper,
            AlgorithmsRepository algorithmsRepository,
            AlgorithmTimeComplexitiesRepository algorithmTimeComplexitiesRepository,
            TestsService testsService)
        {
            _algorithmsMapper = algorithmsMapper;
            _algorithmTimeComplexitiesMapper = algorithmTimeComplexitiesMapper;
            _algorithmsRepository = algorithmsRepository;
            _algorithmTimeComplexitiesRepository = algorithmTimeComplexitiesRepository;
            _testsService = testsService;
        }

        public async Task<Algorithm> GetAlgorithm(int algorithmId)
        {
            var algorithmEntity = await _algorithmsRepository.GetAlgorithmById(algorithmId);
            var tests = await _testsService.GetTests(algorithmId);

            return _algorithmsMapper.ToDomain(algorithmEntity, tests);
        }

        public async Task<IEnumerable<Algorithm>> GetAllAlgorithms()
        {
            var algorithmEntities = await _algorithmsRepository.GetAllAlgorithms();
            var algorithmIds = algorithmEntities.Select(a => a.Id);
            var tests = await _testsService.GetTests(algorithmIds);

            return _algorithmsMapper.ToDomainCollection(algorithmEntities, tests);
        }
    }
}
