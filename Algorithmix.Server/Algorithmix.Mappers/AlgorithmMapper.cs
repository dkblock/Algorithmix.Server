using Algorithmix.Entities;
using Algorithmix.Models.Algorithms;
using Algorithmix.Models.Tests;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class AlgorithmMapper
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
                TimeComplexityId = algorithmEntity.TimeComplexityId,
                TimeComplexity = timeComplexity ?? new AlgorithmTimeComplexity(),
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
                var currentAlgorithmTimeComplexity = timeComplexities.SingleOrDefault(tc => tc.Id == entity.TimeComplexityId);
                var currentAlgorithmTests = tests.Where(t => t.Algorithm.Id == entity.Id);
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
                TimeComplexityId = algorithm.TimeComplexityId
            };
        }
    }
}
