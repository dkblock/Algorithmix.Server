using Algorithmix.Api.Core.TestPass;
using Algorithmix.Identity.Core;
using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class TestPassManager
    {
        private readonly PublishedTestManager _testManager;
        private readonly PublishedTestQuestionManager _questionManager;
        private readonly UserAnswerMapper _userAnswerMapper;
        private readonly UserAnswerManager _userAnswerManager;
        private readonly UserTestResultManager _userTestResultManager;
        private readonly IUserContextHandler _userContextHandler;

        public TestPassManager(
            PublishedTestManager testManager,
            PublishedTestQuestionManager questionManager,
            UserAnswerMapper userAnswerMapper,
            UserAnswerManager userAnswerManager,
            UserTestResultManager userTestResultManager,
            IUserContextHandler userContextHandler)
        {
            _testManager = testManager;
            _questionManager = questionManager;
            _userAnswerMapper = userAnswerMapper;
            _userAnswerManager = userAnswerManager;
            _userTestResultManager = userTestResultManager;
            _userContextHandler = userContextHandler;
        }

        public async Task<TestQuestion> GetNextTestQuestion(UserAnswerPayload userAnswerPayload, int testId)
        {
            var userId = _userContextHandler.CurrentUser.Id;

            if (userAnswerPayload == null)
                return await HandleTestStart(testId, userId);

            var currentQuestion = await _questionManager.GetTestQuestion(userAnswerPayload.QuestionId);
            var userAnswerData = _userAnswerMapper.ToData(userAnswerPayload, userId);
            await _userAnswerManager.CreateUserAnswer(userAnswerData, currentQuestion);

            if (currentQuestion.NextQuestionId != null)
                return await _questionManager.GetTestQuestion((int)currentQuestion.NextQuestionId);
            else
            {
                await _userTestResultManager.CreateUserTestResult(testId, userId);
                return null;
            }
        }

        private async Task<TestQuestion> HandleTestStart(int testId, string userId)
        {
            var test = await _testManager.GetTest(testId);
            var firstQuestion = await _questionManager.GetTestQuestion(test.Questions.First().Id);
            var questionIds = test.Questions.Select(q => q.Id);

            if (!await _userTestResultManager.Exists(testId, userId))
                await _userAnswerManager.DeleteUserAnswers(questionIds, userId);

            return firstQuestion;
        }

        public async Task<TestQuestion> GetPreviousTestQuestion(int currentQuestionId)
        {
            var userId = _userContextHandler.CurrentUser.Id;
            var currentQuestion = await _questionManager.GetTestQuestion(currentQuestionId);

            if (currentQuestion.PreviousQuestionId == null)
                return null;

            var previousQuestion = await _questionManager.GetTestQuestion((int)currentQuestion.PreviousQuestionId);
            await _userAnswerManager.DeleteUserAnswers(new List<int> { previousQuestion.Id }, userId);

            return previousQuestion;
        }
    }
}
