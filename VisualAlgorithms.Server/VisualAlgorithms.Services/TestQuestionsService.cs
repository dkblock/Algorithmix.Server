using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Extensions;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Tests;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestQuestionsService
    {
        private readonly TestAnswersService _answersService;
        private readonly TestQuestionsMapper _questionsMapper;
        private readonly TestQuestionsRepository _questionsRepository;

        public TestQuestionsService(TestAnswersService answersService, TestQuestionsMapper questionsMapper, TestQuestionsRepository questionsRepository)
        {
            _answersService = answersService;
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
            var questionAnswers = await _answersService.GetTestAnswers(id);

            return _questionsMapper.ToModel(questionEntity, questionAnswers);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(int testId)
        {
            var questionEntities = await _questionsRepository.GetTestQuestions(q => q.TestId == testId);
            return await GetTestQuestions(questionEntities);
        }

        public async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<int> testIds)
        {
            var questionEntities = await _questionsRepository.GetTestQuestions(q => testIds.Contains(q.TestId));
            return await GetTestQuestions(questionEntities);
        }

        private async Task<IEnumerable<TestQuestion>> GetTestQuestions(IEnumerable<TestQuestionEntity> questionEntities)
        {
            var questionIds = questionEntities.Select(q => q.Id);
            var questionAnswers = await _answersService.GetTestAnswers(questionIds);

            return _questionsMapper.ToModelsCollection(questionEntities, questionAnswers);
        }

        public async Task DeleteTestQuestion(int id)
        {
            var question = await GetTestQuestion(id);
            await question.Answers.ForEachAsync(async a => await _answersService.DeleteTestAnswer(a.Id));
            await _questionsRepository.DeleteTestQuestion(id);
        }

        public async Task<TestQuestion> UpdateTestQuestion(int questionId, TestQuestionPayload questionPayload)
        {
            var questionEntity = _questionsMapper.ToEntity(questionPayload, questionId);
            var updatedQuestion = await _questionsRepository.UpdateTestQuestion(questionEntity);
            var questionAnswers = await _answersService.GetTestAnswers(questionId);

            return _questionsMapper.ToModel(updatedQuestion, questionAnswers);
        }
    }
}
