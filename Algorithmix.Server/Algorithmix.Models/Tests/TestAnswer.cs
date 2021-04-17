namespace Algorithmix.Models.Tests
{
    public class TestAnswer
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public int? PreviousAnswerId { get; set; }
        public int? NextAnswerId { get; set; }
        public TestQuestion Question { get; set; }
    }
}
