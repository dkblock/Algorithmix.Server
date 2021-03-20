using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestQuestionsService
    {
        private readonly TestQuestionsMapper _questionsMapper;
        private readonly TestQuestionsRepository _questionsRepository;

        public TestQuestionsService(TestQuestionsMapper questionsMapper, TestQuestionsRepository questionsRepository)
        {
            _questionsMapper = questionsMapper;
            _questionsRepository = questionsRepository;
        }

        public async Task<TestQuestion> CreateTestQuestion(TestQuestionPayload questionPayload)
        {
            var questionEntity = _questionsMapper.ToEntity(questionPayload);
            var createdQuestion = await _questionsRepository.CreateTestQuestion(questionEntity);

            return _questionsMapper.ToModel(createdQuestion);
        }

        public async Task<bool> Exists(int questionId, int testId)
        {
            var questionEntity = await _questionsRepository.GetTestQuestionById(questionId);
            return questionEntity != null && questionEntity.TestId == testId;
        }

        public async Task<TestQuestion> GetTestQuestion(int id)
        {
            var questionEntity = await _questionsRepository.GetTestQuestionById(id);
            return _questionsMapper.ToModel(questionEntity);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(int testId)
        {
            var questionEntities = await _questionsRepository.GetTestQuestions(q => q.TestId == testId);
            return _questionsMapper.ToModelsCollection(questionEntities);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questionEntities = await _questionsRepository.GetTestQuestions(q => testIds.Contains(q.TestId));
            return _questionsMapper.ToModelsCollection(questionEntities);
        }

        public async Task DeleteTestQuestion(int id)
        {
            await _questionsRepository.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(int questionId, TestQuestionPayload questionPayload)
        {
            var questionEntity = _questionsMapper.ToEntity(questionPayload, questionId);
            var updatedQuestion = await _questionsRepository.UpdateTestQuestion(questionEntity);

            return _questionsMapper.ToModel(updatedQuestion);
        }
    }
}
