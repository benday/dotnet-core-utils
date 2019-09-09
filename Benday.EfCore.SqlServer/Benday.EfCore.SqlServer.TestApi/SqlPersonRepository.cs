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

        protected override DbSet<Person> EntityDbSet
        {
            get
            {
                return Context.Persons;
            }
        }

        protected override List<string> Includes
        {
            get
            {
                var includes = new List<string>();

                includes.Add(nameof(Person.Notes));

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
                        String.Format("Unknown argument '{0}'.", arg.PropertyName));
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
                        String.Format("Unknown argument '{0}'.", arg.PropertyName));
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
                        String.Format("Unknown argument '{0}'.", arg.PropertyName));
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
                        String.Format("Unknown argument '{0}'.", arg.PropertyName));
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
                        String.Format("Unknown argument '{0}'.", arg.PropertyName));
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
                        String.Format("Unknown argument '{0}'.", arg.PropertyName));
                }

                return returnValue;
            }
        }
    }
}