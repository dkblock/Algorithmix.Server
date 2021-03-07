using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Extensions;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Mappers;
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

        public async Task<int> AddTestQuestion(TestQuestion question)
        {
            var questionEntity = _questionsMapper.ToEntity(question);
            return await _questionsRepository.AddTestQuestion(questionEntity);
        }

        public async Task<TestQuestion> GetTestQuestion(int id)
        {
            var questionEntity = await _questionsRepository.GetTestQuestionById(id);
            var questionAnswers = await _answersService.GetTestAnswers(id);

            return _questionsMapper.ToDomain(questionEntity, questionAnswers);
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

            return _questionsMapper.ToDomainCollection(questionEntities, questionAnswers);
        }

        public async Task RemoveTestQuestion(int id)
        {
            var question = await GetTestQuestion(id);
            await question.TestAnswers.ForEachAsync(async a => await _answersService.RemoveTestAnswer(a.Id));
            await _questionsRepository.RemoveTestQuestion(id);
        }

        public async Task UpdateTestQuestion(TestQuestion question)
        {
            var questionEntity = _questionsMapper.ToEntity(question);
            await _questionsRepository.UpdateTestQuestion(questionEntity);
        }
    }
}
