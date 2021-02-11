using System.Collections.Generic;
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

        public async Task<IEnumerable<Algorithm>> GetAllAlgorithms()
        {
            var algorithmEntities = await _algorithmsRepository.GetAllAlgorithms();
            var algorithms = _algorithmsMapper.ToDomainCollection(algorithmEntities);
            await algorithms.ForEachAsync(async a =>
            {
                var algorithmTimeComplexityEntity = await _algorithmTimeComplexitiesRepository.GetAlgorithmTimeComplexityById(a.AlgorithmTimeComplexityId);
                a.AlgorithmTimeComplexity = _algorithmTimeComplexitiesMapper.ToDomain(algorithmTimeComplexityEntity);
                a.Tests = await _testsService.GetAllAlgorithmTests(a.Id);
            });

            return algorithms;
        }
    }
}
