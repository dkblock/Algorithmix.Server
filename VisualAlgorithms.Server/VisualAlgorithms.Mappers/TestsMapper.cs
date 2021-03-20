using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Models.Tests;

namespace VisualAlgorithms.Mappers
{
    public class TestsMapper
    {
        public TestEntity ToEntity(TestPayload testPayload, int? id = null)
        {
            return new TestEntity
            {
                Id = id ?? 0,
                Name = testPayload.Name,
                AlgorithmId = testPayload.AlgorithmId
            };
        }

        public Test ToModel(TestEntity testEntity, IEnumerable<TestQuestion> testQuestions = null)
        {
            if (testEntity == null)
                return null;

            return new Test
            {
                Id = testEntity.Id,
                Name = testEntity.Name,
                AlgorithmId = testEntity.AlgorithmId,
                Questions = testQuestions ?? new List<TestQuestion>()
            };
        }

        public IEnumerable<Test> ToModelsCollection(IEnumerable<TestEntity> testEntities, IEnumerable<TestQuestion> testQuestions)
        {
            foreach (var entity in testEntities)
            {
                var currentTestQuestions = testQuestions.Where(q => q.TestId == entity.Id);
                yield return ToModel(entity, currentTestQuestions);
            }
        }
    }
}
