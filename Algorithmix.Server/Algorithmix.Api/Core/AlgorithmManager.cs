using Algorithmix.Common.Helpers;
using Algorithmix.Models.Algorithms;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class AlgorithmManager
    {
        private readonly AlgorithmService _algorithmService;
        private readonly AlgorithmTimeComplexityService _algorithmTimeComplexityService;
        private readonly QueryHelper _queryHelper;

        public AlgorithmManager(AlgorithmService algorithmService, AlgorithmTimeComplexityService algorithmTimeComplexityService)
        {
            _algorithmService = algorithmService;
            _algorithmTimeComplexityService = algorithmTimeComplexityService;
            _queryHelper = new QueryHelper();
        }

        public async Task<bool> Exists(string id)
        {
            return await _algorithmService.Exists(id);
        }

        public async Task<Algorithm> GetAlgorithm(string id)
        {
            var algorithm = await _algorithmService.GetAlgorithm(id);
            return await PrepareAlgorithm(algorithm);
        }

        public async Task<IEnumerable<Algorithm>> GetAlgorithms(AlgorithmQuery query)
        {
            var algorithms = await _algorithmService.GetAllAlgorithms();
            return await PrepareAlgorithms(algorithms, query);
        }

        private async Task<Algorithm> PrepareAlgorithm(Algorithm algorithm)
        {
            algorithm.TimeComplexity = await _algorithmTimeComplexityService.GetAlgorithmTimeComplexity(algorithm.TimeComplexityId);
            return algorithm;
        }

        private async Task<IEnumerable<Algorithm>> PrepareAlgorithms(IEnumerable<Algorithm> algorithms, AlgorithmQuery query)
        {
            var preparedAlgorithms = new List<Algorithm>();

            foreach (var algorithm in algorithms)
            {
                var preparedAlgorithm = await PrepareAlgorithm(algorithm);

                if (!_queryHelper.IsMatch(query.SearchText, new[] { algorithm.Id, algorithm.Name }))
                    continue;

                preparedAlgorithms.Add(preparedAlgorithm);
            }

            return preparedAlgorithms;
        }
    }
}
