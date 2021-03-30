using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Managers
{
    public class TestAnswerManager
    {
        private readonly TestAnswerService _answerService;
        private readonly TestQuestionService _questionService;

        public TestAnswerManager(TestAnswerService answerService, TestQuestionService questionService)
        {
            _answerService = answerService;
            _questionService = questionService;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswerPayload answerPayload)
        {
            var createdAnswer = await _answerService.CreateTestAnswer(answerPayload);
            return await PrepareAnswer(createdAnswer);
        }

        public async Task<bool> Exists(int answerId, int questionId)
        {
            return await _answerService.Exists(answerId, questionId);
        }

        public async Task<TestAnswer> GetTestAnswer(int id)
        {
            var answer = await _answerService.GetTestAnswer(id);
            return await PrepareAnswer(answer);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(int questionId)
        {
            var answers = await _answerService.GetTestAnswers(questionId);
            return await PrepareAnswers(answers);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(IEnumerable<int> questionIds)
        {
            var answers = await _answerService.GetTestAnswers(questionIds);
            return await PrepareAnswers(answers);
        }

        public async Task DeleteTestAnswer(int id)
        {
            await _answerService.DeleteTestAnswer(id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(int id, TestAnswerPayload answerPayload)
        {
            var updatedAnswer = await _answerService.UpdateTestAnswer(id, answerPayload);
            return await PrepareAnswer(updatedAnswer);
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
