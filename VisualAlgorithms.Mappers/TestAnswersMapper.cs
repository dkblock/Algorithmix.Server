using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Mappers
{
    public class TestAnswersMapper
    {
        public TestAnswer ToDomain(TestAnswerEntity answerEntity)
        {
            return new TestAnswer
            {
                Id = answerEntity.Id,
                Value = answerEntity.Value,
                TestQuestionId = answerEntity.TestQuestionId
            };
        }

        public TestAnswerEntity ToEntity(TestAnswer answer)
        {
            return new TestAnswerEntity
            {
                Id = answer.Id,
                Value = answer.Value,
                TestQuestionId = answer.TestQuestionId
            };
        }
    }
}
