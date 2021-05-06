using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services.TestDesign;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core.TestDesign
{
    public class TestAnswerManager
    {
        private readonly TestAnswerService _answerService;
        private readonly TestQuestionService _questionService;
        private readonly TestService _testService;

        public TestAnswerManager(TestAnswerService answerService, TestQuestionService questionService, TestService testService)
        {
            _answerService = answerService;
            _questionService = questionService;
            _testService = testService;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswerPayload answerPayload)
        {
            var question = await _questionService.GetTestQuestion(answerPayload.QuestionId);
            var createdAnswer = await _answerService.CreateTestAnswer(answerPayload, question.Type);
            await _testService.UpdateTest(question.Test.Id);

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
            var answer = await _answerService.GetTestAnswer(id);
            var question = await _questionService.GetTestQuestion(answer.Question.Id);

            await _answerService.DeleteTestAnswer(id, question.Type);
            await _testService.UpdateTest(question.Test.Id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(int id, TestAnswerPayload answerPayload)
        {
            var question = await _questionService.GetTestQuestion(answerPayload.QuestionId);
            var updatedAnswer = await _answerService.UpdateTestAnswer(id, answerPayload, question.Type);
            await _testService.UpdateTest(question.Test.Id);

            return await PrepareAnswer(updatedAnswer);
        }

        public async Task<IEnumerable<TestAnswer>> MoveTestAnswer(int questionId, int oldIndex, int newIndex)
        {
            var question = await _questionService.GetTestQuestion(questionId);
            var movedAnswers = await _answerService.MoveTestAnswer(questionId, oldIndex, newIndex);
            await _testService.UpdateTest(question.Test.Id);

            return await PrepareAnswers(movedAnswers);
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
