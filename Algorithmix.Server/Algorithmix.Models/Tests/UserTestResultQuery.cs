namespace Algorithmix.Models.Tests
{
    public class UserTestResultQuery
    {
        public UserTestResultQuery(string searchText, int groupId, int pageIndex, int pageSize, UserTestResultSortBy sortBy, bool sortByDesc)
        {
            SearchText = searchText;
            GroupId = groupId;
            PageIndex = pageIndex;
            PageSize = pageSize;
            SortBy = sortBy;
            SortByDesc = sortByDesc;
        }

        public string SearchText { get; set; }
        public int GroupId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public UserTestResultSortBy SortBy { get; set; }
        public bool SortByDesc { get; set; }        
    }

    public enum UserTestResultSortBy
    {
        FullName,
        GroupName,
        TestName,
        Result,
        PassingTime,
    }
}
