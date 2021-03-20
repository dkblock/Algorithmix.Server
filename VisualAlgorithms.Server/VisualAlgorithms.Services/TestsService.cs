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

        public async Task<Test> CreateTest(TestPayload testPayload)
        {
            var testEntity = _testsMapper.ToEntity(testPayload);
            var createdTest = await _testsRepository.CreateTest(testEntity);

            return _testsMapper.ToModel(createdTest);
        }

        public async Task<bool> Exists(int id)
        {
            return await _testsRepository.GetTestById(id) != null;
        }

        public async Task<Test> GetTest(int id)
        {
            var testEntity = await _testsRepository.GetTestById(id);
            var testQuestions = await _questionsService.GetTestQuestions(id);

            return _testsMapper.ToModel(testEntity, testQuestions);
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            var testEntities = await _testsRepository.GetAllTests();
            return await GetTests(testEntities);
        }

        public async Task<IEnumerable<Test>> GetTests(string algorithmId)
        {
            var testEntities = await _testsRepository.GetTests(t => t.AlgorithmId == algorithmId);
            return await GetTests(testEntities);
        }

        public async Task<IEnumerable<Test>> GetTests(IEnumerable<string> algorithmIds)
        {
            var testEntities = await _testsRepository.GetTests(t => algorithmIds.Contains(t.AlgorithmId));
            return await GetTests(testEntities);
        }

        private async Task<IEnumerable<Test>> GetTests(IEnumerable<TestEntity> testEntities)
        {
            var testIds = testEntities.Select(t => t.Id);
            var testQuestions = await _questionsService.GetTestQuestions(testIds);

            return _testsMapper.ToModelsCollection(testEntities, testQuestions);
        }

        public async Task DeleteTest(int id)
        {
            var test = await GetTest(id);
            await test.Questions.ForEachAsync(async q => await _questionsService.DeleteTestQuestion(q.Id));
            await _testsRepository.DeleteTest(id);
        }

        public async Task<Test> UpdateTest(int id, TestPayload testPayload)
        {
            var testEntity = _testsMapper.ToEntity(testPayload, id);
            var updatedTest = await _testsRepository.UpdateTest(testEntity);
            var testQuestions = await _questionsService.GetTestQuestions(id);

            return _testsMapper.ToModel(updatedTest, testQuestions);
        }
    }
}
