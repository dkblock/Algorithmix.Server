using Algorithmix.Models.Algorithms;
using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AverageResult { get; set; }
        public Algorithm Algorithm { get; set; }
        public IEnumerable<TestQuestion> Questions { get; set; }
        public UserTestResult UserResult { get; set; }
    }
}
