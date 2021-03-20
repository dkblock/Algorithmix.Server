using System.Collections.Generic;
using VisualAlgorithms.Models.Algorithms;

namespace VisualAlgorithms.Models.Tests
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Algorithm Algorithm { get; set; }
        public IEnumerable<TestQuestion> Questions { get; set; }
    }
}
