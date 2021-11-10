namespace Algorithmix.Models.Algorithms
{
    public class AlgorithmTimeComplexityPayload
    {
        public int Id { get; set; }

        public string SortingBestTime { get; set; }
        public string SortingAverageTime { get; set; }
        public string SortingWorstTime { get; set; }

        public string SearchingBestTime { get; set; }
        public string SearchingAverageTime { get; set; }
        public string SearchingWorstTime { get; set; }

        public string InsertionBestTime { get; set; }
        public string InsertionAverageTime { get; set; }
        public string InsertionWorstTime { get; set; }

        public string DeletionBestTime { get; set; }
        public string DeletionAverageTime { get; set; }
        public string DeletionWorstTime { get; set; }

        public string IndexingBestTime { get; set; }
        public string IndexingAverageTime { get; set; }
        public string IndexingWorstTime { get; set; }

        public string FindMaxElementBestTime { get; set; }
        public string FindMaxElementAverageTime { get; set; }
        public string FindMaxElementWorstTime { get; set; }

        public string GetMaxElementBestTime { get; set; }
        public string GetMaxElementAverageTime { get; set; }
        public string GetMaxElementWorstTime { get; set; }

        public string AlgorithmId { get; set; }
    }
}
