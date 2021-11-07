using Algorithmix.Entities;
using Algorithmix.Models.Algorithms;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class AlgorithmMapper
    {
        public AlgorithmEntity ToEntity(AlgorithmPayload algorithmPayload)
        {
            return new AlgorithmEntity
            {
                Id = algorithmPayload.Id.ToLower(),
                Name = algorithmPayload.Name,
                ImageUrl = algorithmPayload.ImageUrl,
                TimeComplexityId = algorithmPayload.TimeComplexityId
            };
        }

        public AlgorithmEntity ToEntity(Algorithm algorithm)
        {
            return new AlgorithmEntity
            {
                Id = algorithm.Id,
                Name = algorithm.Name,
                ImageUrl = algorithm.ImageUrl,
                TimeComplexityId = algorithm.TimeComplexityId
            };
        }

        public Algorithm ToModel(AlgorithmEntity algorithmEntity)
        {
            return new Algorithm
            {
                Id = algorithmEntity.Id,
                Name = algorithmEntity.Name,
                ImageUrl = algorithmEntity.ImageUrl,
                TimeComplexityId = algorithmEntity.TimeComplexityId,
                TimeComplexity = new AlgorithmTimeComplexity(),
                Tests = new List<Test>()
            };
        }

        public IEnumerable<Algorithm> ToModelsCollection(IEnumerable<AlgorithmEntity> algorithmEntities)
        {
            return algorithmEntities.Select(entity => ToModel(entity));
        }
    }
}
