using Algorithmix.Mappers;
using Algorithmix.Models.Tests;
using Algorithmix.Repository.TestPass;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Services.TestPass
{
    public class PublishedTestQuestionService
    {
        private readonly TestQuestionMapper _questionMapper;
        private readonly PublishedTestQuestionRepository _questionRepository;

        public PublishedTestQuestionService(TestQuestionMapper questionMapper, PublishedTestQuestionRepository questionRepository)
        {
            _questionMapper = questionMapper;
            _questionRepository = questionRepository;
        }

        public async Task<TestQuestion> CreateTestQuestion(TestQuestion question)
        {
            var questionEntity = _questionMapper.ToEntity(question);
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
            return _questionMapper.ToModelCollection(questionEntities);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questionEntities = await _questionRepository.GetTestQuestions(q => testIds.Contains(q.TestId));
            return _questionMapper.ToModelCollection(questionEntities);
        }

        public async Task DeleteTestQuestion(int id)
        {
            await _questionRepository.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(TestQuestion testQuestion)
        {
            var questionEntity = _questionMapper.ToEntity(testQuestion);
            var updatedQuestion = await _questionRepository.UpdateTestQuestion(questionEntity);

            return _questionMapper.ToModel(updatedQuestion);
        }
    }
}
