using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Extensions;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Api.Managers
{
    public class TestAnswersManager
    {
        private readonly TestAnswersService _answersService;
        private readonly TestQuestionsService _questionsService;

        public TestAnswersManager(TestAnswersService answersService, TestQuestionsService questionsService)
        {
            _answersService = answersService;
            _questionsService = questionsService;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswerPayload answerPayload)
        {
            var createdAnswer = await _answersService.CreateTestAnswer(answerPayload);
            return await PrepareAnswer(createdAnswer);
        }

        public async Task<bool> Exists(int answerId, int questionId)
        {
            return await _answersService.Exists(answerId, questionId);
        }

        public async Task<TestAnswer> GetTestAnswer(int id)
        {
            var answer = await _answersService.GetTestAnswer(id);
            return await PrepareAnswer(answer);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(int questionId)
        {
            var answers = await _answersService.GetTestAnswers(questionId);
            return await PrepareAnswers(answers);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(IEnumerable<int> questionIds)
        {
            var answers = await _answersService.GetTestAnswers(questionIds);
            return await PrepareAnswers(answers);
        }

        public async Task DeleteTestAnswer(int id)
        {
            await _answersService.DeleteTestAnswer(id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(int id, TestAnswerPayload answerPayload)
        {
            var updatedAnswer = await _answersService.UpdateTestAnswer(id, answerPayload);
            return await PrepareAnswer(updatedAnswer);
        }

        private async Task<TestAnswer> PrepareAnswer(TestAnswer answer)
        {
            answer.Question = await _questionsService.GetTestQuestion(answer.Question.Id);
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
