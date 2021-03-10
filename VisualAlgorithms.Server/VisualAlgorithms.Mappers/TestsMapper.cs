using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Models.Tests;

namespace VisualAlgorithms.Mappers
{
    public class TestsMapper
    {
        public Test ToDomain(TestEntity testEntity, IEnumerable<TestQuestion> testQuestions)
        {
            if (testEntity == null)
                return null;

            return new Test
            {
                Id = testEntity.Id,
                Name = testEntity.Name,
                AlgorithmId = testEntity.AlgorithmId,
                TestQuestions = testQuestions ?? new List<TestQuestion>()
            };
        }

        public Test ToDomain(TestModel testModel)
        {
            return new Test
            {
                Id = testModel.Id,
                Name = testModel.Name,
                AlgorithmId = testModel.AlgorithmId,
                TestQuestions = new List<TestQuestion>()
            };
        }

        public IEnumerable<Test> ToDomainCollection(IEnumerable<TestEntity> testEntities, IEnumerable<TestQuestion> testQuestions)
        {
            foreach (var entity in testEntities)
            {
                var currentTestQuestions = testQuestions.Where(q => q.TestId == entity.Id);
                yield return ToDomain(entity, currentTestQuestions);
            }
        }

        public TestEntity ToEntity(Test test)
        {
            return new TestEntity
            {
                Id = test.Id,
                Name = test.Name,
                AlgorithmId = test.AlgorithmId
            };
        }

        public TestModel ToModel(Test test)
        {
            return new TestModel
            {
                Id = test.Id,
                Name = test.Name,
                AlgorithmId = test.AlgorithmId
            };
        }
    }
}
