using Algorithmix.Api.Core.Helpers;
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
        private readonly IUserContextManager _userContextManager;
        private readonly QueryHelper _queryHelper;

        public TestManager(
            AlgorithmService algorithmService,
            TestService testService,
            TestAlgorithmService testAlgorithmService,
            TestQuestionService questionService,
            ApplicationUserService userService,
            UserTestResultService userTestResultService,
            TestDataManager testDataManager,
            IUserContextManager userContextManager,
            QueryHelper queryHelper)
        {
            _algorithmService = algorithmService;
            _testService = testService;
            _testAlgorithmService = testAlgorithmService;
            _questionService = questionService;
            _userService = userService;
            _userTestResultService = userTestResultService;
            _testDataManager = testDataManager;
            _userContextManager = userContextManager;
            _queryHelper = queryHelper;
        }

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            testPayload.UserId = _userContextManager.CurrentUser.Id;

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

        public async Task<IEnumerable<Test>> GetTests(TestQuery query)
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
            testPayload.UserId = _userContextManager.CurrentUser.Id;

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
            var userTestResults = await _userTestResultService.GetUserTestResults(test.Id);
            var testAlgorithms = await _testAlgorithmService.GetTestAlgorithms(test.Id);

            test.CreatedBy = await _userService.GetUserById(test.CreatedBy.Id);
            test.Algorithms = await _algorithmService.GetAlgorithms(testAlgorithms.Select(ta => ta.AlgorithmId));
            test.Questions = await _questionService.GetTestQuestions(test.Id);
            test.PassesCount = userTestResults.Count();
            test.AverageResult = userTestResults.Any() ? (int)userTestResults.Average(utr => utr.Result) : 0;

            return test;
        }

        private async Task<IEnumerable<Test>> PrepareTests(IEnumerable<Test> tests, TestQuery query)
        {
            var preparedTests = new List<Test>();

            foreach (var test in tests)
            {
                var preparedTest = await PrepareTest(test);
                var filters = new[] { test.Name, test.CreatedBy.FirstName, test.CreatedBy.LastName }
                    .Union(test.Algorithms.Select(a => a.Name));

                if (!_queryHelper.IsMatch(query.SearchText, filters.ToArray()))
                    continue;

                preparedTests.Add(preparedTest);
            }

            return preparedTests.OrderByDescending(test => test.Id);
        }
    }
}
