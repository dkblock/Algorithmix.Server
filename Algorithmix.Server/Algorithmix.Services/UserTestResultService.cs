using Algorithmix.Entities;
using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class UserTestResultService
    {
        private readonly UserTestResultRepository _userTestResultRepository;
        private readonly UserTestResultMapper _userTestResultMapper;

        public UserTestResultService(UserTestResultRepository userTestResultRepository, UserTestResultMapper userTestResultMapper)
        {
            _userTestResultRepository = userTestResultRepository;
            _userTestResultMapper = userTestResultMapper;
        }

        public async Task<UserTestResult> CreateUserTestResult(UserTestResultData userTestResultData)
        {
            var userTestResultEntity = _userTestResultMapper.ToEntity(userTestResultData);
            var createdUserTestResult = await _userTestResultRepository.CreateUserTestResult(userTestResultEntity);

            return _userTestResultMapper.ToModel(createdUserTestResult);
        }

        public async Task<bool> Exists(int testId, string userId)
        {
            return await _userTestResultRepository.GetUserTestResultById(testId, userId) != null;
        }

        public async Task<UserTestResult> GetUserTestResult(int testId, string userId)
        {
            var userTestResult = await _userTestResultRepository.GetUserTestResultById(testId, userId);
            return _userTestResultMapper.ToModel(userTestResult);
        }

        public async Task DeleteUserTestResult(int testId, string userId)
        {
            await _userTestResultRepository.DeleteUserTestResult(testId, userId);
        }
    }
}
