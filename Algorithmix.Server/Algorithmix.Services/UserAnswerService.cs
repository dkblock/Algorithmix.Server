using Algorithmix.Common.Extensions;
using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class UserAnswerService
    {
        private readonly UserAnswerRepository _userAnswerRepository;
        private readonly UserAnswerMapper _userAnswerMapper;

        public UserAnswerService(UserAnswerRepository userAnswerRepository, UserAnswerMapper userAnswerMapper)
        {
            _userAnswerRepository = userAnswerRepository;
            _userAnswerMapper = userAnswerMapper;
        }

        public async Task<UserAnswer> CreateUserAnswer(UserAnswerData userAnswerData)
        {
            var userAnswerEntity = _userAnswerMapper.ToEntity(userAnswerData);
            var createdUserAnswer = await _userAnswerRepository.CreateUserAnswer(userAnswerEntity);

            return _userAnswerMapper.ToModel(createdUserAnswer);
        }

        public async Task<bool> Exists(int questionId, string userId)
        {
            return await _userAnswerRepository.GetUserAnswerById(questionId, userId) != null;
        }

        public async Task<IEnumerable<UserAnswer>> GetUserAnswers(IEnumerable<int> questionIds, string userId)
        {
            var userAnswerEntities = await _userAnswerRepository.GetUserAnswers(ua => questionIds.Contains(ua.QuestionId) && ua.UserId == userId);
            return _userAnswerMapper.ToModelsCollection(userAnswerEntities);
        }

        public async Task<IEnumerable<UserAnswer>> GetUserAnswers(int questionId)
        {
            var userAnswerEntities = await _userAnswerRepository.GetUserAnswers(ua => ua.QuestionId == questionId);
            return _userAnswerMapper.ToModelsCollection(userAnswerEntities);
        }

        public async Task DeleteUserAnswer(int questionId, string userId)
        {
            await _userAnswerRepository.DeleteUserAnswer(questionId, userId);
        }

        public async Task DeleteUserAnswers(int questionId)
        {
            var userAnswers = await _userAnswerRepository.GetUserAnswers(ua => ua.QuestionId == questionId);
            await userAnswers.ForEachAsync(async ua => await _userAnswerRepository.DeleteUserAnswer(questionId, ua.UserId));
        }
    }
}
