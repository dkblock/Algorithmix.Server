using System.Collections.Generic;
using System.Threading.Tasks;
using VisualAlgorithms.Common;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Repository;

namespace VisualAlgorithms.Services
{
    public class TestsService
    {
        private readonly TestsMapper _testsMapper;
        private readonly TestsRepository _testsRepository;
        private readonly TestQuestionsService _questionsService;

        public TestsService(TestsMapper testsMapper, TestsRepository testsRepository, TestQuestionsService questionsService)
        {
            _testsMapper = testsMapper;
            _testsRepository = testsRepository;
            _questionsService = questionsService;
        }

        public async Task<int> AddTest(Test test)
        {
            var testEntity = _testsMapper.ToEntity(test);
            return await _testsRepository.AddTest(testEntity);
        }

        public async Task<Test> GetTest(int id)
        {
            var testEntity = await _testsRepository.GetTestById(id);
            return _testsMapper.ToDomain(testEntity);
        }

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            var testEntities = await _testsRepository.GetAllTests();
            var tests = _testsMapper.ToDomainCollection(testEntities);
            await tests.ForEachAsync(async t => t.TestQuestions = await _questionsService.GetAllTestQuestions(t.Id));

            return tests;
        }

        public async Task RemoveTest(int id)
        {
            var test = await GetTest(id);
            await test.TestQuestions.ForEachAsync(async q => await _questionsService.RemoveTestQuestion(q.Id));
            await _testsRepository.RemoveTest(id);
        }

        public async Task UpdateTest(Test test)
        {
            var testEntity = _testsMapper.ToEntity(test);
            await _testsRepository.UpdateTest(testEntity);
        }
    }
}
