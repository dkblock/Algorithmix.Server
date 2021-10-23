using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class TestStat
    {
        public Test Test { get; set; }
        public IEnumerable<TestQuestionStat> QuestionStats { get; set; }
    }
}
