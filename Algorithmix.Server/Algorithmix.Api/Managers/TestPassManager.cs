using Algorithmix.Common.Constants;
using Algorithmix.Entities;
using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Managers
{
    public class TestPassManager
    {
        private readonly TestManager _testManager;
        private readonly TestQuestionManager _questionManager;
        private readonly UserAnswerMapper _userAnswerMapper;
        private readonly UserAnswerService _userAnswerService;
        private readonly UserTestResultService _userTestResultService;

        public TestPassManager(
            TestManager testManager,
            TestQuestionManager questionManager,
            UserAnswerMapper userAnswerMapper,
            UserAnswerService userAnswerService,
            UserTestResultService userTestResultService)
        {
            _testManager = testManager;
            _questionManager = questionManager;
            _userAnswerMapper = userAnswerMapper;
            _userAnswerService = userAnswerService;
            _userTestResultService = userTestResultService;
        }

        public async Task<TestQuestion> GetNextTestQuestion(UserAnswerPayload userAnswerPayload, string userId, int testId)
        {
            if (userAnswerPayload == null)
            {
                var test = await _testManager.GetTest(testId);
                var firstQuestion = await _questionManager.GetTestQuestion(test.Questions.First().Id);

                return firstQuestion;
            }

            var currentQuestion = await _questionManager.GetTestQuestion(userAnswerPayload.QuestionId);
            await HandleUserAnswer(userAnswerPayload, currentQuestion, userId);

            if (currentQuestion.NextQuestionId != null)
                return await _questionManager.GetTestQuestion((int)currentQuestion.NextQuestionId);
            else
            {
                await ProcessUserTestResult(currentQuestion.Test.Id, userId);
                return null;
            }
        }

        private async Task HandleUserAnswer(UserAnswerPayload userAnswerPayload, TestQuestion currentQuestion, string userId)
        {
            var userAnswerEntity = _userAnswerMapper.ToEntity(userAnswerPayload, userId);

            switch (currentQuestion.Type)
            {
                case TestQuestionTypes.SingleAnswerQuestion:
                    HandleUserAnswerOnSingleAnswerQuestionType(userAnswerEntity, userAnswerPayload.Answers, currentQuestion);
                    break;
                case TestQuestionTypes.MultiAnswerQuestion:
                    HandleUserAnswerOnMultiAnswerQuestionType(userAnswerEntity, userAnswerPayload.Answers, currentQuestion);
                    break;
                case TestQuestionTypes.FreeAnswerQuestion:
                    HandleUserAnswerOnFreeAnswerQuestionType(userAnswerEntity, userAnswerPayload.Answers, currentQuestion);
                    break;
                default:
                    break;
            }

            await _userAnswerService.CreateUserAnswer(userAnswerEntity);
        }

        private void HandleUserAnswerOnSingleAnswerQuestionType(UserAnswerEntity userAnswerEntity, IEnumerable<string> userAnswers, TestQuestion currentQuestion)
        {
            var correctAnswer = currentQuestion.Answers.Single(a => a.IsCorrect);
            var userAnswerValue = userAnswers.Single();

            userAnswerEntity.Value = userAnswerValue;
            userAnswerEntity.IsCorrect = userAnswerValue == correctAnswer.Id.ToString();
        }

        private void HandleUserAnswerOnMultiAnswerQuestionType(UserAnswerEntity userAnswerEntity, IEnumerable<string> userAnswers, TestQuestion currentQuestion)
        {
            var correctAnswers = currentQuestion.Answers.Where(a => a.IsCorrect).Select(a => a.Id.ToString());
            var correctUserAnswers = userAnswers.Where(ua => correctAnswers.Contains(ua));

            userAnswerEntity.Value = string.Join("__", userAnswers);
            userAnswerEntity.IsCorrect = correctUserAnswers.Count() == correctAnswers.Count();
        }

        private void HandleUserAnswerOnFreeAnswerQuestionType(UserAnswerEntity userAnswerEntity, IEnumerable<string> userAnswers, TestQuestion currentQuestion)
        {
            var correctAnswer = currentQuestion.Answers.Single(a => a.IsCorrect);
            var userAnswerValue = userAnswers.Single();

            userAnswerEntity.Value = userAnswerValue;
            userAnswerEntity.IsCorrect = userAnswerValue.ToLower() == correctAnswer.Value.ToLower();
        }

        private async Task<UserTestResult> ProcessUserTestResult(int testId, string userId)
        {
            var test = await _testManager.GetTest(testId);
            var questionIds = test.Questions.Select(q => q.Id);
            var userAnswers = await _userAnswerService.GetUserAnswers(questionIds, userId);
            var correctUserAnswersCount = userAnswers.Count(ua => ua.IsCorrect);
            var result = (double)correctUserAnswersCount / test.Questions.Count() * 100;

            var userTestResult = new UserTestResultEntity
            {
                TestId = testId,
                UserId = userId,
                CorrectAnswers = correctUserAnswersCount,
                TotalQuestions = test.Questions.Count(),
                Result = (int)result,
                IsPassed = true,
                PassingTime = DateTime.Now,
            };

            var createdUserTestResult = await _userTestResultService.CreateUserTestResult(userTestResult);
            createdUserTestResult.UserAnswers = userAnswers;

            return createdUserTestResult;
        }

        public async Task<TestQuestion> GetPreviousTestQuestion(int currentQuestionId, string userId)
        {
            var currentQuestion = await _questionManager.GetTestQuestion(currentQuestionId);

            if (currentQuestion.PreviousQuestionId == null)
                return null;

            var previousQuestion = await _questionManager.GetTestQuestion((int)currentQuestion.PreviousQuestionId);
            await _userAnswerService.DeleteUserAnswer(previousQuestion.Id, userId);

            return previousQuestion;
        }
    }
}
