namespace VisualAlgorithms.Entities
{
    public class UserAnswerEntity
    {
        public string Value { get; set; }
        public bool IsCorrect { get; set; }
        public int TestQuestionId { get; set; }
        public string UserId { get; set; }
    }
}
