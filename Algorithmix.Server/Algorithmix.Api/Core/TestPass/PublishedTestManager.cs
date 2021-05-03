using Algorithmix.Common.Extensions;
using Algorithmix.Common.Helpers;
using Algorithmix.Models.SearchFilters;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using Algorithmix.Services.TestPass;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core.TestPass
{
    public class PublishedTestManager
    {
        private readonly AlgorithmService _algorithmService;
        private readonly PublishedTestService _testService;
        private readonly PublishedTestQuestionService _questionService;
        private readonly ApplicationUserService _userService;
        private readonly UserTestResultService _userTestResultService;
        private readonly FilterHelper _filterHelper;
        private readonly TestDataManager _testDataManager;

        public PublishedTestManager(
            AlgorithmService algorithmService,
            PublishedTestService testService,
            PublishedTestQuestionService questionService,
            ApplicationUserService userService,
            UserTestResultService userTestResultService,
            TestDataManager testDataManager)
        {
            _algorithmService = algorithmService;
            _testService = testService;
            _questionService = questionService;
            _userService = userService;
            _userTestResultService = userTestResultService;
            _filterHelper = new FilterHelper();
            _testDataManager = testDataManager;
        }

        public async Task CreateTest(Test test)
        {
            var createdTest = await _testService.CreateTest(test);
            _testDataManager.CreateTestQuestionImagesDirectory(createdTest.Id, true);
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
            _testDataManager.DeleteTestQuestionImagesDirectory(id, true);
        }

        private async Task<Test> PrepareTest(Test test, TestFilterPayload filter = null)
        {
            test.CreatedBy = await _userService.GetUserById(test.CreatedBy.Id);
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
