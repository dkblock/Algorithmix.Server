using Algorithmix.Entities;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class TestAnswerMapper
    {
        public TestAnswerEntity ToEntity(TestAnswerPayload answerPayload, int? id = null)
        {
            return new TestAnswerEntity
            {
                Id = id ?? 0,
                Value = answerPayload.Value,
                QuestionId = answerPayload.QuestionId
            };
        }

        public TestAnswer ToModel(TestAnswerEntity answerEntity)
        {
            if (answerEntity == null)
                return null;

            return new TestAnswer
            {
                Id = answerEntity.Id,
                Value = answerEntity.Value,
                Question = new TestQuestion { Id = answerEntity.QuestionId }
            };
        }

        public IEnumerable<TestAnswer> ToModelsCollection(IEnumerable<TestAnswerEntity> answerEntities)
        {
            return answerEntities.Select(entity => ToModel(entity));
        }             
    }
}
