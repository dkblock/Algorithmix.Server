using System.Collections.Generic;

namespace Algorithmix.Models.SearchFilters
{
    public class TestFilterPayload
    {
        public string UserId { get; set; }
        public string SearchText { get; set; }
        public bool OnlyPassed { get; set; }
        public IEnumerable<string> AlgorithmIds { get; set; } = new List<string>();
    }
}
