using Benday.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
                Expression<Func<TEntity, bool>> predicate = c => true;

                foreach (var arg in search.Arguments)
                {
                    if (arg.Method == SearchMethod.Contains)
                    {
                        predicate = AddWhereClauseForContains(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.StartsWith)
                    {
                        predicate = AddWhereClauseForStartsWith(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.EndsWith)
                    {
                        predicate = AddWhereClauseForEndsWith(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.Exact)
                    {
                        predicate = AddWhereClauseForExact(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.IsNot)
                    {
                        predicate = AddWhereClauseForIsNotEqualTo(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.DoesNotContain)
                    {
                        predicate = AddWhereClauseForDoesNotContain(predicate, arg);
                    }
                }

                var query = EntityDbSet.Where(predicate);
                
                query = BeforeSearch(query, search);

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

        protected virtual IQueryable<TEntity> BeforeSearch(IQueryable<TEntity> query, Search search)
        {
            return query;
        }

        protected abstract Expression<Func<TEntity, bool>> AddWhereClauseForDoesNotContain(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> AddWhereClauseForIsNotEqualTo(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> AddWhereClauseForExact(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> AddWhereClauseForEndsWith(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> AddWhereClauseForStartsWith(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> AddWhereClauseForContains(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
    }
}
