using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override DbSet<Person> EntityDbSet => Context.Persons;

        protected override List<string> Includes
        {
            get
            {
                var includes = new List<string>
                {
                    nameof(Person.Notes)
                };

                return includes;
            }
        }

        protected override Expression<Func<Person, bool>> GetPredicateForContains(SearchArgument arg)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            else
            {
                Expression<Func<Person, bool>> returnValue;

                if (arg.PropertyName == "FirstName")
                {
                    returnValue = (p) => p.FirstName.Contains(arg.SearchValue);
                }
                else if (arg.PropertyName == "LastName")
                {
                    returnValue = (p) => p.LastName.Contains(arg.SearchValue);
                }
                else if (arg.PropertyName == "NoteText")
                {
                    returnValue = (p) => p.Notes.Any(n => n.NoteText.Contains(arg.SearchValue));
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }

        protected override Expression<Func<Person, bool>> GetPredicateForDoesNotContain(SearchArgument arg)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            else
            {
                Expression<Func<Person, bool>> returnValue;

                if (arg.PropertyName == "FirstName")
                {
                    returnValue = (p) => !p.FirstName.Contains(arg.SearchValue);
                }
                else if (arg.PropertyName == "LastName")
                {
                    returnValue = (p) => !p.LastName.Contains(arg.SearchValue);
                }
                else if (arg.PropertyName == "NoteText")
                {
                    returnValue = (p) => !p.Notes.Any(n => n.NoteText.Contains(arg.SearchValue));
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }

        protected override Expression<Func<Person, bool>> GetPredicateForEndsWith(SearchArgument arg)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            else
            {
                Expression<Func<Person, bool>> returnValue;

                if (arg.PropertyName == "FirstName")
                {
                    returnValue = (p) => p.FirstName.EndsWith(arg.SearchValue);
                }
                else if (arg.PropertyName == "LastName")
                {
                    returnValue = (p) => p.LastName.EndsWith(arg.SearchValue);
                }
                else if (arg.PropertyName == "NoteText")
                {
                    returnValue = (p) => p.Notes.Any(n => n.NoteText.EndsWith(
                        arg.SearchValue));
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }

        protected override Expression<Func<Person, bool>> GetPredicateForEquals(SearchArgument arg)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            else
            {
                Expression<Func<Person, bool>> returnValue;

                if (arg.PropertyName == "FirstName")
                {
                    returnValue = (p) => p.FirstName == arg.SearchValue;
                }
                else if (arg.PropertyName == "LastName")
                {
                    returnValue = (p) => p.LastName == arg.SearchValue;
                }
                else if (arg.PropertyName == "NoteText")
                {
                    returnValue = (p) => p.Notes.Any(n => n.NoteText == arg.SearchValue);
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }

        protected override Expression<Func<Person, bool>> GetPredicateForIsNotEqualTo(SearchArgument arg)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            else
            {
                Expression<Func<Person, bool>> returnValue;

                if (arg.PropertyName == "FirstName")
                {
                    returnValue = (p) => p.FirstName != arg.SearchValue;
                }
                else if (arg.PropertyName == "LastName")
                {
                    returnValue = (p) => p.LastName != arg.SearchValue;
                }
                else if (arg.PropertyName == "NoteText")
                {
                    returnValue = (p) => p.Notes.Any(n => n.NoteText != arg.SearchValue);
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }

        protected override Expression<Func<Person, bool>> GetPredicateForStartsWith(SearchArgument arg)
        {
            if (arg is null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            else
            {
                Expression<Func<Person, bool>> returnValue;

                if (arg.PropertyName == "FirstName")
                {
                    returnValue = (p) => p.FirstName.StartsWith(arg.SearchValue);
                }
                else if (arg.PropertyName == "LastName")
                {
                    returnValue = (p) => p.LastName.StartsWith(arg.SearchValue);
                }
                else if (arg.PropertyName == "NoteText")
                {
                    returnValue = (p) => p.Notes.Any(n => n.NoteText.StartsWith(
                        arg.SearchValue));
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }

        protected override IOrderedQueryable<Person> AddSortDescending(IOrderedQueryable<Person> query, string propertyName, bool isFirstSort)
        {
            if (propertyName == "FirstName")
            {
                if (isFirstSort == true)
                {
                    return query.OrderByDescending(x => x.FirstName);
                }
                else
                {
                    return query.ThenByDescending(x => x.FirstName);
                }
            }
            else if (propertyName == "LastName")
            {
                if (isFirstSort == true)
                {
                    return query.OrderByDescending(x => x.LastName);
                }
                else
                {
                    return query.ThenByDescending(x => x.LastName);
                }
            }
            else if (propertyName == "Id")
            {
                if (isFirstSort == true)
                {
                    return query.OrderByDescending(x => x.Id);
                }
                else
                {
                    return query.ThenByDescending(x => x.Id);
                }
            }
            else
            {
                return null;
            }
        }

        protected override IOrderedQueryable<Person> AddSortAscending(IOrderedQueryable<Person> query, string propertyName, bool isFirstSort)
        {
            if (propertyName == "FirstName")
            {
                if (isFirstSort == true)
                {
                    return query.OrderBy(x => x.FirstName);
                }
                else
                {
                    return query.ThenBy(x => x.FirstName);
                }
            }
            else if (propertyName == "LastName")
            {
                if (isFirstSort == true)
                {
                    return query.OrderBy(x => x.LastName);
                }
                else
                {
                    return query.ThenBy(x => x.LastName);
                }
            }
            else if (propertyName == "Id")
            {
                if (isFirstSort == true)
                {
                    return query.OrderBy(x => x.Id);
                }
                else
                {
                    return query.ThenBy(x => x.Id);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
