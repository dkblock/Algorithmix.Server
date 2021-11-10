using Algorithmix.Entities;
using Algorithmix.Models.Algorithms;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class AlgorithmTimeComplexityMapper
    {
        public AlgorithmTimeComplexityEntity ToEntity(AlgorithmTimeComplexityPayload timeComplexityPayload, int? id = null)
        {
            return new AlgorithmTimeComplexityEntity
            {
                Id = id ?? 0,
                AlgorithmId = timeComplexityPayload.AlgorithmId,

                DeletionAverageTime = timeComplexityPayload.DeletionAverageTime,
                DeletionBestTime = timeComplexityPayload.DeletionBestTime,
                DeletionWorstTime = timeComplexityPayload.DeletionWorstTime,

                InsertionAverageTime = timeComplexityPayload.InsertionAverageTime,
                InsertionBestTime = timeComplexityPayload.InsertionBestTime,
                InsertionWorstTime = timeComplexityPayload.InsertionWorstTime,

                SearchingAverageTime = timeComplexityPayload.SearchingAverageTime,
                SearchingBestTime = timeComplexityPayload.SearchingBestTime,
                SearchingWorstTime = timeComplexityPayload.SearchingWorstTime,

                IndexingAverageTime = timeComplexityPayload.IndexingAverageTime,
                IndexingBestTime = timeComplexityPayload.IndexingBestTime,
                IndexingWorstTime = timeComplexityPayload.IndexingWorstTime,

                FindMaxElementAverageTime = timeComplexityPayload.FindMaxElementAverageTime,
                FindMaxElementBestTime = timeComplexityPayload.FindMaxElementBestTime,
                FindMaxElementWorstTime = timeComplexityPayload.FindMaxElementWorstTime,

                GetMaxElementAverageTime = timeComplexityPayload.GetMaxElementAverageTime,
                GetMaxElementBestTime = timeComplexityPayload.GetMaxElementBestTime,
                GetMaxElementWorstTime = timeComplexityPayload.GetMaxElementWorstTime,

                SortingAverageTime = timeComplexityPayload.SortingAverageTime,
                SortingBestTime = timeComplexityPayload.SortingBestTime,
                SortingWorstTime = timeComplexityPayload.SortingWorstTime
            };
        }

        public AlgorithmTimeComplexityEntity ToEntity(AlgorithmTimeComplexity timeComplexity)
        {
            return new AlgorithmTimeComplexityEntity
            {
                Id = timeComplexity.Id,
                AlgorithmId = timeComplexity.AlgorithmId,

                DeletionAverageTime = timeComplexity.DeletionAverageTime,
                DeletionBestTime = timeComplexity.DeletionBestTime,
                DeletionWorstTime = timeComplexity.DeletionWorstTime,

                InsertionAverageTime = timeComplexity.InsertionAverageTime,
                InsertionBestTime = timeComplexity.InsertionBestTime,
                InsertionWorstTime = timeComplexity.InsertionWorstTime,

                SearchingAverageTime = timeComplexity.SearchingAverageTime,
                SearchingBestTime = timeComplexity.SearchingBestTime,
                SearchingWorstTime = timeComplexity.SearchingWorstTime,

                IndexingAverageTime = timeComplexity.IndexingAverageTime,
                IndexingBestTime = timeComplexity.IndexingBestTime,
                IndexingWorstTime = timeComplexity.IndexingWorstTime,

                FindMaxElementAverageTime = timeComplexity.FindMaxElementAverageTime,
                FindMaxElementBestTime = timeComplexity.FindMaxElementBestTime,
                FindMaxElementWorstTime = timeComplexity.FindMaxElementWorstTime,

                GetMaxElementAverageTime = timeComplexity.GetMaxElementAverageTime,
                GetMaxElementBestTime = timeComplexity.GetMaxElementBestTime,
                GetMaxElementWorstTime = timeComplexity.GetMaxElementWorstTime,

                SortingAverageTime = timeComplexity.SortingAverageTime,
                SortingBestTime = timeComplexity.SortingBestTime,
                SortingWorstTime = timeComplexity.SortingWorstTime
            };
        }

        public AlgorithmTimeComplexity ToModel(AlgorithmTimeComplexityEntity timeComplexityEntity)
        {
            if (timeComplexityEntity == null)
                return null;

            return new AlgorithmTimeComplexity
            {
                Id = timeComplexityEntity.Id,
                AlgorithmId = timeComplexityEntity.AlgorithmId,

                DeletionAverageTime = timeComplexityEntity.DeletionAverageTime,
                DeletionBestTime = timeComplexityEntity.DeletionBestTime,
                DeletionWorstTime = timeComplexityEntity.DeletionWorstTime,

                InsertionAverageTime = timeComplexityEntity.InsertionAverageTime,
                InsertionBestTime = timeComplexityEntity.InsertionBestTime,
                InsertionWorstTime = timeComplexityEntity.InsertionWorstTime,

                SearchingAverageTime = timeComplexityEntity.SearchingAverageTime,
                SearchingBestTime = timeComplexityEntity.SearchingBestTime,
                SearchingWorstTime = timeComplexityEntity.SearchingWorstTime,

                IndexingAverageTime = timeComplexityEntity.IndexingAverageTime,
                IndexingBestTime = timeComplexityEntity.IndexingBestTime,
                IndexingWorstTime = timeComplexityEntity.IndexingWorstTime,

                FindMaxElementAverageTime = timeComplexityEntity.FindMaxElementAverageTime,
                FindMaxElementBestTime = timeComplexityEntity.FindMaxElementBestTime,
                FindMaxElementWorstTime = timeComplexityEntity.FindMaxElementWorstTime,

                GetMaxElementAverageTime = timeComplexityEntity.GetMaxElementAverageTime,
                GetMaxElementBestTime = timeComplexityEntity.GetMaxElementBestTime,
                GetMaxElementWorstTime = timeComplexityEntity.GetMaxElementWorstTime,

                SortingAverageTime = timeComplexityEntity.SortingAverageTime,
                SortingBestTime = timeComplexityEntity.SortingBestTime,
                SortingWorstTime = timeComplexityEntity.SortingWorstTime
            };
        }

        public IEnumerable<AlgorithmTimeComplexity> ToModelsCollection(IEnumerable<AlgorithmTimeComplexityEntity> timeComplexityEntities)
        {
            return timeComplexityEntities.Select(entity => ToModel(entity));
        }        
    }
}
