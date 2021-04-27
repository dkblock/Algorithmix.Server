using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class TestQuestionManager
    {
        private readonly TestAnswerService _answerService;
        private readonly TestQuestionService _questionService;
        private readonly TestService _testService;

        public TestQuestionManager(TestAnswerService answerService, TestQuestionService questionService, TestService testService)
        {
            _answerService = answerService;
            _questionService = questionService;
            _testService = testService;
        }

        public async Task<TestQuestion> CreateTestQuestion(TestQuestionPayload questionPayload)
        {
            var createdQuestion = await _questionService.CreateTestQuestion(questionPayload);
            return await PrepareQuestion(createdQuestion);
        }

        public async Task<bool> Exists(int questionId, int testId)
        {
            return await _questionService.Exists(questionId, testId);
        }

        public async Task<TestQuestion> GetTestQuestion(int id)
        {
            var question = await _questionService.GetTestQuestion(id);
            return await PrepareQuestion(question);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(int testId)
        {
            var questions = await _questionService.GetTestQuestions(testId);
            return await PrepareQuestions(questions);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questions = await _questionService.GetTestQuestions(testIds);
            return await PrepareQuestions(questions);
        }

        public async Task DeleteTestQuestion(int id)
        {
            await _questionService.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(int id, TestQuestionPayload questionPayload)
        {
            var question = await _questionService.GetTestQuestion(id);
            var updatedQuestion = await _questionService.UpdateTestQuestion(id, questionPayload);

            if (question.Type != updatedQuestion.Type)
                await _answerService.UpdateTestAnswers(id, updatedQuestion.Type);

            return await PrepareQuestion(updatedQuestion);
        }

        public async Task<TestQuestion> UpdateTestQuestionImage(int id, string imagePath)
        {
            var updatedQuestion = await _questionService.UpdateTestQuestionImage(id, imagePath);
            return await PrepareQuestion(updatedQuestion);
        }

        public async Task<string> ClearTestQuestionImage(int id)
        {
            return await _questionService.ClearTestQuestionImage(id);
        }

        public async Task<IEnumerable<TestQuestion>> MoveTestQuestion(int testId, int oldIndex, int newIndex)
        {
            var movedQuestions = await _questionService.MoveTestQuestion(testId, oldIndex, newIndex);
            return await PrepareQuestions(movedQuestions);
        }

        private async Task<TestQuestion> PrepareQuestion(TestQuestion question)
        {
            question.Test = await _testService.GetTest(question.Test.Id);
            question.Answers = await _answerService.GetTestAnswers(question.Id);

            return question;
        }

        private async Task<IEnumerable<TestQuestion>> PrepareQuestions(IEnumerable<TestQuestion> questions)
        {
            var preparedQuestions = new List<TestQuestion>();
            await questions.ForEachAsync(async question => preparedQuestions.Add(await PrepareQuestion(question)));
            return preparedQuestions;
        }
    }
}
