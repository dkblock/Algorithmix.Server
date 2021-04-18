using Algorithmix.Entities;
using Algorithmix.Models.Tests;

namespace Algorithmix.Mappers
{
    public class UserTestResultMapper
    {
        public UserTestResult ToModel(UserTestResultEntity userTestResultEntity)
        {
            return new UserTestResult
            {
                TestId = userTestResultEntity.TestId,
                UserId = userTestResultEntity.UserId,
                CorrectAnswers = userTestResultEntity.CorrectAnswers,
                TotalQuestions = userTestResultEntity.TotalQuestions,
                Result = userTestResultEntity.Result,
                IsPassed = userTestResultEntity.IsPassed,
                PassingTime = userTestResultEntity.PassingTime
            };
        }
    }
}
