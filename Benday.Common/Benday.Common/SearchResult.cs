using System.Collections.Generic;

namespace Benday.Common
{
    public class SearchResult<T>
    {
        public virtual IList<T> Results { get; set; }
        public virtual Search SearchRequest { get; set; }
    }
}
