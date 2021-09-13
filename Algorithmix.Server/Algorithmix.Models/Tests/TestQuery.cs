namespace Algorithmix.Models.Tests
{
    public class TestQuery
    {
        public TestQuery(string searchText)
        {
            SearchText = searchText;
        }

        public string SearchText { get; set; }
    }
}
