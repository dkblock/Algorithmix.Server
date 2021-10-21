using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services.TestPass;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core.TestPass
{
    public class PublishedTestAnswerManager
    {
        private readonly PublishedTestAnswerService _answerService;
        private readonly PublishedTestQuestionService _questionService;

        public PublishedTestAnswerManager(PublishedTestAnswerService answerService, PublishedTestQuestionService questionService)
        {
            _answerService = answerService;
            _questionService = questionService;
        }

        public async Task CreateTestAnswer(TestAnswer answer)
        {
            await _answerService.CreateTestAnswer(answer);
        }

        public async Task<bool> Exists(int answerId, int questionId)
        {
            return await _answerService.Exists(answerId, questionId);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(int questionId)
        {
            var answers = await _answerService.GetTestAnswers(questionId);
            return await PrepareAnswers(answers);
        }

        public async Task DeleteTestAnswer(int id)
        {
            await _answerService.DeleteTestAnswer(id);
        }

        public async Task UpdateTestAnswer(TestAnswer answer)
        {
            await _answerService.UpdateTestAnswer(answer);
        }

        private async Task<TestAnswer> PrepareAnswer(TestAnswer answer)
        {
            answer.Question = await _questionService.GetTestQuestion(answer.Question.Id);
            return answer;
        }

        private async Task<IEnumerable<TestAnswer>> PrepareAnswers(IEnumerable<TestAnswer> answers)
        {
            var preparedAnswers = new List<TestAnswer>();
            await answers.ForEachAsync(async answer => preparedAnswers.Add(await PrepareAnswer(answer)));
            return preparedAnswers;
        }
    }
}
