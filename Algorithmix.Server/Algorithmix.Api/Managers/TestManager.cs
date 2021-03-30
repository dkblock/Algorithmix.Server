using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Managers
{
    public class TestManager
    {
        private readonly AlgorithmService _algorithmService;
        private readonly TestService _testService;
        private readonly TestQuestionService _questionService;

        public TestManager(AlgorithmService algorithmService, TestService testService, TestQuestionService questionService)
        {
            _algorithmService = algorithmService;
            _testService = testService;
            _questionService = questionService;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            var createdTest = await _testService.CreateTest(testPayload);
            return await PrepareTest(createdTest);
        }

        public async Task<bool> Exists(int id)
        {
            return await _testService.Exists(id);
        }

        public async Task<Test> GetTest(int id)
        {
            var test = await _testService.GetTest(id);
            return await PrepareTest(test);
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var tests = await _testService.GetTests();
            return await PrepareTests(tests);
        }

        public async Task<IEnumerable<Test>> GetTests(string algorithmId)
        {
            var tests = await _testService.GetTests(algorithmId);
            return await PrepareTests(tests);
        }

        public async Task<IEnumerable<Test>> GetTests(IEnumerable<string> algorithmIds)
        {
            var tests = await _testService.GetTests(algorithmIds);
            return await PrepareTests(tests);
        }

        public async Task DeleteTest(int id)
        {
            await _testService.DeleteTest(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            var updatedTest = await _testService.UpdateTest(id, testPayload);
            return await PrepareTest(updatedTest);
        }

        private async Task<Test> PrepareTest(Test test)
        {
            test.Algorithm = await _algorithmService.GetAlgorithm(test.Algorithm.Id);
            test.Questions = await _questionService.GetTestQuestions(test.Id);

            return test;
        }

        private async Task<IEnumerable<Test>> PrepareTests(IEnumerable<Test> tests)
        {
            var preparedTests = new List<Test>();
            await tests.ForEachAsync(async test => preparedTests.Add(await PrepareTest(test)));
            return preparedTests.OrderByDescending(test => test.Id);
        }
    }
}
