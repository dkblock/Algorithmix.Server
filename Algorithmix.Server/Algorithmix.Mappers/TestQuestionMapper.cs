using Algorithmix.Entities;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class TestQuestionMapper
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

        public TestQuestion ToModel(TestQuestionEntity questionEntity)
        {
            if (questionEntity == null)
                return null;

            return new TestQuestion
            {
                Id = questionEntity.Id,
                Value = questionEntity.Value,
                Image = questionEntity.Image,
                CorrectAnswerId = questionEntity.CorrectAnswerId,
                Test = new Test { Id = questionEntity.TestId },
                Answers = new List<TestAnswer>()
            };
        }

        public IEnumerable<TestQuestion> ToModelsCollection(IEnumerable<TestQuestionEntity> questionEntities)
        {
            return questionEntities.Select(entity => ToModel(entity));
        }
    }
}
