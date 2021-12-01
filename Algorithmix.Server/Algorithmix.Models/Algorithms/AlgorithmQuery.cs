namespace Algorithmix.Models.Algorithms
{
    public class AlgorithmQuery
    {
        public AlgorithmQuery(string searchText, bool onlyAccessible, int pageIndex, int pageSize, AlgorithmSortBy sortBy, bool sortByDesc)
        {
            SearchText = searchText;
            OnlyAccessible = onlyAccessible;
            PageSize = pageSize;
            PageIndex = pageIndex;
            SortBy = sortBy;
            SortByDesc = sortByDesc;
        }

        public string SearchText { get; set; }
        public bool OnlyAccessible { get; set; }
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
        CreatedBy,
        HasDescription,
        HasConstructor,
        TestsCount
    }
}
