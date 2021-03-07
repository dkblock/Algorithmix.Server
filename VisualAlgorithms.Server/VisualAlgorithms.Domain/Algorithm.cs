using System.Collections.Generic;

namespace VisualAlgorithms.Domain
{
    public class Algorithm
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int AlgorithmTimeComplexityId { get; set; }
        public AlgorithmTimeComplexity AlgorithmTimeComplexity { get; set; }
        public IEnumerable<Test> Tests { get; set; }
    }
}
