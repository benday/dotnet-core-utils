﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Benday.Common;
using Microsoft.EntityFrameworkCore;

namespace Benday.EfCore.SqlServer
{
    public abstract class SqlEntityFrameworkSearchableRepositoryBase<TEntity, TDbContext> :
        SqlEntityFrameworkCrudRepositoryBase<TEntity, TDbContext>, ISearchableRepository<TEntity>
        where TEntity : class, IEntityBase
        where TDbContext : DbContext
    {
        public SqlEntityFrameworkSearchableRepositoryBase(
            TDbContext context) : base(context)
        {
        }

        public virtual SearchResult<TEntity> Search(Search search)
        {
            var returnValue = new SearchResult<TEntity>
            {
                SearchRequest = search
            };

            if (search == null)
            {
                returnValue.Results = new List<TEntity>();
            }
            else
            {
                var whereClausePredicate = GetWhereClause(search);

                IQueryable<TEntity> query;
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
                    returnValue.Results = query.ToList();
                }
                else
                {
                    returnValue.Results = query.Take(search.MaxNumberOfResults).ToList();
                }
            }

            return returnValue;
        }

        protected virtual IOrderedQueryable<TEntity> EnsureIsOrderedQueryable(IQueryable<TEntity> query)
        {
            if (query is IOrderedQueryable<TEntity>)
            {
                return query as IOrderedQueryable<TEntity>;
            }
            else
            {
                return query.OrderBy(x => 0);
            }
        }

        protected virtual IOrderedQueryable<TEntity> AddSort(IOrderedQueryable<TEntity> query, SortBy sort, bool isFirstSort)
        {
            if (sort.Direction == SearchConstants.SortDirectionAscending)
            {
                return AddSortAscending(query, sort.PropertyName, isFirstSort);
            }
            else
            {
                return AddSortDescending(query, sort.PropertyName, isFirstSort);
            }
        }

        protected abstract IOrderedQueryable<TEntity> AddSortDescending(IOrderedQueryable<TEntity> query, string propertyName, bool isFirstSort);
        protected abstract IOrderedQueryable<TEntity> AddSortAscending(IOrderedQueryable<TEntity> query, string propertyName, bool isFirstSort);

        protected virtual IQueryable<TEntity> AddSorts(Search search, IQueryable<TEntity> query)
        {
            if (search is null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (search.Sorts == null || search.Sorts.Count == 0)
            {
                return query;
            }
            else if (search.Sorts.Count == 1)
            {
                if (string.IsNullOrWhiteSpace(search.Sorts[0].PropertyName) == false)
                {
                    var returnValue = AddSort(EnsureIsOrderedQueryable(query), search.Sorts[0], true);

                    return returnValue;
                }
                else
                {
                    return query;
                }
            }
            else
            {
                var isFirst = true;

                foreach (var item in search.Sorts)
                {
                    if (string.IsNullOrWhiteSpace(item.PropertyName) == false)
                    {
                        query = AddSort(EnsureIsOrderedQueryable(query), item, isFirst);

                        isFirst = false;
                    }
                }

                return query;
            }
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
                        string.Format("Search operator '{0}' is not supported.", arg.Operator));
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
