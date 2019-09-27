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
            if (search == null)
            {
                return new List<TEntity>();
            }
            else
            {
                Expression<Func<TEntity, bool>> whereClausePredicate = null;
                whereClausePredicate = GetWhereClause(search);

                IQueryable<TEntity> query = null;

                if (whereClausePredicate == null)
                {
                    query = EntityDbSet.AsQueryable();
                }
                else
                {
                    query = EntityDbSet.Where(whereClausePredicate);
                }

                query = AddIncludes(query);

                query = AddSorts(search, query);

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

        protected virtual IQueryable<TEntity> AddSorts(Search search, IQueryable<TEntity> query)
        {
            return query;
        }

        private Expression<Func<TEntity, bool>> GetWhereClause(Search search)
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
                else if (arg.Method == SearchMethod.Equals)
                {
                    predicate = GetPredicateForEquals(arg);
                }
                else if (arg.Method == SearchMethod.IsNotEqual)
                {
                    predicate = GetPredicateForIsNotEqualTo(arg);
                }
                else if (arg.Method == SearchMethod.DoesNotContain)
                {
                    predicate = GetPredicateForDoesNotContain(arg);
                }

                if (predicate == null)
                {
                    // if predicate is null, the implementer chose to ignore this 
                    // search argument and returned null as an indication to skip
                    continue;
                }
                else if (whereClausePredicate == null)
                {
                    whereClausePredicate = predicate;
                }
                else if (arg.Operator == SearchOperator.Or)
                {
                    whereClausePredicate = whereClausePredicate.Or(predicate);
                }
                else if (arg.Operator == SearchOperator.And)
                {
                    whereClausePredicate = whereClausePredicate.And(predicate);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Search operator '{0}' is not supported.", arg.Operator));
                }
            }

            return whereClausePredicate;
        }

        protected virtual IQueryable<TEntity> BeforeSearch(IQueryable<TEntity> query, Search search)
        {
            return query;
        }

        protected abstract Expression<Func<TEntity, bool>> GetPredicateForDoesNotContain(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForIsNotEqualTo(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForEquals(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForEndsWith(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForStartsWith(
            SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForContains(
            SearchArgument arg);      
    }
}
