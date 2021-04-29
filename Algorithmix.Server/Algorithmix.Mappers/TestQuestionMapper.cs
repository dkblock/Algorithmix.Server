using Algorithmix.Common.Constants;
using Algorithmix.Entities.Test;
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
                PreviousQuestionId = questionPayload.PreviousQuestionId,
                NextQuestionId = questionPayload.NextQuestionId,
                Image = questionPayload.Image,
                Type = questionPayload.Type,
                TestId = questionPayload.TestId
            };
        }

        public PublishedTestQuestionEntity ToEntity(TestQuestion question)
        {
            return new PublishedTestQuestionEntity
            {
                Id = question.Id,
                Value = question.Value,
                PreviousQuestionId = question.PreviousQuestionId,
                NextQuestionId = question.NextQuestionId,
                Image = question.Image?.Replace(TestQuestionImageDirectories.TestImagesDirectory, TestQuestionImageDirectories.PublishedTestImagesDirectory),
                Type = question.Type,
                TestId = question.Test.Id
            };
        }

        public TestQuestion ToModel(BaseTestQuestionEntity questionEntity)
        {
            if (questionEntity == null)
                return null;

            return new TestQuestion
            {
                Id = questionEntity.Id,
                Value = questionEntity.Value,
                PreviousQuestionId = questionEntity.PreviousQuestionId,
                NextQuestionId = questionEntity.NextQuestionId,
                Image = questionEntity.Image,
                Type = questionEntity.Type,
                Test = new Test { Id = questionEntity.TestId },
                Answers = new List<TestAnswer>()
            };
        }

        public IEnumerable<TestQuestion> ToModelCollection(IEnumerable<BaseTestQuestionEntity> questionEntities)
        {
            return questionEntities.Select(entity => ToModel(entity));
        }
    }
}
