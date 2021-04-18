using System;
using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class UserTestResult
    {
        public string UserId { get; set; }
        public int TestId { get; set; }
        public int Result { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public bool IsPassed { get; set; }
        public DateTime PassingTime { get; set; }
        public IEnumerable<UserAnswer> UserAnswers { get; set; }
    }
}
