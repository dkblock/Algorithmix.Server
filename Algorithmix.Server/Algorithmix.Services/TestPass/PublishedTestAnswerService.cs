using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository.TestPass;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services.TestPass
{
    public class PublishedTestAnswerService
    {
        private readonly TestAnswerMapper _answerMapper;
        private readonly PublishedTestAnswerRepository _answerRepository;

        public PublishedTestAnswerService(TestAnswerMapper answerMapper, PublishedTestAnswerRepository answerRepository)
        {
            _answerMapper = answerMapper;
            _answerRepository = answerRepository;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswer answer)
        {
            var answerEntity = _answerMapper.ToEntity(answer);
            var createdAnswer = await _answerRepository.CreateTestAnswer(answerEntity);

            return _answerMapper.ToModel(createdAnswer);
        }

        public async Task<bool> Exists(int answerId, int questionId)
        {
            var answerEntity = await _answerRepository.GetTestAnswerById(answerId);
            return answerEntity != null && answerEntity.QuestionId == questionId;
        }

        public async Task<TestAnswer> GetTestAnswer(int id)
        {
            var answerEntity = await _answerRepository.GetTestAnswerById(id);
            return _answerMapper.ToModel(answerEntity);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(int questionId)
        {
            var answerEntities = await _answerRepository.GetTestAnswers(a => a.QuestionId == questionId);
            return _answerMapper.ToModelCollection(answerEntities);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(IEnumerable<int> questionIds)
        {
            var answerEntities = await _answerRepository.GetTestAnswers(a => questionIds.Contains(a.QuestionId));
            return _answerMapper.ToModelCollection(answerEntities);
        }

        public async Task DeleteTestAnswer(int id)
        {
            await _answerRepository.DeleteTestAnswer(id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(TestAnswer testAnswer)
        {
            var answerEntity = _answerMapper.ToEntity(testAnswer);
            var updatedAnswer = await _answerRepository.UpdateTestAnswer(answerEntity);

            return _answerMapper.ToModel(updatedAnswer);
        }
    }
}
