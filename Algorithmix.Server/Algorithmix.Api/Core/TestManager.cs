using Algorithmix.Common.Extensions;
using Algorithmix.Common.Helpers;
using Algorithmix.Models.SearchFilters;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class TestManager
    {
        private readonly AlgorithmService _algorithmService;
        private readonly TestService _testService;
        private readonly TestQuestionService _questionService;
        private readonly UserTestResultService _userTestResultService;
        private readonly FilterHelper _filterHelper;
        private readonly TestDataManager _testDataManager;

        public TestManager(
            AlgorithmService algorithmService,
            TestService testService,
            TestQuestionService questionService,
            UserTestResultService userTestResultService,
            TestDataManager testDataManager)
        {
            _algorithmService = algorithmService;
            _testService = testService;
            _questionService = questionService;
            _userTestResultService = userTestResultService;
            _filterHelper = new FilterHelper();
            _testDataManager = testDataManager;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            var createdTest = await _testService.CreateTest(testPayload);
            _testDataManager.CreateTestQuestionImagesDirectory(createdTest.Id);

            return await PrepareTest(createdTest);
        }

        public async Task<bool> Exists(int id)
        {
            return await _testService.Exists(id);
        }

        public async Task<Test> GetTest(int id, TestFilterPayload filter = null)
        {
            var test = await _testService.GetTest(id);
            return await PrepareTest(test, filter);
        }

        public async Task<IEnumerable<Test>> GetTests(TestFilterPayload filter = null)
        {
            var tests = await _testService.GetTests();
            return await PrepareTests(tests, filter);
        }

        public async Task DeleteTest(int id)
        {
            await _testService.DeleteTest(id);
            _testDataManager.DeleteTestQuestionImagesDirectory(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            var updatedTest = await _testService.UpdateTest(id, testPayload);
            return await PrepareTest(updatedTest);
        }

        private async Task<Test> PrepareTest(Test test, TestFilterPayload filter = null)
        {
            test.Algorithm = await _algorithmService.GetAlgorithm(test.Algorithm.Id);
            test.Questions = await _questionService.GetTestQuestions(test.Id);
            test.AverageResult = await _userTestResultService.GetAverageUserTestResult(test.Id);

            if (filter != null && await _userTestResultService.Exists(test.Id, filter.UserId))
                test.UserResult = await _userTestResultService.GetUserTestResult(test.Id, filter.UserId);

            return test;
        }

        private async Task<IEnumerable<Test>> PrepareTests(IEnumerable<Test> tests, TestFilterPayload filter = null)
        {
            var preparedTests = new List<Test>();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchText))
                    tests = tests.Where(t =>
                        _filterHelper.IsMatch(filter.SearchText, t.Name) || _filterHelper.IsMatch(filter.SearchText, t.Algorithm.Name));

                if (filter.OnlyPassed)
                    tests = await tests.WhereAsync(async t => await _userTestResultService.Exists(t.Id, filter.UserId));

                if (filter.AlgorithmIds.Any())
                    tests = tests.Where(t => filter.AlgorithmIds.Contains(t.Algorithm.Id));
            }

            await tests.ForEachAsync(async test => preparedTests.Add(await PrepareTest(test, filter)));

            return preparedTests.OrderByDescending(test => test.Id);
        }
    }
}
