using System;
using Benday.EfCore.SqlServer.TestApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace Benday.EfCore.SqlServer.IntegrationTests
{
    [TestClass]
    public class DirectlyAgainstDbContextFixture
    {
        private readonly string _ConnectionString = "Server=localhost; Database=benday-efcore-sqlserver; User Id=sa; Password=Pa$$word;";

        [TestMethod]
        public void CreateSampleData()
        {
            // arrange
           
            // act
            var data = CreateSamplePersonRecords();

            // assert

            Assert.AreNotEqual<int>(0, data.Count, "There should be test data records");

            using (var context = GetDbContext())
            {
                var reloaded = context.Persons.ToList();

                Assert.AreEqual<int>(data.Count, reloaded.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void CreateSampleData_CleansUpOldData()
        {
            // arrange

            // act
            CreateSamplePersonRecords();

            var data = CreateSamplePersonRecords();

            // assert

            Assert.AreNotEqual<int>(0, data.Count, "There should be test data records");

            using (var context = GetDbContext())
            {
                var reloaded = context.Persons.ToList();

                Assert.AreEqual<int>(data.Count, reloaded.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void LinqQuery_ContainsOrQueryAgainstDbContext_TwoCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "bonk";
            var expectedCount = 2;
                        
            using (var context = GetDbContext())
            {
                // act
                var query = context.Persons.Where(
                    p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString));

                DebugIQueryable(query);

                var actual = query.ToList();
                
                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void LinqQuery_ContainsAndQueryAgainstDbContext_ReturnsOneMatches()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchStringFirstName = "all";
            var searchStringLastName = "onk";
            var expectedCount = 1;

            using (var context = GetDbContext())
            {
                // act
                var actual = context.Persons.Where(
                    p => p.FirstName.Contains(searchStringFirstName) &&
                    p.LastName.Contains(searchStringLastName)).ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void LinqQuery_ContainsAndQueryAgainstDbContext_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchStringLastName = "onk";
            var expectedCount = 2;

            using (var context = GetDbContext())
            {
                // act
                var query = context.Persons.Where(
                    p => p.LastName.Contains("onk"));
                    
                DebugIQueryable(query);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void LinqQuery_ContainsAndQueryAgainstDbContext_StartsWith_OneCriteria_OneResult()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchStringLastName = "had";
            var expectedCount = 1;

            using (var context = GetDbContext())
            {
                // act
                var query = context.Persons.Where(
                    p => p.LastName.StartsWith(searchStringLastName));

                DebugIQueryable(query);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }
        
        [TestMethod]
        public void DynamicQuery_ContainsAndQueryAgainstDbContext_ReturnsOneMatches()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchStringFirstName = "all";
            var searchStringLastName = "onk";
            var expectedCount = 1;

            using (var context = GetDbContext())
            {
                // act
                var query = context.Persons.AsQueryable();

                query = query.Where(p => p.FirstName.Contains(searchStringFirstName));
                query = query.Where(p => p.LastName.Contains(searchStringLastName));
                
                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void DynamicQuery_Equals_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchStringFirstName = "bonk";
            var searchStringLastName = "bonk";
            var expectedCount = 1;

            using (var context = GetDbContext())
            {
                // act
                var expression = GetSingleEquals<Person>("LastName", "Bonkbonk");

                var query = context.Persons.Where(expression);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void DynamicQuery_Contains_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var expectedCount = 2;

            using (var context = GetDbContext())
            {
                // act
                var expression = GetContains<Person>("LastName", "onk");

                var query = context.Persons.Where(expression);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void DynamicQuery_Contains_TwoCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var expectedCount = 2;

            using (var context = GetDbContext())
            {
                // act
                var expression = GetContains<Person>("LastName", "FirstName", "onk");
                
                var query = context.Persons.Where(expression);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void DynamicQuery_Contains_WithChildCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var expectedCount = 2;

            using (var context = GetDbContext())
            {
                // act
                var personExpressions = GetContains<Person>("LastName", "FirstName", "onk");

                Expression<Func<Person, bool>> childExpression = p => p.Notes.Any(n => n.NoteText.Contains("onk"));

                var queryExpression = personExpressions.Or(childExpression);                    

                var query = context.Persons.Where(queryExpression);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        [TestMethod]
        public void DynamicQuery_Contains_TwoCriteria_AlternateVersion_EvaluatedInMemory()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var expectedCount = 2;

            using (var context = GetDbContext())
            {
                // act
                /*
                Expression<Func<Person, bool>> expr0 = p => p.FirstName.Contains("onk");
                Expression<Func<Person, bool>> expr1 = p => p.LastName.Contains("onk");
                */

                Predicate<Person> expr0 = p => p.FirstName.Contains("onk");
                Predicate<Person> expr1 = p => p.LastName.Contains("onk");

                var orExpressions = new List<Predicate<Person>>();

                orExpressions.Add(expr0);
                orExpressions.Add(expr1);

                Expression<Func<Person, bool>> exprWhere =
                    p => orExpressions.Any(expr => expr(p));

                var query = context.Persons.Where(exprWhere);

                var actual = query.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }


        public Expression<Func<T, bool>> GetSingleEquals<T>(string propertyName, string searchValue)
        {
            var parameterItem = Expression.Parameter(typeof(T), "item");

            var lambda = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Property(
                        parameterItem,
                        propertyName
                    ),
                    Expression.Constant(searchValue)
                ),
                parameterItem
            );

            // var result = queryableData.Where(lambda);
            return lambda;
        }

        public Expression<Func<T, bool>> GetContains<T>(string propertyName, string searchValue)
        {
            var parameterItem = Expression.Parameter(typeof(T), "item");

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            var lambda = Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    Expression.Property(
                        parameterItem,
                        propertyName
                    ),
                    containsMethod,
                    Expression.Constant(searchValue)), 
                    parameterItem
                );

            return lambda;
        }

        public Expression<Func<T, bool>> GetContains<T>(string propertyName1, string propertyName2, string searchValue)
        {
            var parameterItem = Expression.Parameter(typeof(T), "item");

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            var left = Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    Expression.Property(
                        parameterItem,
                        propertyName1
                    ),
                    containsMethod,
                    Expression.Constant(searchValue)),
                    parameterItem
                );

            var right = Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    Expression.Property(
                        parameterItem,
                        propertyName2
                    ),
                    containsMethod,
                    Expression.Constant(searchValue)),
                    parameterItem
                );

            var lambda = left.Or<T>(right);

            return lambda;
        }

        [TestMethod]
        public void CreateContainsOrQueryAgainstDbContext_WriteExpressionsToConsole()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "bonk";
            var expectedCount = 2;

            using (var context = GetDbContext())
            {
                // act
                var actualIQueryable = context.Persons.Where(
                    p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString));

                DebugIQueryable(actualIQueryable);

                var actual = actualIQueryable.ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
        }

        private void DebugIQueryable(IQueryable<Person> actualIQueryable)
        {
            var rootExpression = actualIQueryable.Expression;

            Console.WriteLine("asdf");
        }

        private List<Person> CreateSamplePersonRecords()
        {
            using (var context = GetDbContext())
            {
                DeleteExistingPersonRecords(context);

                return CreateSamplePersonRecords(context);
            }
        }

        private List<Person> CreateSamplePersonRecords(TestDbContext context)
        {
            var returnValues = new List<Person>();

            returnValues.Add(CreatePerson("James", "Ratt"));
            returnValues.Add(CreatePerson("Mary", "Haddalitalamb"));
            returnValues.Add(CreatePerson("Bing", "Bonkbonk"));
            returnValues.Add(CreatePerson("Sally", "Kahbonka"));
            returnValues.Add(CreatePerson("Turk", "Menistan"));
            returnValues.Add(CreatePerson("Mary Ann", "Thump"));

            context.Persons.AddRange(returnValues);

            context.SaveChanges();

            return returnValues;
        }

        private Person CreatePerson(string firstName, string lastName)
        {
            var temp = new Person();

            temp.FirstName = firstName;
            temp.LastName = lastName;

            var noteText0 = 
                String.Format("{0} {1} note {2}", firstName, lastName, "0");

            var noteText1 =
                String.Format("{0} {1} note {2}", firstName, lastName, "1");

            var noteText2 =
                String.Format("{0} {1} note {2}", firstName, lastName, "2");

            temp.Notes.Add(new PersonNote() { NoteText = noteText0 });
            temp.Notes.Add(new PersonNote() { NoteText = noteText1 });
            temp.Notes.Add(new PersonNote() { NoteText = noteText2 });

            return temp;
        }

        private void DeleteExistingPersonRecords(TestDbContext context)
        {
            var existing = context.Persons.ToList();

            existing.ForEach(p => context.Persons.Remove(p));

            context.SaveChanges();
        }

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }

        private TestDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(_ConnectionString);

            TestDbContext context = new TestDbContext(optionsBuilder.Options);

            return context;
        }
    }
}
