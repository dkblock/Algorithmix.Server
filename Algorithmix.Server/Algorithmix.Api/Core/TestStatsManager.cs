using Algorithmix.Api.Core.TestDesign;
using Algorithmix.Common.Constants;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public interface ITestStatsManager
    {
        Task<TestStat> GetTestStats(int testId);
    }

    public class TestStatsManager : ITestStatsManager
    {
        private readonly TestManager _testManager;
        private readonly TestQuestionManager _questionManager;
        private readonly UserAnswerManager _userAnswerManager;

        public TestStatsManager(TestManager testManager, TestQuestionManager questionManager, UserAnswerManager userAnswerManager)
        {
            _testManager = testManager;
            _questionManager = questionManager;
            _userAnswerManager = userAnswerManager;
        }

        public async Task<TestStat> GetTestStats(int testId)
        {
            var test = await _testManager.GetTest(testId);
            var questions = await _questionManager.GetTestQuestions(testId);
            var questionStats = new List<TestQuestionStat>();

            foreach (var question in questions)
            {
                var questionStat = await GetQuestionStats(question);
                questionStats.Add(questionStat);
            }

            return new TestStat
            {
                Test = test,
                QuestionStats = questionStats
            };
        }

        private async Task<TestQuestionStat> GetQuestionStats(TestQuestion question)
        {
            var userAnswers = await _userAnswerManager.GetUserAnswers(question.Id);
            var averageResult = userAnswers.Any() ? (double)userAnswers.Count(ua => ua.IsCorrect) / userAnswers.Count() * 100 : 0;
            var passesCount = userAnswers.Count();
            var correctAnswersCount = userAnswers.Count(ua => ua.IsCorrect);

            return new TestQuestionStat
            {
                Question = question,
                AverageResult = (int)averageResult,
                PassesCount = passesCount,
                CorrectAnswersCount = correctAnswersCount,
                IncorrectAnswersCount = passesCount - correctAnswersCount,
                UserAnswersRatio = GetUserAnswersRatio(question, userAnswers)
            };
        }

        private IDictionary<string, int> GetUserAnswersRatio(TestQuestion question, IEnumerable<UserAnswer> userAnswers)
        {
            var userAnswersRatio = new Dictionary<string, int>();
            var groupedAnswers = userAnswers
                .SelectMany(ua => ua.Answers)
                .GroupBy(ua => ua);

            if (question.Type == TestQuestionTypes.FreeAnswerQuestion)
            {
                foreach (var groupedAnswer in groupedAnswers)
                    userAnswersRatio.Add(groupedAnswer.Key, groupedAnswer.Count());
            }
            else
            {
                foreach (var answer in question.Answers)
                {
                    var answerId = answer.Id.ToString();
                    var answersCount = groupedAnswers.Any(a => a.Key == answerId)
                        ? groupedAnswers.Single(a => a.Key == answerId).Count()
                        : 0;

                    userAnswersRatio.Add(answerId, answersCount);
                }
            }

            return userAnswersRatio;
        }
    }
}
