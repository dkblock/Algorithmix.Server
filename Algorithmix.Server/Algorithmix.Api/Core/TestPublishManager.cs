using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Api.Core.TestPass;
using Algorithmix.Common.Extensions;
using Algorithmix.Models.Tests;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class TestPublishManager
    {
        private readonly TestManager _testManager;
        private readonly TestQuestionManager _questionManager;
        private readonly TestDataManager _testDataManager;

        private readonly PublishedTestManager _pubTestManager;
        private readonly PublishedTestAnswerManager _pubAnswerManager;
        private readonly PublishedTestQuestionManager _pubQuestionManager;

        private readonly UserAnswerService _userAnswerService;
        private readonly UserTestResultService _userTestResultService;

        public TestPublishManager(
            TestManager testManager,
            TestQuestionManager questionManager,
            TestDataManager testDataManager,
            PublishedTestManager pubTestManager,
            PublishedTestAnswerManager pubAnswerManager,
            PublishedTestQuestionManager pubQuestionManager,
            UserAnswerService userAnswerService,
            UserTestResultService userTestResultService)
        {
            _testManager = testManager;
            _questionManager = questionManager;
            _testDataManager = testDataManager;

            _pubTestManager = pubTestManager;
            _pubAnswerManager = pubAnswerManager;
            _pubQuestionManager = pubQuestionManager;

            _userAnswerService = userAnswerService;
            _userTestResultService = userTestResultService;
        }

        public async Task<Test> GetPreparedTest(int testId)
        {
            var test = await _testManager.GetTest(testId);
            test.Questions = await _questionManager.GetTestQuestions(testId);

            return test;
        }

        public async Task PublishTest(Test test, bool clearTestResults)
        {
            var questions = test.Questions;
            var questionIds = questions.Select(q => q.Id);
            var answers = questions.SelectMany(q => q.Answers);

            if (clearTestResults)
                await ClearTestResults(test, questions);

            await UpdatePublishedTest(test);
            await UpdatePublishedQuestions(questions, test.Id);
            await UpdatePublishedAnswers(answers, questionIds);
            _testDataManager.CopyTestQuestionImagesToPublishedTest(test.Id);

            await _testManager.PublishTest(test.Id);
        }

        private async Task ClearTestResults(Test test, IEnumerable<TestQuestion> questions)
        {
            await questions.ForEachAsync(async q => await _userAnswerService.DeleteUserAnswers(q.Id));
            await _userTestResultService.DeleteUserTestResults(test.Id);

            test.AverageResult = 0;
        }

        private async Task UpdatePublishedTest(Test test)
        {
            if (await _pubTestManager.Exists(test.Id))
                await _pubTestManager.UpdateTest(test);
            else
                await _pubTestManager.CreateTest(test);
        }

        private async Task UpdatePublishedQuestions(IEnumerable<TestQuestion> questions, int testId)
        {
            var existedQuestions = await _pubQuestionManager.GetTestQuestions(testId);

            foreach (var question in existedQuestions)
            {
                if (!questions.Any(q => q.Id == question.Id))
                    await _pubQuestionManager.DeleteTestQuestion(question.Id);
            }

            foreach (var question in questions)
            {
                if (await _pubQuestionManager.Exists(question.Id, question.Test.Id))
                    await _pubQuestionManager.UpdateTestQuestion(question);
                else
                    await _pubQuestionManager.CreateTestQuestion(question);
            }
        }

        private async Task UpdatePublishedAnswers(IEnumerable<TestAnswer> answers, IEnumerable<int> questionIds)
        {
            foreach (var questionId in questionIds)
            {
                var existedAnswers = await _pubAnswerManager.GetTestAnswers(questionId);

                foreach (var answer in existedAnswers)
                {
                    if (!answers.Any(a => a.Id == answer.Id))
                        await _pubAnswerManager.DeleteTestAnswer(answer.Id);
                }
            }

            foreach (var answer in answers)
            {
                if (await _pubAnswerManager.Exists(answer.Id, answer.Question.Id))
                    await _pubAnswerManager.UpdateTestAnswer(answer);
                else
                    await _pubAnswerManager.CreateTestAnswer(answer);
            }
        }
    }
}
