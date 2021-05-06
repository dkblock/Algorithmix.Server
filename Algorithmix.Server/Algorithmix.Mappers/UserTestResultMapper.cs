using Algorithmix.Entities.Test;
using Algorithmix.Models.Tests;
using Algorithmix.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class UserTestResultMapper
    {
        public UserTestResultEntity ToEntity(UserTestResultData userTestResultEntity)
        {
            return new UserTestResultEntity
            {
                TestId = userTestResultEntity.TestId,
                UserId = userTestResultEntity.UserId,
                CorrectAnswers = userTestResultEntity.CorrectAnswers,
                Result = userTestResultEntity.Result,
                IsPassed = userTestResultEntity.IsPassed,
                PassingTime = userTestResultEntity.PassingTime
            };
        }

        public UserTestResult ToModel(UserTestResultEntity userTestResultEntity)
        {
            return new UserTestResult
            {
                CorrectAnswers = userTestResultEntity.CorrectAnswers,
                Result = userTestResultEntity.Result,
                IsPassed = userTestResultEntity.IsPassed,
                PassingTime = userTestResultEntity.PassingTime,
                Test = new Test { Id = userTestResultEntity.TestId },
                User = new ApplicationUser { Id = userTestResultEntity.UserId },
                UserAnswers = new List<UserAnswer>()
            };
        }

        public IEnumerable<UserTestResult> ToModelsCollection(IEnumerable<UserTestResultEntity> userTestResultEntities)
        {
            return userTestResultEntities.Select(entity => ToModel(entity));
        }
    }
}
