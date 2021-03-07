using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Mappers
{
    public class TestQuestionsMapper
    {
        public TestQuestion ToDomain(TestQuestionEntity questionEntity, IEnumerable<TestAnswer> questionAnswers)
        {
            if (questionEntity == null)
                return null;

            return new TestQuestion
            {
                Id = questionEntity.Id,
                Value = questionEntity.Value,
                Image = questionEntity.Image,
                CorrectAnswerId = questionEntity.CorrectAnswerId,
                TestId = questionEntity.TestId,
                TestAnswers = questionAnswers ?? new List<TestAnswer>()
            };
        }

        public IEnumerable<TestQuestion> ToDomainCollection(IEnumerable<TestQuestionEntity> questionEntities, IEnumerable<TestAnswer> questionAnswers)
        {
            foreach (var entity in questionEntities)
            {
                var currentQuestionAnswers = questionAnswers.Where(a => a.TestQuestionId == entity.Id);
                yield return ToDomain(entity, currentQuestionAnswers);
            }
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
