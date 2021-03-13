using System.Collections.Generic;

namespace VisualAlgorithms.Domain
{
    public class Algorithm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int AlgorithmTimeComplexityId { get; set; }
        public AlgorithmTimeComplexity AlgorithmTimeComplexity { get; set; }
        public IEnumerable<Test> Tests { get; set; }
    }
}
