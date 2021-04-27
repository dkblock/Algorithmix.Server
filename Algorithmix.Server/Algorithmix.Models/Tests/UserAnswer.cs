using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class UserAnswer
    {
        public bool IsCorrect { get; set; }
        public IEnumerable<string> Answers { get; set; }
        public TestQuestion Question { get; set; }
    }
}
