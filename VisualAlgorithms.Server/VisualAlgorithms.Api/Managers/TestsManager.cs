using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Extensions;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Api.Managers
{
    public class TestsManager
    {
        private readonly AlgorithmsService _algorithmsService;
        private readonly TestsService _testsService;
        private readonly TestQuestionsService _questionsService;

        public TestsManager(AlgorithmsService algorithmsService, TestsService testsService, TestQuestionsService questionsService)
        {
            _algorithmsService = algorithmsService;
            _testsService = testsService;
            _questionsService = questionsService;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            var createdTest = await _testsService.CreateTest(testPayload);
            return await PrepareTest(createdTest);
        }

        public async Task<bool> Exists(int id)
        {
            return await _testsService.Exists(id);
        }

        public async Task<Test> GetTest(int id)
        {
            var test = await _testsService.GetTest(id);
            return await PrepareTest(test);
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var tests = await _testsService.GetTests();
            return await PrepareTests(tests);
        }

        public async Task<IEnumerable<Test>> GetTests(string algorithmId)
        {
            var tests = await _testsService.GetTests(algorithmId);
            return await PrepareTests(tests);
        }

        public async Task<IEnumerable<Test>> GetTests(IEnumerable<string> algorithmIds)
        {
            var tests = await _testsService.GetTests(algorithmIds);
            return await PrepareTests(tests);
        }

        public async Task DeleteTest(int id)
        {
            await _testsService.DeleteTest(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            var updatedTest = await _testsService.UpdateTest(id, testPayload);
            return await PrepareTest(updatedTest);
        }

        private async Task<Test> PrepareTest(Test test)
        {
            test.Algorithm = await _algorithmsService.GetAlgorithm(test.Algorithm.Id);
            test.Questions = await _questionsService.GetTestQuestions(test.Id);

            return test;
        }

        private async Task<IEnumerable<Test>> PrepareTests(IEnumerable<Test> tests)
        {
            var preparedTests = new List<Test>();
            await tests.ForEachAsync(async test => preparedTests.Add(await PrepareTest(test)));
            return preparedTests;
        }
    }
}
