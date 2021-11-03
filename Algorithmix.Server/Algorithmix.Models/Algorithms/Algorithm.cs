using Algorithmix.Models.Tests;
using System.Collections.Generic;

namespace Algorithmix.Models.Algorithms
{
    public class Algorithm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int TimeComplexityId { get; set; }
        public bool HasConstructor { get; set; }
        public AlgorithmTimeComplexity TimeComplexity { get; set; }
        public IEnumerable<Test> Tests { get; set; }
    }
}
