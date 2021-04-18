using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class UserAnswerPayload
    {
        public IEnumerable<string> Answers { get; set; }
        public int QuestionId { get; set; }
    }
}
