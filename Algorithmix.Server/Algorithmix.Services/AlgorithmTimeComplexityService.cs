using Algorithmix.Entities;
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

        public async Task<AlgorithmTimeComplexity> CreateAlgorithmTimeComplexity(string algorithmId)
        {
            var timeComplexityEntity = new AlgorithmTimeComplexityEntity { AlgorithmId = algorithmId };
            var timeComplexity = await _algorithmTimeComplexityRepository.CreateAlgorithmTimeComplexity(timeComplexityEntity);

            return _algorithmTimeComplexityMapper.ToModel(timeComplexity);
        }

        public async Task<AlgorithmTimeComplexity> GetAlgorithmTimeComplexity(int id)
        {
            var algorithmTimeComplexityEntity = await _algorithmTimeComplexityRepository.GetAlgorithmTimeComplexityById(id);
            var algorithmTimeComplexity = _algorithmTimeComplexityMapper.ToModel(algorithmTimeComplexityEntity);

            return algorithmTimeComplexity;
        }

        public async Task DeleteAlgorithmTimeComplexity(int id)
        {
            await _algorithmTimeComplexityRepository.DeleteAlgorithmTimeComplexity(id);
        }

        public async Task<AlgorithmTimeComplexity> UpdateAlgorithmTimeComplexity(int id, AlgorithmTimeComplexityPayload timeComplexityPayload)
        {
            var timeComplexityEntity = _algorithmTimeComplexityMapper.ToEntity(timeComplexityPayload, id);
            var updatedTimeComplexity = await _algorithmTimeComplexityRepository.UpdateAlgorithmTimeComplexity(timeComplexityEntity);

            return _algorithmTimeComplexityMapper.ToModel(updatedTimeComplexity);
        }
    }
}
