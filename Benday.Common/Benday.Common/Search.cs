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
            string value,
            SearchOperator combineWithOtherArgumentsAs = SearchOperator.And)
        {
            Arguments.Add(
                new SearchArgument(propertyName, method, value, combineWithOtherArgumentsAs));
        }

        public int MaxNumberOfResults { get; set; }
    }
}
