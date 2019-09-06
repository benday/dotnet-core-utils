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
                        predicate = GetPredicateForContains(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.StartsWith)
                    {
                        predicate = GetPredicateForStartsWith(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.EndsWith)
                    {
                        predicate = GetPredicateForEndsWith(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.Exact)
                    {
                        predicate = GetPredicateForExact(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.IsNot)
                    {
                        predicate = GetPredicateForIsNotEqualTo(predicate, arg);
                    }
                    else if (arg.Method == SearchMethod.DoesNotContain)
                    {
                        predicate = GetPredicateForDoesNotContain(predicate, arg);
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
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForIsNotEqualTo(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForExact(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForEndsWith(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForStartsWith(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
        protected abstract Expression<Func<TEntity, bool>> GetPredicateForContains(
            Expression<Func<TEntity, bool>> predicate, SearchArgument arg);
    }
}
