namespace Algorithmix.Entities
{
    public class TestQuestionEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int? PreviousQuestionId { get; set; }
        public int? NextQuestionId { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public int TestId { get; set; }
    }
}
