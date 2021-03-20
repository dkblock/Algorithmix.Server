using System.Collections.Generic;

namespace VisualAlgorithms.Models.Tests
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public int CorrectAnswerId { get; set; }
        public int TestId { get; set; }
        public IEnumerable<TestAnswer> Answers { get; set; }
    }
}
