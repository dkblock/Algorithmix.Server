using System;

namespace Algorithmix.Models.Tests
{
    public class UserTestResultData
    {
        public string UserId { get; set; }
        public int TestId { get; set; }
        public int Result { get; set; }
        public int CorrectAnswers { get; set; }
        public bool IsPassed { get; set; }
        public DateTime PassingTime { get; set; }
    }
}
