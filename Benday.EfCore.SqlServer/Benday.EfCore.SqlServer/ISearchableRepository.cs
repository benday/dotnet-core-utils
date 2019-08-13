using System.Collections.Generic;

namespace Benday.EfCore.SqlServer
{
    public interface ISearchableRepository<T> : IRepository<T> 
        where T : IInt32Identity
    {
        IList<T> Search(EntitySearch search);
    }
}
