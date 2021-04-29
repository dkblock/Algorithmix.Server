namespace Algorithmix.Entities.Test
{
    public class BaseTestAnswerEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public int? PreviousAnswerId { get; set; }
        public int? NextAnswerId { get; set; }
        public int QuestionId { get; set; }
    }
}
