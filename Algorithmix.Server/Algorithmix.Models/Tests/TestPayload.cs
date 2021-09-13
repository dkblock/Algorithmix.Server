using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class TestPayload
    {
        public string Name { get; set; }
        public IEnumerable<string> AlgorithmIds { get; set; }
        public string UserId { get; set; }
    }
}
