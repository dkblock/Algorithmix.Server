using Algorithmix.Api.Core.TestPass;
using Algorithmix.Common.Constants;
using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class UserAnswerManager
    {
        private readonly PublishedTestQuestionManager _questionManager;
        private readonly UserAnswerService _userAnswerService;

        public UserAnswerManager(PublishedTestQuestionManager questionManager, UserAnswerService userAnswerService)
        {
            _questionManager = questionManager;
            _userAnswerService = userAnswerService;
        }

        public async Task<UserAnswer> CreateUserAnswer(UserAnswerData userAnswerData, TestQuestion currentQuestion)
        {
            switch (currentQuestion.Type)
            {
                case TestQuestionTypes.SingleAnswerQuestion:
                    HandleUserAnswerOnSingleAnswerQuestionType(userAnswerData, currentQuestion);
                    break;
                case TestQuestionTypes.MultiAnswerQuestion:
                    HandleUserAnswerOnMultiAnswerQuestionType(userAnswerData, currentQuestion);
                    break;
                case TestQuestionTypes.FreeAnswerQuestion:
                    HandleUserAnswerOnFreeAnswerQuestionType(userAnswerData, currentQuestion);
                    break;
                default:
                    break;
            }

            var createdUserAnswer = await _userAnswerService.CreateUserAnswer(userAnswerData);
            return await PrepareUserAnswer(createdUserAnswer);
        }

        private void HandleUserAnswerOnSingleAnswerQuestionType(UserAnswerData userAnswerData, TestQuestion currentQuestion)
        {
            var correctAnswer = currentQuestion.Answers.Single(a => a.IsCorrect);
            var userAnswerValue = userAnswerData.Answers.Single();

            userAnswerData.Value = userAnswerValue;
            userAnswerData.IsCorrect = userAnswerValue == correctAnswer.Id.ToString();
        }

        private void HandleUserAnswerOnMultiAnswerQuestionType(UserAnswerData userAnswerData, TestQuestion currentQuestion)
        {
            var userAnswers = userAnswerData.Answers;
            var correctAnswers = currentQuestion.Answers.Where(a => a.IsCorrect).Select(a => a.Id.ToString());

            userAnswerData.Value = string.Join("__", userAnswers);
            userAnswerData.IsCorrect = userAnswers.All(ua => correctAnswers.Contains(ua)) && correctAnswers.Count() == userAnswers.Count();
        }

        private void HandleUserAnswerOnFreeAnswerQuestionType(UserAnswerData userAnswerData, TestQuestion currentQuestion)
        {
            var correctAnswer = currentQuestion.Answers.Single(a => a.IsCorrect);
            var userAnswerValue = userAnswerData.Answers.Single();

            userAnswerData.Value = userAnswerValue;
            userAnswerData.IsCorrect = userAnswerValue.ToLower() == correctAnswer.Value.ToLower();
        }

        public async Task<IEnumerable<UserAnswer>> GetUserAnswers(IEnumerable<int> questionIds, string userId)
        {
            var userAnswers = await _userAnswerService.GetUserAnswers(questionIds, userId);
            return await PrepareUserAnswers(userAnswers);
        }

        public async Task<IEnumerable<UserAnswer>> GetUserAnswers(int questionId)
        {
            var userAnswers = await _userAnswerService.GetUserAnswers(questionId);
            return await PrepareUserAnswers(userAnswers);
        }

        public async Task DeleteUserAnswers(IEnumerable<int> questionIds, string userId)
        {
            foreach (var questionId in questionIds)
            {
                if (await _userAnswerService.Exists(questionId, userId))
                    await _userAnswerService.DeleteUserAnswer(questionId, userId);
            }
        }

        private async Task<UserAnswer> PrepareUserAnswer(UserAnswer userAnswer)
        {
            userAnswer.Question = await _questionManager.GetTestQuestion(userAnswer.Question.Id);
            return userAnswer;
        }

        private async Task<IEnumerable<UserAnswer>> PrepareUserAnswers(IEnumerable<UserAnswer> userAnswers)
        {
            var preparedUserAnswers = new List<UserAnswer>();
            await userAnswers.ForEachAsync(async userAnswer => preparedUserAnswers.Add(await PrepareUserAnswer(userAnswer)));

            return preparedUserAnswers;
        }
    }
}
