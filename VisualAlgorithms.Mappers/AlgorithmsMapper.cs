using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Mappers
{
    public class AlgorithmsMapper
    {
        public Algorithm ToDomain(AlgorithmEntity algorithmEntity)
        {
            return new Algorithm
            {
                Id = algorithmEntity.Id,
                Tag = algorithmEntity.Tag,
                Name = algorithmEntity.Name,
                ImageUrl = algorithmEntity.ImageUrl,
                AlgorithmTimeComplexityId = algorithmEntity.AlgorithmTimeComplexityId,
                AlgorithmTimeComplexity = new AlgorithmTimeComplexity(),
                Tests = new List<Test>()
            };
        }

        public IEnumerable<Algorithm> ToDomainCollection(IEnumerable<AlgorithmEntity> algorithmEntities)
        {
            return algorithmEntities.Select(entity => ToDomain(entity));
        }

        public AlgorithmEntity ToEntity(Algorithm algorithm)
        {
            return new AlgorithmEntity
            {
                Id = algorithm.Id,
                Tag = algorithm.Tag,
                Name = algorithm.Name,
                ImageUrl = algorithm.ImageUrl,
                AlgorithmTimeComplexityId = algorithm.AlgorithmTimeComplexityId
            };
        }
    }
}
