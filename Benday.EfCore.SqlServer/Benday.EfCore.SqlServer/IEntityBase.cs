using System.Collections.Generic;
using Benday.Common;

namespace Benday.EfCore.SqlServer
{
    public interface IEntityBase : IInt32Identity, IDeleteable
    {
        IList<IDependentEntityCollection> GetDependentEntities();
    }
}
