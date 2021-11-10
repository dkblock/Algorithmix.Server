﻿using Algorithmix.Api.Core.Helpers;
using Algorithmix.Models.Algorithms;
using Algorithmix.Services;
using Algorithmix.Services.TestDesign;
using Algorithmix.Services.TestPass;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class AlgorithmManager
    {
        private readonly IAlgorithmDataManager _algorithmDataManager;
        private readonly AlgorithmService _algorithmService;
        private readonly AlgorithmTimeComplexityService _algorithmTimeComplexityService;
        private readonly TestAlgorithmService _testAlgorithmService;
        private readonly TestService _testService;
        private readonly PublishedTestService _publishedTestService;
        private readonly QueryHelper _queryHelper;

        public AlgorithmManager(
            IAlgorithmDataManager algorithmDataManager,
            AlgorithmService algorithmService,
            AlgorithmTimeComplexityService algorithmTimeComplexityService,
            TestAlgorithmService testAlgorithmService,
            TestService testService,
            PublishedTestService publishedTestService,
            QueryHelper queryHelper)
        {
            _algorithmDataManager = algorithmDataManager;
            _algorithmService = algorithmService;
            _algorithmTimeComplexityService = algorithmTimeComplexityService;
            _testAlgorithmService = testAlgorithmService;
            _testService = testService;
            _publishedTestService = publishedTestService;
            _queryHelper = queryHelper;
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
            var algorithm = await _algorithmService.GetAlgorithm(id);
            var testAlgorithms = await _testAlgorithmService.GetTestAlgorithms(algorithm.Id);
            var testIds = testAlgorithms.Select(ta => ta.TestId);

            await _testAlgorithmService.DeleteTestAlgorithms(algorithm.Id);

            foreach (var testId in testIds)
            {
                await _testService.DeleteTest(testId);
                await _publishedTestService.DeleteTest(testId);
            }

            await _algorithmService.DeleteAlgorithm(id);
            _algorithmDataManager.DeleteAlgorithmDataFolder(id);
            _algorithmDataManager.DeleteAlgorithmImage(algorithm.ImageUrl);
        }

        public async Task<Algorithm> UpdateAlgorithm(string id, AlgorithmPayload algorithmPayload)
        {
            var updatedAlgorithm = await _algorithmService.UpdateAlgorithm(id, algorithmPayload);
            return await PrepareAlgorithm(updatedAlgorithm);
        }

        public async Task<AlgorithmTimeComplexity> UpdateAlgorithmTimeComplexity(int id, AlgorithmTimeComplexityPayload timeComplexityPayload)
        {
            var updatedAlgorithmTimeComplexity = await _algorithmTimeComplexityService.UpdateAlgorithmTimeComplexity(id, timeComplexityPayload);
            return updatedAlgorithmTimeComplexity;
        }

        public void UpdateAlgorithmDescription(string id, IFormFile description)
        {
            _algorithmDataManager.CreateAlgorithmDescription(id, description);
        }

        public void DeleteAlgorithmDescription(string id)
        {
            _algorithmDataManager.DeleteAlgorithmDescription(id);
        }

        public void UpdateAlgorithmConstructor(string id, IFormFile constructor)
        {
            _algorithmDataManager.CreateAlgorithmConstructor(id, constructor);
        }

        public void DeleteAlgorithmConstructor(string id)
        {
            _algorithmDataManager.DeleteAlgorithmConstructor(id);
        }

        public async Task<Algorithm> UpdateAlgorithmImage(string id, IFormFile image)
        {
            var imagePath = _algorithmDataManager.CreateAlgorithmImage(id, image);
            var updatedAlgorithm = await _algorithmService.UpdateAlgorithmImage(id, imagePath);

            return await PrepareAlgorithm(updatedAlgorithm);
        }

        public async Task ClearAlgorithmImage(string id)
        {
            var algorithm = await _algorithmService.GetAlgorithm(id);
            _algorithmDataManager.DeleteAlgorithmImage(algorithm.ImageUrl);

            await _algorithmService.UpdateAlgorithmImage(id, _algorithmDataManager.DefaultAlgorithmImageUrl);
        }

        public FileStream GetAlgorithmDataTemplate(string id)
        {
            return _algorithmDataManager.GetAlgorithmDataTemplate(id);
        }

        private async Task<Algorithm> PrepareAlgorithm(Algorithm algorithm)
        {
            var testAlgorithms = await _testAlgorithmService.GetTestAlgorithms(algorithm.Id);
            var testIds = testAlgorithms.Select(ta => ta.TestId);

            algorithm.Tests = await _testService.GetTests(testIds);
            algorithm.TimeComplexity = await _algorithmTimeComplexityService.GetAlgorithmTimeComplexity(algorithm.TimeComplexityId);
            algorithm.HasConstructor = _algorithmDataManager.ConstructorExists(algorithm.Id);
            algorithm.HasDescription = _algorithmDataManager.DescriptionExists(algorithm.Id);

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
