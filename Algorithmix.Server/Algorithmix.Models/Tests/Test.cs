using Algorithmix.Models.Algorithms;
using Algorithmix.Models.Users;
using System;
using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }
        public int AverageResult { get; set; }
        public int PassesCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool UserHasAccess { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public IEnumerable<Algorithm> Algorithms { get; set; }
        public IEnumerable<TestQuestion> Questions { get; set; }
        public UserTestResult UserResult { get; set; }
    }
}
