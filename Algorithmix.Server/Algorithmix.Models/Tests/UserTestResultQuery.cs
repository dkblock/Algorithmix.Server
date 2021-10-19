namespace Algorithmix.Models.Tests
{
    public class UserTestResultQuery
    {
        public UserTestResultQuery(string searchText, int groupId, string sortBy, bool desc)
        {
            SearchText = searchText;
            GroupId = groupId;
            SortBy = sortBy;
            Desc = desc;
        }

        public string SearchText { get; set; }
        public int GroupId { get; set; }
        public string SortBy { get; set; }
        public bool Desc { get; set; }
    }
}
