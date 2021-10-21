using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services.TestPass;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core.TestPass
{
    public class PublishedTestQuestionManager
    {
        private readonly PublishedTestAnswerService _answerService;
        private readonly PublishedTestQuestionService _questionService;
        private readonly PublishedTestService _testService;

        public PublishedTestQuestionManager(
            PublishedTestAnswerService answerService,
            PublishedTestQuestionService questionService,
            PublishedTestService testService)
        {
            _answerService = answerService;
            _questionService = questionService;
            _testService = testService;
        }

        public async Task CreateTestQuestion(TestQuestion testQuestion)
        {
            await _questionService.CreateTestQuestion(testQuestion);
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

        public async Task UpdateTestQuestion(TestQuestion question)
        {
            await _questionService.UpdateTestQuestion(question);
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
