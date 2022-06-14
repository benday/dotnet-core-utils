using Microsoft.EntityFrameworkCore;

namespace Benday.EfCore.SqlServer
{
    public interface IDependentEntityCollection
    {
        void AfterSave();
        void BeforeSave(DbContext dbContext);
    }
}
