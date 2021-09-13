namespace Algorithmix.Models.Algorithms
{
    public class AlgorithmQuery
    {
        public AlgorithmQuery(string searchText)
        {
            SearchText = searchText;
        }

        public string SearchText { get; set; }
    }
}
