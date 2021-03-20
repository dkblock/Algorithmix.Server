using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestAnswersService
    {
        private readonly TestAnswersMapper _answersMapper;
        private readonly TestAnswersRepository _answersRepository;

        public TestAnswersService(TestAnswersMapper answersMapper, TestAnswersRepository answersRepository)
        {
            _answersMapper = answersMapper;
            _answersRepository = answersRepository;
        }

        public async Task<TestAnswer> CreateTestAnswer(TestAnswerPayload answerPayload)
        {
            var answerEntity = _answersMapper.ToEntity(answerPayload);
            var createdAnswer = await _answersRepository.CreateTestAnswer(answerEntity);

            return _answersMapper.ToModel(createdAnswer);
        }

        public async Task<bool> Exists(int answerId, int questionId)
        {
            var answerEntity = await _answersRepository.GetTestAnswerById(answerId);
            return answerEntity != null && answerEntity.QuestionId == questionId;
        }

        public async Task<TestAnswer> GetTestAnswer(int id)
        {
            var answerEntity = await _answersRepository.GetTestAnswerById(id);
            return _answersMapper.ToModel(answerEntity);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(int questionId)
        {
            var answerEntities = await _answersRepository.GetTestAnswers(a => a.QuestionId == questionId);
            return _answersMapper.ToModelsCollection(answerEntities);
        }

        public async Task<IEnumerable<TestAnswer>> GetTestAnswers(IEnumerable<int> questionIds)
        {
            var answerEntities = await _answersRepository.GetTestAnswers(a => questionIds.Contains(a.QuestionId));
            return _answersMapper.ToModelsCollection(answerEntities);
        }

        public async Task DeleteTestAnswer(int id)
        {
            await _answersRepository.DeleteTestAnswer(id);
        }

        public async Task<TestAnswer> UpdateTestAnswer(int id, TestAnswerPayload answerPayload)
        {
            var answerEntity = _answersMapper.ToEntity(answerPayload, id);
            var updatedAnswer = await _answersRepository.UpdateTestAnswer(answerEntity);

            return _answersMapper.ToModel(updatedAnswer);
        }
    }
}
