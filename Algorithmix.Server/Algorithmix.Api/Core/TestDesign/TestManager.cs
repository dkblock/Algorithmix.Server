using Algorithmix.Api.Core.Helpers;
using Algorithmix.Common.Constants;
using Algorithmix.Identity.Core;
using Algorithmix.Models;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using Algorithmix.Services.TestDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core.TestDesign
{
    public class TestManager
    {
        private readonly AlgorithmService _algorithmService;
        private readonly TestService _testService;
        private readonly TestAlgorithmService _testAlgorithmService;
        private readonly TestQuestionService _questionService;
        private readonly ApplicationUserService _userService;
        private readonly UserTestResultService _userTestResultService;
        private readonly TestDataManager _testDataManager;
        private readonly IUserContextHandler _userContextHandler;
        private readonly QueryHelper _queryHelper;

        public TestManager(
            AlgorithmService algorithmService,
            TestService testService,
            TestAlgorithmService testAlgorithmService,
            TestQuestionService questionService,
            ApplicationUserService userService,
            UserTestResultService userTestResultService,
            TestDataManager testDataManager,
            IUserContextHandler userContextHandler,
            QueryHelper queryHelper)
        {
            _algorithmService = algorithmService;
            _testService = testService;
            _testAlgorithmService = testAlgorithmService;
            _questionService = questionService;
            _userService = userService;
            _userTestResultService = userTestResultService;
            _testDataManager = testDataManager;
            _userContextHandler = userContextHandler;
            _queryHelper = queryHelper;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            testPayload.UserId = _userContextHandler.CurrentUser.Id;

            var createdTest = await _testService.CreateTest(testPayload);
            await _testAlgorithmService.CreateTestAlgorithms(createdTest.Id, testPayload.AlgorithmIds);
            _testDataManager.CreateTestQuestionImagesDirectory(createdTest.Id);

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

        public async Task<PageResponse<Test>> GetTests(TestQuery query)
        {
            var tests = await _testService.GetAllTests();
            return await PrepareTests(tests, query);
        }

        public async Task DeleteTest(int id)
        {
            await _testAlgorithmService.DeleteTestAlgorithms(id);
            await _testService.DeleteTest(id);
            _testDataManager.DeleteTestQuestionImagesDirectory(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            testPayload.UserId = _userContextHandler.CurrentUser.Id;

            var updatedTest = await _testService.UpdateTest(id, testPayload);
            await _testAlgorithmService.UpdateTestAlgorithms(id, testPayload.AlgorithmIds);

            return await PrepareTest(updatedTest);
        }

        public async Task PublishTest(int id)
        {
            await _testService.UpdateTest(id, true);
        }

        private async Task<Test> PrepareTest(Test test)
        {
            var currentUser = _userContextHandler.CurrentUser;

            var userTestResults = await _userTestResultService.GetUserTestResults(test.Id);
            var testAlgorithms = await _testAlgorithmService.GetTestAlgorithms(test.Id);

            test.CreatedBy = await _userService.GetUserById(test.CreatedBy.Id);
            test.Algorithms = await _algorithmService.GetAlgorithms(testAlgorithms.Select(ta => ta.AlgorithmId));
            test.Questions = await _questionService.GetTestQuestions(test.Id);
            test.PassesCount = userTestResults.Count();
            test.AverageResult = userTestResults.Any() ? (int)userTestResults.Average(utr => utr.Result) : 0;
            test.UserHasAccess = currentUser != null
                && (test.CreatedBy.Id == currentUser.Id || currentUser.Role == Roles.Administrator);

            return test;
        }

        private async Task<PageResponse<Test>> PrepareTests(IEnumerable<Test> tests, TestQuery query)
        {
            var preparedTests = new List<Test>();

            foreach (var test in tests)
            {
                var preparedTest = await PrepareTest(test);

                var filters = new List<string>
                {
                    preparedTest.Name,
                    preparedTest.CreatedBy.FirstName,
                    preparedTest.CreatedBy.LastName,
                    $"{preparedTest.CreatedBy.FirstName} {preparedTest.CreatedBy.LastName}",
                    $"{preparedTest.CreatedBy.LastName} {preparedTest.CreatedBy.FirstName}",
                };

                if (!_queryHelper.IsMatch(query.SearchText, filters.Union(test.Algorithms.Select(a => a.Name)).ToArray()))
                    continue;

                if (query.OnlyAccessible && !preparedTest.UserHasAccess)
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
