using System.Collections.Generic;
using Benday.Common;

namespace Benday.EfCore.SqlServer
{
    public interface ISearchableRepository<T> : IRepository<T> 
        where T : IInt32Identity
    {
        IList<T> Search(Search search);
    }
}
