﻿namespace Algorithmix.Models.Algorithms
{
    public class AlgorithmQuery
    {
        public AlgorithmQuery(string searchText, int pageIndex, int pageSize, AlgorithmSortBy sortBy, bool sortByDesc)
        {
            SearchText = searchText;
            PageSize = pageSize;
            PageIndex = pageIndex;
            SortBy = sortBy;
            SortByDesc = sortByDesc;
        }

        public string SearchText { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public AlgorithmSortBy SortBy { get; set; }
        public bool SortByDesc { get; set; }
    }

    public enum AlgorithmSortBy
    {
        None,
        Id,
        Name,
        HasDescription,
        HasConstructor,
        TestsCount
    }
}
