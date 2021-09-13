using Algorithmix.Mappers;
using Algorithmix.Models.Algorithms;
using Algorithmix.Repository;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class AlgorithmTimeComplexityService
    {
        private readonly AlgorithmTimeComplexityMapper _algorithmTimeComplexityMapper;
        private readonly AlgorithmTimeComplexityRepository _algorithmTimeComplexityRepository;

        public AlgorithmTimeComplexityService(
            AlgorithmTimeComplexityMapper algorithmTimeComplexityMapper,
            AlgorithmTimeComplexityRepository algorithmTimeComplexityRepository)
        {
            _algorithmTimeComplexityMapper = algorithmTimeComplexityMapper;
            _algorithmTimeComplexityRepository = algorithmTimeComplexityRepository;
        }

        public async Task<AlgorithmTimeComplexity> GetAlgorithmTimeComplexity(int id)
        {
            var algorithmTimeComplexityEntity = await _algorithmTimeComplexityRepository.GetAlgorithmTimeComplexityById(id);
            var algorithmTimeComplexity = _algorithmTimeComplexityMapper.ToModel(algorithmTimeComplexityEntity);

            return algorithmTimeComplexity;
        }
    }
}
