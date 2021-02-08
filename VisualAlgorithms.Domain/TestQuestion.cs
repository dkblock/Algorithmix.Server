using System.Collections.Generic;

namespace VisualAlgorithms.Domain
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public int CorrectAnswerId { get; set; }
        public int TestId { get; set; }
        public List<TestAnswer> TestAnswers { get; set; }
    }
}
