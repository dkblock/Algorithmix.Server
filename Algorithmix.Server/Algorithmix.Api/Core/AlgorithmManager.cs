﻿using Algorithmix.Api.Core.Helpers;
using Algorithmix.Models.Algorithms;
using Algorithmix.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class AlgorithmManager
    {
        private readonly IAlgorithmDataManager _algorithmDataManager;
        private readonly AlgorithmService _algorithmService;
        private readonly AlgorithmTimeComplexityService _algorithmTimeComplexityService;
        private readonly QueryHelper _queryHelper;
        private readonly IWebHostEnvironment _env;

        public AlgorithmManager(
            IAlgorithmDataManager algorithmDataManager,
            AlgorithmService algorithmService,
            AlgorithmTimeComplexityService algorithmTimeComplexityService,
            QueryHelper queryHelper,
            IWebHostEnvironment env)
        {
            _algorithmDataManager = algorithmDataManager;
            _algorithmService = algorithmService;
            _algorithmTimeComplexityService = algorithmTimeComplexityService;
            _queryHelper = queryHelper;
            _env = env;
        }

        public async Task<Algorithm> CreateAlgorithm(AlgorithmPayload algorithmPayload)
        {
            var timeComplexity = await _algorithmTimeComplexityService.CreateAlgorithmTimeComplexity(algorithmPayload.Id);

            algorithmPayload.TimeComplexityId = timeComplexity.Id;
            algorithmPayload.ImageUrl = _algorithmDataManager.DefaultAlgorithmImageUrl;

            var createdAlgorithm = await _algorithmService.CreateAlgorithm(algorithmPayload);
            return await PrepareAlgorithm(createdAlgorithm);
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

        public async Task DeleteAlgorithm(string id)
        {
            await _algorithmService.DeleteAlgorithm(id);
        }

        public async Task<Algorithm> UpdateAlgorithm(string id, AlgorithmPayload algorithmPayload)
        {
            var updatedAlgorithm = await _algorithmService.UpdateAlgorithm(id, algorithmPayload);
            return await PrepareAlgorithm(updatedAlgorithm);
        }

        public async Task<Algorithm> UpdateAlgorithmImage(string algorithmId, IFormFile image)
        {
            var imagePath = _algorithmDataManager.CreateAlgorithmImage(algorithmId, image);
            var updatedAlgorithm = await _algorithmService.UpdateAlgorithmImage(algorithmId, imagePath);

            return await PrepareAlgorithm(updatedAlgorithm);
        }

        public async Task ClearAlgorithmImage(string algorithmId)
        {
            var algorithm = await _algorithmService.GetAlgorithm(algorithmId);
            _algorithmDataManager.DeleteAlgorithmImage(algorithm.ImageUrl);

            await _algorithmService.UpdateAlgorithmImage(algorithmId, _algorithmDataManager.DefaultAlgorithmImageUrl);
        }

        private async Task<Algorithm> PrepareAlgorithm(Algorithm algorithm)
        {
            algorithm.TimeComplexity = await _algorithmTimeComplexityService.GetAlgorithmTimeComplexity(algorithm.TimeComplexityId);
            algorithm.HasConstructor = Directory.Exists(Path.Combine(_env.WebRootPath, "algorithms", algorithm.Id, "constructor"));
            algorithm.HasDescription = Directory.Exists(Path.Combine(_env.WebRootPath, "algorithms", algorithm.Id, "description"));

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
