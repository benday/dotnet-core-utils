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
                Expression<Func<TEntity, bool>> whereClausePredicate = null;
                Expression<Func<TEntity, bool>> predicate = null;

                foreach (var arg in search.Arguments)
                {
                    if (arg.Method == SearchMethod.Contains)
                    {
                        predicate = GetPredicateForContains(arg);
                    }
                    else if (arg.Method == SearchMethod.StartsWith)
                    {
                        predicate = GetPredicateForStartsWith(arg);
                    }
                    else if (arg.Method == SearchMethod.EndsWith)
                    {
                        predicate = GetPredicateForEndsWith(arg);
                    }
                    else if (arg.Method == SearchMethod.Exact)
                    {
                        predicate = GetPredicateForExact(arg);
                    }
                    else if (arg.Method == SearchMethod.IsNot)
                    {
                        predicate = GetPredicateForIsNotEqualTo(arg);
                    }
                    else if (arg.Method == SearchMethod.DoesNotContain)
                    {
                        predicate = GetPredicateForDoesNotContain(arg);
                    }

                    if (whereClausePredicate == null)
                    {
                        whereClausePredicate = predicate;
                    }
                    if (arg.CombineWithOtherArgumentsAs == SearchOperator.Or)
                    {
                        whereClausePredicate = whereClausePredicate.Or(predicate);
                    }
                    else
                    {
                        whereClausePredicate = whereClausePredicate.Or(predicate);
                    }
                }

                var query = EntityDbSet.Where(whereClausePredicate);
                
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

        protected abstract Expression<Func<TEntity, bool>> GetPredicateForDoesNotContain(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForIsNotEqualTo(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForExact(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForEndsWith(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForStartsWith(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForContains(
            SearchArgument arg);
    }
}
