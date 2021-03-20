using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Models.Tests;

namespace VisualAlgorithms.Mappers
{
    public class TestAnswersMapper
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
                QuestionId = answerEntity.QuestionId
            };
        }

        public IEnumerable<TestAnswer> ToModelsCollection(IEnumerable<TestAnswerEntity> answerEntities)
        {
            return answerEntities.Select(entity => ToModel(entity));
        }             
    }
}
