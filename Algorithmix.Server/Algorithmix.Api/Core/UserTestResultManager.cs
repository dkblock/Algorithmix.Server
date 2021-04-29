using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class UserTestResultManager
    {
        private readonly TestManager _testManager;
        private readonly UserAnswerManager _userAnswerManager;
        private readonly UserTestResultService _userTestResultService;
        private readonly UserService _userService;

        public UserTestResultManager(
            TestManager testManager,
            UserAnswerManager userAnswerManager,
            UserTestResultService userTestResultService,
            UserService userService)
        {
            _testManager = testManager;
            _userAnswerManager = userAnswerManager;
            _userTestResultService = userTestResultService;
            _userService = userService;
        }

        public async Task<UserTestResult> CreateUserTestResult(int testId, string userId)
        {
            var test = await _testManager.GetTest(testId, null);
            var questionIds = test.Questions.Select(q => q.Id);
            var userAnswers = await _userAnswerManager.GetUserAnswers(questionIds, userId);
            var correctUserAnswersCount = userAnswers.Count(ua => ua.IsCorrect);
            var result = (double)correctUserAnswersCount / test.Questions.Count() * 100;

            var userTestResultData = new UserTestResultData
            {
                TestId = testId,
                UserId = userId,
                CorrectAnswers = correctUserAnswersCount,
                Result = (int)result,
                IsPassed = true,
                PassingTime = DateTime.Now,
            };

            var createdUserTestResult = await _userTestResultService.CreateUserTestResult(userTestResultData);
            return await PrepareUserTestResult(createdUserTestResult);
        }

        public async Task<UserTestResult> GetUserTestResult(int testId, string userId)
        {
            var userTestResult = await _userTestResultService.GetUserTestResult(testId, userId);
            return await PrepareUserTestResult(userTestResult);
        }

        public async Task<bool> Exists(int testId, string userId)
        {
            return await _userTestResultService.Exists(testId, userId);
        }

        private async Task<UserTestResult> PrepareUserTestResult(UserTestResult userTestResult)
        {
            userTestResult.Test = await _testManager.GetTest(userTestResult.Test.Id, null);
            userTestResult.User = await _userService.GetUserById(userTestResult.User.Id);

            var questionIds = userTestResult.Test.Questions.Select(q => q.Id);
            userTestResult.UserAnswers = await _userAnswerManager.GetUserAnswers(questionIds, userTestResult.User.Id);

            return userTestResult;
        }

        private async Task<IEnumerable<UserTestResult>> PrepareUserTestResults(IEnumerable<UserTestResult> userTestResults)
        {
            var preparedUserTestResults = new List<UserTestResult>();
            await userTestResults.ForEachAsync(async userTestResult => preparedUserTestResults.Add(await PrepareUserTestResult(userTestResult)));

            return preparedUserTestResults;
        }
    }
}
