namespace Algorithmix.Models.Tests
{
    public class TestQuery
    {
        public TestQuery(string searchText, bool onlyAccessible, int pageSize, int pageIndex, TestSortBy sortBy, bool sortByDesc)
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
        public TestSortBy SortBy { get; set; }
        public bool SortByDesc { get; set; }
    }

    public enum TestSortBy
    {
        Name,
        Status,
        IsPublished,
        AverageResult,
        CreatedBy,
        CreatedDate,
        UpdatedDate,
    }
}
