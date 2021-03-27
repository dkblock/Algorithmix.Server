using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestAnswerService
    {
        private readonly TestAnswerMapper _answerMapper;
        private readonly TestAnswerRepository _answerRepository;

        public TestAnswerService(TestAnswerMapper answerMapper, TestAnswerRepository answerRepository)
        {
            _answerMapper = answerMapper;
            _answerRepository = answerRepository;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswerPayload answerPayload)
        {
            var answerEntity = _answerMapper.ToEntity(answerPayload);
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
            return _answerMapper.ToModelsCollection(answerEntities);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(IEnumerable<int> questionIds)
        {
            var answerEntities = await _answerRepository.GetTestAnswers(a => questionIds.Contains(a.QuestionId));
            return _answerMapper.ToModelsCollection(answerEntities);
        }

        public async Task DeleteTestAnswer(int id)
        {
            await _answerRepository.DeleteTestAnswer(id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(int id, TestAnswerPayload answerPayload)
        {
            var answerEntity = _answerMapper.ToEntity(answerPayload, id);
            var updatedAnswer = await _answerRepository.UpdateTestAnswer(answerEntity);

            return _answerMapper.ToModel(updatedAnswer);
        }
    }
}
