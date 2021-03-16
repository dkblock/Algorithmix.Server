using System.Collections.Generic;
using System.Linq;
using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Mappers
{
    public class AlgorithmsMapper
    {
        public Algorithm ToDomain(AlgorithmEntity algorithmEntity, AlgorithmTimeComplexity timeComplexity, IEnumerable<Test> tests)
        {
            if (algorithmEntity == null)
                return null;

            return new Algorithm
            {
                Id = algorithmEntity.Id,
                Name = algorithmEntity.Name,
                ImageUrl = algorithmEntity.ImageUrl,
                AlgorithmTimeComplexityId = algorithmEntity.AlgorithmTimeComplexityId,
                AlgorithmTimeComplexity = timeComplexity ?? new AlgorithmTimeComplexity(),
                Tests = tests ?? new List<Test>()
            };
        }

        public IEnumerable<Algorithm> ToDomainCollection(
            IEnumerable<AlgorithmEntity> algorithmEntities,
            IEnumerable<AlgorithmTimeComplexity> timeComplexities,
            IEnumerable<Test> tests)
        {
            foreach (var entity in algorithmEntities)
            {
                var currentAlgorithmTimeComplexity = timeComplexities.SingleOrDefault(tc => tc.Id == entity.AlgorithmTimeComplexityId);
                var currentAlgorithmTests = tests.Where(t => t.AlgorithmId == entity.Id);
                yield return ToDomain(entity, currentAlgorithmTimeComplexity, currentAlgorithmTests);
            }
        }

        public AlgorithmEntity ToEntity(Algorithm algorithm)
        {
            return new AlgorithmEntity
            {
                Id = algorithm.Id,
                Name = algorithm.Name,
                ImageUrl = algorithm.ImageUrl,
                AlgorithmTimeComplexityId = algorithm.AlgorithmTimeComplexityId
            };
        }
    }
}
