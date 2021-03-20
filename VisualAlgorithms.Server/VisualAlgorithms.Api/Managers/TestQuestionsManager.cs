using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Extensions;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Services;

namespace VisualAlgorithms.Api.Managers
{
    public class TestQuestionsManager
    {
        private readonly TestAnswersService _answersService;
        private readonly TestQuestionsService _questionsService;
        private readonly TestsService _testsService;

        public TestQuestionsManager(TestAnswersService answersService, TestQuestionsService questionsService, TestsService testsService)
        {
            _answersService = answersService;
            _questionsService = questionsService;
            _testsService = testsService;
        }

        public async Task<TestQuestion> CreateTestQuestion(TestQuestionPayload questionPayload)
        {
            var createdQuestion = await _questionsService.CreateTestQuestion(questionPayload);
            return await PrepareQuestion(createdQuestion);
        }

        public async Task<bool> Exists(int questionId, int testId)
        {
            return await _questionsService.Exists(questionId, testId);
        }

        public async Task<TestQuestion> GetTestQuestion(int id)
        {
            var question = await _questionsService.GetTestQuestion(id);
            return await PrepareQuestion(question);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(int testId)
        {
            var questions = await _questionsService.GetTestQuestions(testId);
            return await PrepareQuestions(questions);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questions = await _questionsService.GetTestQuestions(testIds);
            return await PrepareQuestions(questions);
        }

        public async Task DeleteTestQuestion(int id)
        {
            await _questionsService.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(int questionId, TestQuestionPayload questionPayload)
        {
            var updatedQuestion = await _questionsService.UpdateTestQuestion(questionId, questionPayload);
            return await PrepareQuestion(updatedQuestion);
        }

        private async Task<TestQuestion> PrepareQuestion(TestQuestion question)
        {
            question.Test = await _testsService.GetTest(question.Test.Id);
            question.Answers = await _answersService.GetTestAnswers(question.Id);

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
