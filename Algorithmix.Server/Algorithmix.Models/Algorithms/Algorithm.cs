using Algorithmix.Models.Tests;
using Algorithmix.Models.Users;
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
        public bool HasDescription { get; set; }
        public bool UserHasAccess { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public AlgorithmTimeComplexity TimeComplexity { get; set; }
    }
}
