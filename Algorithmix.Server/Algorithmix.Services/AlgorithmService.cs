using Algorithmix.Mappers;
using Algorithmix.Models.Algorithms;
using Algorithmix.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class AlgorithmService
    {
        private readonly AlgorithmMapper _algorithmMapper;
        private readonly AlgorithmRepository _algorithmRepository;

        public AlgorithmService(AlgorithmMapper algorithmMapper, AlgorithmRepository algorithmRepository)
        {
            _algorithmMapper = algorithmMapper;
            _algorithmRepository = algorithmRepository;
        }

        public async Task<bool> Exists(string algorithmId)
        {
            return await _algorithmRepository.GetAlgorithmById(algorithmId) != null;
        }

        public async Task<Algorithm> GetAlgorithm(string algorithmId)
        {
            var algorithmEntity = await _algorithmRepository.GetAlgorithmById(algorithmId);
            var algorithm = _algorithmMapper.ToModel(algorithmEntity);

            return algorithm;
        }

        public async Task<IEnumerable<Algorithm>> GetAllAlgorithms()
        {
            var algorithmEntities = await _algorithmRepository.GetAllAlgorithms();
            var algorithms = _algorithmMapper.ToModelsCollection(algorithmEntities);

            return algorithms.OrderBy(a => a.TimeComplexityId);
        }

        public async Task<IEnumerable<Algorithm>> GetAlgorithms(IEnumerable<string> algorithmIds)
        {
            var algorithmEntities = await _algorithmRepository.GetAlgorithms(a => algorithmIds.Contains(a.Id));
            var algorithms = _algorithmMapper.ToModelsCollection(algorithmEntities);

            return algorithms.OrderBy(a => a.TimeComplexityId);
        }
    }
}
