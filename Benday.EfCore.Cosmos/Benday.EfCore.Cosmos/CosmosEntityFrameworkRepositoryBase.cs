using Benday.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Benday.EfCore.Cosmos
{
    public abstract class CosmosEntityFrameworkRepositoryBase<TEntity, TDbContext> :
        IDisposable where TEntity : class, IStringIdentity
        where TDbContext : DbContext
    {
        public CosmosEntityFrameworkRepositoryBase(
            TDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "context is null.");

            _Context = context;
        }

        public void Dispose()
        {
            ((IDisposable)_Context).Dispose();
        }

        private TDbContext _Context;

        protected TDbContext Context
        {
            get
            {
                return _Context;
            }
        }

        protected void VerifyItemIsAddedOrAttachedToDbSet(DbSet<TEntity> dbset, TEntity item)
        {
            if (item == null)
            {
                return;
            }
            else
            {
                var entry = _Context.Entry<TEntity>(item);

                if (entry.State == EntityState.Detached)
                {
                    dbset.Attach(item);
                }

                entry.State = EntityState.Modified;
            }
        }
    }
}
