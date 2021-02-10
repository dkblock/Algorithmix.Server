using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Common;
using VisualAlgorithms.Domain;
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
            var question = _questionsMapper.ToDomain(questionEntity);
            question.TestAnswers = await _answersService.GetAllTestQuestionAnswers(id);

            return question;
        }

        public async Task<IEnumerable<TestQuestion>> GetAllTestQuestions(int testId)
        {
            var questionEntities = await _questionsRepository.GetTestQuestions(q => q.TestId == testId);
            var questions = _questionsMapper.ToDomainCollection(questionEntities);
            await questions.ForEachAsync(async q => q.TestAnswers = await _answersService.GetAllTestQuestionAnswers(q.Id));

            return questions;
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
