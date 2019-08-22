using Benday.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Benday.EfCore.SqlServer
{
    public abstract class SqlEntityFrameworkSearchableRepositoryBase<TEntity, TDbContext> :
        SqlEntityFrameworkCrudRepositoryBase<TEntity, TDbContext>, ISearchableRepository<TEntity>
        where TEntity : class, IInt32Identity
        where TDbContext : DbContext
    {
        public SqlEntityFrameworkSearchableRepositoryBase(
            TDbContext context) : base(context)
        {

        }

        public virtual IList<TEntity> Search(Search search)
        {
            if (search == null || search.Arguments.Count == 0)
            {
                return new List<TEntity>();
            }
            else
            {
                var query = EntityDbSet.AsQueryable<TEntity>();

                foreach (var arg in search.Arguments)
                {
                    if (arg.Method == SearchMethod.Contains)
                    {
                        query = AddWhereClauseForContains(query, arg);
                    }
                    else if (arg.Method == SearchMethod.StartsWith)
                    {
                        query = AddWhereClauseForStartsWith(query, arg);
                    }
                    else if (arg.Method == SearchMethod.EndsWith)
                    {
                        query = AddWhereClauseForEndsWith(query, arg);
                    }
                    else if (arg.Method == SearchMethod.Exact)
                    {
                        query = AddWhereClauseForExact(query, arg);
                    }
                    else if (arg.Method == SearchMethod.IsNot)
                    {
                        query = AddWhereClauseForIsNotEqualTo(query, arg);
                    }
                    else if (arg.Method == SearchMethod.DoesNotContain)
                    {
                        query = AddWhereClauseForDoesNotContain(query, arg);
                    }
                }

                if (search.MaxNumberOfResults == -1)
                {
                    return query.ToList();
                }
                else
                {
                    return query.Take(search.MaxNumberOfResults).ToList();
                }
            }
        }

        protected abstract IQueryable<TEntity> AddWhereClauseForDoesNotContain(
            IQueryable<TEntity> query, SearchArgument arg);
        protected abstract IQueryable<TEntity> AddWhereClauseForIsNotEqualTo(
            IQueryable<TEntity> query, SearchArgument arg);
        protected abstract IQueryable<TEntity> AddWhereClauseForExact(
            IQueryable<TEntity> query, SearchArgument arg);
        protected abstract IQueryable<TEntity> AddWhereClauseForEndsWith(
            IQueryable<TEntity> query, SearchArgument arg);
        protected abstract IQueryable<TEntity> AddWhereClauseForStartsWith(
            IQueryable<TEntity> query, SearchArgument arg);
        protected abstract IQueryable<TEntity> AddWhereClauseForContains(
            IQueryable<TEntity> query, SearchArgument arg);
    }
}
