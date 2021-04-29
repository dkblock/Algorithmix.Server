using Algorithmix.Entities.Test;
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
                IsCorrect = answerPayload.IsCorrect,
                PreviousAnswerId = answerPayload.PreviousAnswerId,
                NextAnswerId = answerPayload.NextAnswerId,
                QuestionId = answerPayload.QuestionId
            };
        }

        public PublishedTestAnswerEntity ToEntity(TestAnswer answer)
        {
            return new PublishedTestAnswerEntity
            {
                Id = answer.Id,
                Value = answer.Value,
                IsCorrect = answer.IsCorrect,
                PreviousAnswerId = answer.PreviousAnswerId,
                NextAnswerId = answer.NextAnswerId,
                QuestionId = answer.Question.Id
            };
        }

        public TestAnswer ToModel(BaseTestAnswerEntity answerEntity)
        {
            if (answerEntity == null)
                return null;

            return new TestAnswer
            {
                Id = answerEntity.Id,
                Value = answerEntity.Value,
                IsCorrect = answerEntity.IsCorrect,
                PreviousAnswerId = answerEntity.PreviousAnswerId,
                NextAnswerId = answerEntity.NextAnswerId,
                Question = new TestQuestion { Id = answerEntity.QuestionId }
            };
        }

        public IEnumerable<TestAnswer> ToModelCollection(IEnumerable<BaseTestAnswerEntity> answerEntities)
        {
            return answerEntities.Select(entity => ToModel(entity));
        }
    }
}
