using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestQuestionService
    {
        private readonly TestQuestionMapper _questionMapper;
        private readonly TestQuestionRepository _questionRepository;

        public TestQuestionService(TestQuestionMapper questionMapper, TestQuestionRepository questionRepository)
        {
            _questionMapper = questionMapper;
            _questionRepository = questionRepository;
        }

        public async Task<TestQuestion> CreateTestQuestion(TestQuestionPayload questionPayload)
        {
            var questionEntity = _questionMapper.ToEntity(questionPayload);
            var createdQuestion = await _questionRepository.CreateTestQuestion(questionEntity);

            return _questionMapper.ToModel(createdQuestion);
        }

        public async Task<bool> Exists(int questionId, int testId)
        {
            var questionEntity = await _questionRepository.GetTestQuestionById(questionId);
            return questionEntity != null && questionEntity.TestId == testId;
        }

        public async Task<TestQuestion> GetTestQuestion(int id)
        {
            var questionEntity = await _questionRepository.GetTestQuestionById(id);
            return _questionMapper.ToModel(questionEntity);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(int testId)
        {
            var questionEntities = await _questionRepository.GetTestQuestions(q => q.TestId == testId);
            return _questionMapper.ToModelsCollection(questionEntities);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questionEntities = await _questionRepository.GetTestQuestions(q => testIds.Contains(q.TestId));
            return _questionMapper.ToModelsCollection(questionEntities);
        }

        public async Task DeleteTestQuestion(int id)
        {
            await _questionRepository.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(int questionId, TestQuestionPayload questionPayload)
        {
            var questionEntity = _questionMapper.ToEntity(questionPayload, questionId);
            var updatedQuestion = await _questionRepository.UpdateTestQuestion(questionEntity);

            return _questionMapper.ToModel(updatedQuestion);
        }
    }
}
