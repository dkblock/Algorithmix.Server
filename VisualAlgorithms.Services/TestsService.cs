using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Common;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;
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
            var testQuestions = await _questionsService.GetTestQuestions(id);

            return _testsMapper.ToDomain(testEntity, testQuestions);
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var testEntities = await _testsRepository.GetAllTests();
            return await GetTests(testEntities);
        }

        public async Task<IEnumerable<Test>> GetTests(int algorithmId)
        {
            var testEntities = await _testsRepository.GetTests(t => t.AlgorithmId == algorithmId);
            return await GetTests(testEntities);
        }

        public async Task<IEnumerable<Test>> GetTests(IEnumerable<int> algorithmIds)
        {
            var testEntities = await _testsRepository.GetTests(t => algorithmIds.Contains(t.AlgorithmId));
            return await GetTests(testEntities);
        }

        private async Task<IEnumerable<Test>> GetTests(IEnumerable<TestEntity> testEntities)
        {
            var testIds = testEntities.Select(t => t.Id);
            var testQuestions = await _questionsService.GetTestQuestions(testIds);

            return _testsMapper.ToDomainCollection(testEntities, testQuestions);
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
