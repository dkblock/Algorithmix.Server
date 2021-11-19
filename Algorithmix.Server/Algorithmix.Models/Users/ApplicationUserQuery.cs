namespace Algorithmix.Models.Users
{
    public class ApplicationUserQuery
    {
        public ApplicationUserQuery(string searchText, int groupId, string role, int pageIndex, int pageSize, ApplicationUserSortBy sortBy, bool sortByDesc)
        {
            SearchText = searchText;
            GroupId = groupId;
            Role = role;
            PageSize = pageSize;
            PageIndex = pageIndex;
            SortBy = sortBy;
            SortByDesc = sortByDesc;
        }

        public string SearchText { get; set; }
        public int GroupId { get; set; }
        public string Role { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public ApplicationUserSortBy SortBy { get; set; }
        public bool SortByDesc { get; set; }
    }

    public enum ApplicationUserSortBy
    {
        FullName,
        Email,
        GroupId,
        Role
    }
}
