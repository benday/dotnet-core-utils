using System.Collections.Generic;

namespace Benday.EfCore.SqlServer
{
    public class EntitySearch
    {
        public EntitySearch()
        {
            Arguments = new List<EntitySearchArgument>();
            MaxNumberOfRows = -1;
        }

        public List<EntitySearchArgument> Arguments { get; }

        public void AddArgument(
            string propertyName,
            SearchMethod method,
            string value)
        {
            Arguments.Add(
                new EntitySearchArgument(propertyName, method, value));
        }

        public int MaxNumberOfRows { get; set; }
    }
}
