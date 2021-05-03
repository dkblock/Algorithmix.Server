using Algorithmix.Models.Users;
using System;
using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class UserTestResult
    {
        public int Result { get; set; }
        public int CorrectAnswers { get; set; }
        public bool IsPassed { get; set; }
        public DateTime PassingTime { get; set; }
        public Test Test { get; set; }
        public ApplicationUser User { get; set; }
        public IEnumerable<UserAnswer> UserAnswers { get; set; }
    }
}
