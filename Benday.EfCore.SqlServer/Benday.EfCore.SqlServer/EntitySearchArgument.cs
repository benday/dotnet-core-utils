using System;

namespace Benday.EfCore.SqlServer
{
    public class EntitySearchArgument
    {
        public EntitySearchArgument(string propertyName, SearchMethod method, string searchValue)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Method = method;
            SearchValue = searchValue ?? throw new ArgumentNullException(nameof(searchValue));
        }

        public string PropertyName { get; set; }
        public SearchMethod Method { get; set; }
        public string SearchValue { get; set; }
    }
}
