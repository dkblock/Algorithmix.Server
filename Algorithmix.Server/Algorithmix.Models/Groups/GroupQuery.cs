namespace Algorithmix.Models.Groups
{
    public class GroupQuery
    {
        public GroupQuery(string searchText, int pageIndex, int pageSize, GroupSortBy sortBy, bool sortByDesc)
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
        public GroupSortBy SortBy { get; set; }
        public bool SortByDesc { get; set; }
    }

    public enum GroupSortBy
    {
        Id,
        Name,
        UsersCount,
        IsAvailableForRegister
    }
}
