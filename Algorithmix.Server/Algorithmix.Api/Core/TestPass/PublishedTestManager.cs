using Algorithmix.Api.Core.Helpers;
using Algorithmix.Identity.Core;
using Algorithmix.Models;
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
        private readonly TestDataManager _testDataManager;
        private readonly TestAlgorithmService _testAlgorithmService;
        private readonly IUserContextHandler _userContextHandler;
        private readonly QueryHelper _queryHelper;

        public PublishedTestManager(
            AlgorithmService algorithmService,
            PublishedTestService testService,
            TestAlgorithmService testAlgorithmService,
            PublishedTestQuestionService questionService,
            ApplicationUserService userService,
            UserTestResultService userTestResultService,
            IUserContextHandler userContextHandler,
            TestDataManager testDataManager,
            QueryHelper queryHelper)
        {
            _algorithmService = algorithmService;
            _testService = testService;
            _questionService = questionService;
            _userService = userService;
            _userTestResultService = userTestResultService;
            _testDataManager = testDataManager;
            _testAlgorithmService = testAlgorithmService;
            _userContextHandler = userContextHandler;
            _queryHelper = queryHelper;
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

        public async Task<Test> GetTest(int id)
        {
            var test = await _testService.GetTest(id);
            return await PrepareTest(test);
        }

        public async Task<PageResponse<Test>> GetTests(TestQuery query)
        {
            var tests = await _testService.GetAllTests();
            return await PrepareTests(tests, query);
        }

        public async Task DeleteTest(int id)
        {
            await _testService.DeleteTest(id);
            _testDataManager.DeleteTestQuestionImagesDirectory(id, true);
        }

        public async Task UpdateTest(Test test)
        {
            await _testService.UpdateTest(test);
        }

        private async Task<Test> PrepareTest(Test test)
        {
            var testAlgorithms = await _testAlgorithmService.GetTestAlgorithms(test.Id);

            test.CreatedBy = await _userService.GetUserById(test.CreatedBy.Id);
            test.Algorithms = await _algorithmService.GetAlgorithms(testAlgorithms.Select(ta => ta.AlgorithmId));
            test.Questions = await _questionService.GetTestQuestions(test.Id);
            test.AverageResult = await _userTestResultService.GetAverageUserTestResult(test.Id);

            if (_userContextHandler.CurrentUser != null && await _userTestResultService.Exists(test.Id, _userContextHandler.CurrentUser.Id))
                test.UserResult = await _userTestResultService.GetUserTestResult(test.Id, _userContextHandler.CurrentUser.Id);

            return test;
        }

        private async Task<PageResponse<Test>> PrepareTests(IEnumerable<Test> tests, TestQuery query)
        {
            var preparedTests = new List<Test>();

            foreach (var test in tests)
            {
                var preparedTest = await PrepareTest(test);

                var filters = new[] { test.Name }.Union(test.Algorithms.Select(a => a.Name));

                if (!_queryHelper.IsMatch(query.SearchText, filters.ToArray()))
                    continue;

                preparedTests.Add(preparedTest);
            }

            var sortedTests = query.SortByDesc
                ? preparedTests.OrderByDescending(_queryHelper.TestSortModel[query.SortBy])
                : preparedTests.OrderBy(_queryHelper.TestSortModel[query.SortBy]);

            var result = sortedTests.Skip(query.PageSize * (query.PageIndex - 1));

            return new PageResponse<Test>
            {
                Page = result.Take(query.PageSize),
                TotalCount = sortedTests.Count()
            };
        }
    }
}
