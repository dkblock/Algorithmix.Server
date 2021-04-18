using Algorithmix.Entities;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class UserAnswerMapper
    {
        public UserAnswerEntity ToEntity(UserAnswerPayload userAnswerPayload, string userId)
        {
            return new UserAnswerEntity
            {
                QuestionId = userAnswerPayload.QuestionId,
                UserId = userId
            };
        }

        public UserAnswer ToDomain(UserAnswerEntity userAnswerEntity)
        {
            return new UserAnswer
            {
                Value = userAnswerEntity.Value,
                IsCorrect = userAnswerEntity.IsCorrect,
                QuestionId = userAnswerEntity.QuestionId,
                UserId = userAnswerEntity.UserId
            };
        }

        public IEnumerable<UserAnswer> ToDomainCollection(IEnumerable<UserAnswerEntity> userAnswerEntities)
        {
            return userAnswerEntities.Select(ua => ToDomain(ua));
        }
    }
}
