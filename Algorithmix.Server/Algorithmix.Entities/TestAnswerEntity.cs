namespace Algorithmix.Entities
{
    public class TestAnswerEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public int? PreviousAnswerId { get; set; }
        public int? NextAnswerId { get; set; }
        public int QuestionId { get; set; }
    }
}
