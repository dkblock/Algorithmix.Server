using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public int CorrectAnswerId { get; set; }
        public Test Test { get; set; }
        public IEnumerable<TestAnswer> Answers { get; set; }
    }
}
