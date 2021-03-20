using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Models.Tests;

namespace VisualAlgorithms.Mappers
{
    public class TestQuestionsMapper
    {
        public TestQuestionEntity ToEntity(TestQuestionPayload questionPayload, int? id = null)
        {
            return new TestQuestionEntity
            {
                Id = id ?? 0,
                Value = questionPayload.Value,
                Image = questionPayload.Image,
                CorrectAnswerId = questionPayload.CorrectAnswerId,
                TestId = questionPayload.TestId
            };
        }

        public TestQuestion ToModel(TestQuestionEntity questionEntity, IEnumerable<TestAnswer> questionAnswers = null)
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
                Answers = questionAnswers ?? new List<TestAnswer>()
            };
        }

        public IEnumerable<TestQuestion> ToModelsCollection(IEnumerable<TestQuestionEntity> questionEntities, IEnumerable<TestAnswer> questionAnswers)
        {
            foreach (var entity in questionEntities)
            {
                var currentQuestionAnswers = questionAnswers.Where(a => a.QuestionId == entity.Id);
                yield return ToModel(entity, currentQuestionAnswers);
            }
        }
    }
}
