using System;

namespace Benday.Common
{
    public class SearchArgument
    {
        public SearchArgument(
            string propertyName,
            SearchMethod method,
            string searchValue,
            SearchOperator addAsOperator = SearchOperator.And)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Method = method;
            SearchValue = searchValue ?? throw new ArgumentNullException(nameof(searchValue));
            Operator = addAsOperator;
        }

        public SearchArgument(
            string propertyName,
            SearchMethod method,
            int searchValue,
            SearchOperator addAsOperator = SearchOperator.And)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Method = method;
            SearchValue = searchValue.ToString();
            SearchValueAsInt = searchValue;
            Operator = addAsOperator;
        }

        public string PropertyName { get; set; }
        public SearchMethod Method { get; set; }
        public string SearchValue { get; set; }
        public int SearchValueAsInt { get; set; }
        public SearchOperator Operator { get; set; }
    }
}
