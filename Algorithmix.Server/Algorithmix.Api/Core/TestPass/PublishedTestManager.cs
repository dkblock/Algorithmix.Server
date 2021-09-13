using Algorithmix.Common.Extensions;
using Algorithmix.Common.Helpers;
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
        private readonly QueryHelper _queryHelper;

        public PublishedTestManager(
            AlgorithmService algorithmService,
            PublishedTestService testService,
            TestAlgorithmService testAlgorithmService,
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
            _testDataManager = testDataManager;
            _testAlgorithmService = testAlgorithmService;
            _queryHelper = new QueryHelper();
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

        public async Task<IEnumerable<Test>> GetTests(TestQuery query)
        {
            var tests = await _testService.GetAllTests();
            return await PrepareTests(tests, query);
        }

        public async Task DeleteTest(int id)
        {
            await _testService.DeleteTest(id);
            _testDataManager.DeleteTestQuestionImagesDirectory(id, true);
        }

        private async Task<Test> PrepareTest(Test test)
        {
            var testAlgorithms = await _testAlgorithmService.GetTestAlgorithms(test.Id);

            test.CreatedBy = await _userService.GetUserById(test.CreatedBy.Id);
            test.Algorithms = await _algorithmService.GetAlgorithms(testAlgorithms.Select(ta => ta.AlgorithmId));
            test.Questions = await _questionService.GetTestQuestions(test.Id);
            test.AverageResult = await _userTestResultService.GetAverageUserTestResult(test.Id);

            //if (filter != null && await _userTestResultService.Exists(test.Id, filter.UserId))
            //    test.UserResult = await _userTestResultService.GetUserTestResult(test.Id, filter.UserId);

            return test;
        }

        private async Task<IEnumerable<Test>> PrepareTests(IEnumerable<Test> tests, TestQuery query)
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

            return preparedTests.OrderByDescending(test => test.Id);
        }
    }
}
