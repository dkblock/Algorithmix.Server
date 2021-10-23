using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class TestQuestionStat
    {
        public TestQuestion Question { get; set; }
        public int AverageResult { get; set; }
        public int PassesCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int IncorrectAnswersCount { get; set; }
        public IDictionary<string, int> UserAnswersRatio { get; set; }
    }
}
