using Benday.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.EfCore.Cosmos
{
    public abstract class SqlEntityFrameworkCrudRepositoryBase<TEntity, TDbContext> :
        SqlEntityFrameworkRepositoryBase<TEntity, TDbContext>, IRepository<TEntity>
        where TEntity : class, IInt32Identity
        where TDbContext : DbContext
    {
        public SqlEntityFrameworkCrudRepositoryBase(
            TDbContext context) : base(context)
        {

        }

        protected abstract DbSet<TEntity> EntityDbSet
        {
            get;
        }

        public virtual void Delete(TEntity deleteThis)
        {
            if (deleteThis == null)
                throw new ArgumentNullException("deleteThis", "deleteThis is null.");

            var entry = Context.Entry(deleteThis);

            if (entry.State == EntityState.Detached)
            {
                EntityDbSet.Attach(deleteThis);
            }

            EntityDbSet.Remove(deleteThis);

            BeforeDelete(deleteThis);

            Context.SaveChanges();

            AfterDelete(deleteThis);
        }

        protected virtual void BeforeDelete(TEntity deleteThis)
        {
            
        }

        protected virtual void AfterDelete(TEntity deleteThis)
        {

        }

        protected virtual List<string> Includes
        {
            get;
        }

        public virtual IList<TEntity> GetAll()
        {
            var queryable = EntityDbSet.AsQueryable();

            queryable = AddIncludes(queryable);

            queryable = BeforeGetAll(queryable);
            
            return queryable.ToList();
        }

        protected virtual IQueryable<TEntity> BeforeGetAll(IQueryable<TEntity> query)
        {
            return query;
        }

        public virtual IList<TEntity> GetAll(int maxNumberOfRows)
        {
            var queryable = EntityDbSet.AsQueryable();

            queryable = AddIncludes(queryable);

            return queryable.Take(maxNumberOfRows).ToList();
        }        

        protected virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (Includes == null || Includes.Count == 0)
            {
                return queryable;
            }
            else
            {
                foreach (var item in Includes)
                {
                    queryable = queryable.Include(item);
                }

                return queryable;
            }
        }

        public virtual TEntity GetById(int id)
        {
            var query = from temp in EntityDbSet
                        where temp.Id == id
                        select temp;

            query = AddIncludes(query);

            query = BeforeGetById(query, id);

            return query.FirstOrDefault();
        }

        private IQueryable<TEntity> BeforeGetById(IQueryable<TEntity> query, int id)
        {
            return query;
        }

        public virtual void Save(TEntity saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            VerifyItemIsAddedOrAttachedToDbSet(
                EntityDbSet, saveThis);

            BeforeSave(saveThis);

            Context.SaveChanges();

            AfterSave(saveThis);
        }

        protected virtual void BeforeSave(TEntity saveThis)
        {

        }

        protected virtual void AfterSave(TEntity saveThis)
        {

        }
    }
}
