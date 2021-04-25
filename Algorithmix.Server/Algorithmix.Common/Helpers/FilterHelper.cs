namespace Algorithmix.Common.Helpers
{
    public class FilterHelper
    {
        public bool IsMatch(string searchText, string value)
        {
            return value.ToLower().Contains(searchText.ToLower());
        }
    }
}
