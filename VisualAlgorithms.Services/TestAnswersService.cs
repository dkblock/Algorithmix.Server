using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Mappers;
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

        public async Task<int> AddTestAnswer(TestAnswer answer)
        {
            var answerEntity = _answersMapper.ToEntity(answer);
            return await _answersRepository.AddTestAnswer(answerEntity);
        }

        public async Task<TestAnswer> GetTestAnswer(int id)
        {
            var answerEntity = await _answersRepository.GetTestAnswerById(id);
            return _answersMapper.ToDomain(answerEntity);
        }

        public async Task<IEnumerable<TestAnswer>> GetAllTestQuestionAnswers(int questionId)
        {
            var answerEntities = await _answersRepository.GetTestAnswers(a => a.TestQuestionId == questionId);
            return _answersMapper.ToDomainCollection(answerEntities);
        }

        public async Task RemoveTestAnswer(int id)
        {
            await _answersRepository.RemoveTestAnswer(id);
        }

        public async Task UpdateTestAnswer(TestAnswer answer)
        {
            var answerEntity = _answersMapper.ToEntity(answer);
            await _answersRepository.UpdateTestAnswer(answerEntity);
        }
    }
}
