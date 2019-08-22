using System.Collections.Generic;

namespace Benday.Common
{
    public class Search
    {
        public Search()
        {
            Arguments = new List<SearchArgument>();
            MaxNumberOfResults = -1;
        }

        public List<SearchArgument> Arguments { get; }

        public void AddArgument(
            string propertyName,
            SearchMethod method,
            string value)
        {
            Arguments.Add(
                new SearchArgument(propertyName, method, value));
        }

        public int MaxNumberOfResults { get; set; }
    }
}
