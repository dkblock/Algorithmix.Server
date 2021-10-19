using Algorithmix.Api.Core.TestPass;
using Algorithmix.Common.Extensions;
using Algorithmix.Common.Helpers;
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
        private readonly ApplicationUserManager _userManager;
        private readonly PublishedTestManager _testManager;
        private readonly UserAnswerManager _userAnswerManager;
        private readonly UserTestResultService _userTestResultService;
        private readonly IUserContextManager _userContextManager;
        private readonly QueryHelper _queryHelper;

        public UserTestResultManager(
            ApplicationUserManager userManager,
            PublishedTestManager testManager,
            UserAnswerManager userAnswerManager,
            UserTestResultService userTestResultService,
            IUserContextManager userContextManager)
        {
            _userManager = userManager;
            _testManager = testManager;
            _userAnswerManager = userAnswerManager;
            _userTestResultService = userTestResultService;
            _userContextManager = userContextManager;
            _queryHelper = new QueryHelper();
        }

        public async Task<UserTestResult> CreateUserTestResult(int testId, string userId)
        {
            var test = await _testManager.GetTest(testId);
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

        public async Task<UserTestResult> GetUserTestResult(int testId)
        {
            var userId = _userContextManager.CurrentUser.Id;
            var userTestResult = await _userTestResultService.GetUserTestResult(testId, userId);

            return await PrepareUserTestResult(userTestResult);
        }

        public async Task<UserTestResult> GetUserTestResult(int testId, string userId)
        {
            var userTestResult = await _userTestResultService.GetUserTestResult(testId, userId);
            return await PrepareUserTestResult(userTestResult);
        }

        public async Task<IEnumerable<UserTestResult>> GetUserTestResults(UserTestResultQuery query)
        {
            var userTestResults = await _userTestResultService.GetUserTestResults();
            return await PrepareUserTestResults(userTestResults, query);
        }

        public async Task<bool> Exists(int testId)
        {
            var userId = _userContextManager.CurrentUser.Id;
            return await _userTestResultService.Exists(testId, userId);
        }

        public async Task<bool> Exists(int testId, string userId)
        {
            return await _userTestResultService.Exists(testId, userId);
        }

        public async Task DeleteUserTestResult(int testId, string userId)
        {
            await _userTestResultService.DeleteUserTestResult(testId, userId);
        }

        private async Task<UserTestResult> PrepareUserTestResult(UserTestResult userTestResult)
        {
            userTestResult.Test = await _testManager.GetTest(userTestResult.Test.Id);
            userTestResult.User = await _userManager.GetUserById(userTestResult.User.Id);

            var questionIds = userTestResult.Test.Questions.Select(q => q.Id).ToList();
            userTestResult.UserAnswers = await _userAnswerManager.GetUserAnswers(questionIds, userTestResult.User.Id);
            userTestResult.UserAnswers = userTestResult.UserAnswers.OrderBy(ua => questionIds.IndexOf(ua.Question.Id));

            return userTestResult;
        }

        private async Task<IEnumerable<UserTestResult>> PrepareUserTestResults(IEnumerable<UserTestResult> userTestResults, UserTestResultQuery query)
        {
            var preparedUserTestResults = new List<UserTestResult>();

            foreach (var userTestResult in userTestResults)
            {
                var preparedUserTestResult = await PrepareUserTestResult(userTestResult);
                var filters = new[]
                {
                    preparedUserTestResult.User.FirstName,
                    preparedUserTestResult.User.LastName,
                    $"{preparedUserTestResult.User.FirstName} {preparedUserTestResult.User.LastName}",
                    $"{preparedUserTestResult.User.LastName} {preparedUserTestResult.User.FirstName}",
                    preparedUserTestResult.User.Group.Name,
                    preparedUserTestResult.Test.Name,
                };

                if (!_queryHelper.IsMatch(query.SearchText, filters))
                    continue;

                if (query.GroupId != -1 && query.GroupId != preparedUserTestResult.User.Group.Id)
                    continue;

                preparedUserTestResults.Add(preparedUserTestResult);
            }

            return query.Desc
                ? preparedUserTestResults.OrderByDescending(utr => utr.PassingTime)
                : preparedUserTestResults.OrderBy(utr => utr.PassingTime);
        }
    }
}
