using Algorithmix.Entities.Test;
using Algorithmix.Models.Algorithms;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class TestMapper
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

        public PublishedTestEntity ToEntity(Test test)
        {
            return new PublishedTestEntity
            {
                Id = test.Id,
                Name = test.Name,
                AlgorithmId = test.Algorithm.Id
            };
        }

        public Test ToModel(BaseTestEntity testEntity)
        {
            if (testEntity == null)
                return null;

            return new Test
            {
                Id = testEntity.Id,
                Name = testEntity.Name,
                Algorithm = new Algorithm() { Id = testEntity.AlgorithmId },
                Questions = new List<TestQuestion>()
            };
        }

        public IEnumerable<Test> ToModelsCollection(IEnumerable<BaseTestEntity> testEntities)
        {
            return testEntities.Select(entity => ToModel(entity));
        }
    }
}
