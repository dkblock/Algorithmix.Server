using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Mappers
{
    public class TestQuestionsMapper
    {
        public TestQuestion ToDomain(TestQuestionEntity questionEntity)
        {
            return new TestQuestion
            {
                Id = questionEntity.Id,
                Value = questionEntity.Value,
                Image = questionEntity.Image,
                CorrectAnswerId = questionEntity.CorrectAnswerId,
                TestId = questionEntity.TestId,
                TestAnswers = new List<TestAnswer>()
            };
        }

        public IEnumerable<TestQuestion> ToDomainCollection(IEnumerable<TestQuestionEntity> questionEntities)
        {
            return questionEntities.Select(entity => ToDomain(entity));
        }

        public TestQuestionEntity ToEntity(TestQuestion question)
        {
            return new TestQuestionEntity
            {
                Id = question.Id,
                Value = question.Value,
                Image = question.Image,
                CorrectAnswerId = question.CorrectAnswerId,
                TestId = question.TestId
            };
        }
    }
}
