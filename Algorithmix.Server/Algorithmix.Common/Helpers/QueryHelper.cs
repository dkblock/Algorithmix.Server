namespace Algorithmix.Common.Helpers
{
    public class QueryHelper
    {
        public bool IsMatch(string searchText, string[] filters)
        {
            if (string.IsNullOrEmpty(searchText))
                return true;

            foreach (var filter in filters)
            {
                if (filter.ToLower().Contains(searchText.ToLower()))
                    return true;
            }

            return false;
        }
    }
}
