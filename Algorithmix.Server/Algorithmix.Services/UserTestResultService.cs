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

        public async Task<UserTestResult> CreateUserTestResult(UserTestResultEntity userTestResultEntity)
        {
            var createdUserTestResult = await _userTestResultRepository.CreateUserTestResult(userTestResultEntity);
            return _userTestResultMapper.ToModel(createdUserTestResult);
        }

        public async Task DeleteUserTestResult(int testId, string userId)
        {
            await _userTestResultRepository.DeleteUserTestResult(testId, userId);
        }
    }
}
