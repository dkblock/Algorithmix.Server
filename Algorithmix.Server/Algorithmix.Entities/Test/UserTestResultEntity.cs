using System;

namespace Algorithmix.Entities.Test
{
    public class UserTestResultEntity
    {
        public string UserId { get; set; }
        public int TestId { get; set; }
        public int Result { get; set; }
        public int CorrectAnswers { get; set; }
        public bool IsPassed { get; set; }
        public DateTime PassingTime { get; set; }
    }
}
