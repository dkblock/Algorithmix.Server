using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int? PreviousQuestionId { get; set; }
        public int? NextQuestionId { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public Test Test { get; set; }
        public IEnumerable<TestAnswer> Answers { get; set; }
    }
}
