using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class UserAnswerData
    {
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public IEnumerable<string> Answers { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
    }
}
