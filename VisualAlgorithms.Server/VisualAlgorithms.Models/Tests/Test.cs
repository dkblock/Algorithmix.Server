using System.Collections.Generic;

namespace VisualAlgorithms.Models.Tests
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlgorithmId { get; set; }
        public IEnumerable<TestQuestion> Questions { get; set; }
    }
}
