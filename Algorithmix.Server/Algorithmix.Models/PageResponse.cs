using System.Collections.Generic;

namespace Algorithmix.Models
{
    public class PageResponse<T> where T : class
    {
        public IEnumerable<T> Page { get; set; }
        public int TotalCount { get; set; }
    }
}
