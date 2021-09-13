using Algorithmix.Entities.Test;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class TestAlgorithmMapper
    {
        public TestAlgorithmEntity ToEntity(int testId, string algorithmId)
        {
            return new TestAlgorithmEntity
            {
                TestId = testId,
                AlgorithmId = algorithmId
            };
        }

        public TestAlgorithm ToModel(TestAlgorithmEntity entity)
        {
            return new TestAlgorithm
            {
                Id = entity.Id,
                TestId = entity.TestId,
                AlgorithmId = entity.AlgorithmId
            };
        }

        public IEnumerable<TestAlgorithm> ToModelsCollection(IEnumerable<TestAlgorithmEntity> testAlgorithmEntities)
        {
            return testAlgorithmEntities.Select(entity => ToModel(entity));
        }
    }
}
