using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Mappers
{
    public class TestAnswersMapper
    {
        public TestAnswer ToDomain(TestAnswerEntity answerEntity)
        {
            if (answerEntity == null)
                return null;

            return new TestAnswer
            {
                Id = answerEntity.Id,
                Value = answerEntity.Value,
                TestQuestionId = answerEntity.TestQuestionId
            };
        }

        public IEnumerable<TestAnswer> ToDomainCollection(IEnumerable<TestAnswerEntity> answerEntities)
        {
            return answerEntities.Select(entity => ToDomain(entity));
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
