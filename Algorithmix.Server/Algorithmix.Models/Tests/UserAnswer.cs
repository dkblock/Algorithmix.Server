namespace Algorithmix.Models.Tests
{
    public class UserAnswer
    {
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public TestQuestion Question { get; set; }
    }
}
