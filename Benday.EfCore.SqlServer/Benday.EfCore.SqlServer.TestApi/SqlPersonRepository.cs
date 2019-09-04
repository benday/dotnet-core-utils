using System;
using System.Linq.Expressions;
using Benday.Common;
using Microsoft.EntityFrameworkCore;

namespace Benday.EfCore.SqlServer.TestApi
{
    public class SqlPersonRepository :
        SqlEntityFrameworkSearchableRepositoryBase<Person, TestDbContext>
    {
        public SqlPersonRepository(TestDbContext context) : base(context)
        {
        }

        protected override DbSet<Person> EntityDbSet
        {
            get
            {
                return Context.Persons;
            }
        }

        protected override Expression<Func<Person, bool>> AddWhereClauseForContains(Expression<Func<Person, bool>> predicate, SearchArgument arg)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Person, bool>> AddWhereClauseForDoesNotContain(Expression<Func<Person, bool>> predicate, SearchArgument arg)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Person, bool>> AddWhereClauseForEndsWith(Expression<Func<Person, bool>> predicate, SearchArgument arg)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Person, bool>> AddWhereClauseForExact(Expression<Func<Person, bool>> predicate, SearchArgument arg)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Person, bool>> AddWhereClauseForIsNotEqualTo(Expression<Func<Person, bool>> predicate, SearchArgument arg)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Person, bool>> AddWhereClauseForStartsWith(Expression<Func<Person, bool>> predicate, SearchArgument arg)
        {
            throw new NotImplementedException();
        }
    }
}