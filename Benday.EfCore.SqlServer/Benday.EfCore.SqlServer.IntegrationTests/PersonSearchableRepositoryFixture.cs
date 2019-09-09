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
using Benday.Common;

namespace Benday.EfCore.SqlServer.IntegrationTests
{
    [TestClass]
    public class PersonSearchableRepositoryFixture
    {
        private readonly string _ConnectionString = "Server=localhost; Database=benday-efcore-sqlserver; User Id=sa; Password=Pa$$word;";

        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private SqlPersonRepository _SystemUnderTest;
        public SqlPersonRepository SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SqlPersonRepository(GetDbContext());
                }

                return _SystemUnderTest;
            }
        }

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
        public void PersonSearchableRepository_Search_Contains_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "onk";
            var expectedCount = 2;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_TwoCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "onk";
            var expectedCount = 3;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString, SearchOperator.Or);
            search.AddArgument("FirstName", SearchMethod.Contains, searchString, SearchOperator.Or);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_TwoCriteria_NoResults()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "asdfqwer";
            var expectedCount = 0;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString);
            search.AddArgument("FirstName", SearchMethod.Contains, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_ChildEntitySearch()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "dal";
            var expectedCount = 1;

            var search = new Search();
            search.AddArgument("NoteText", SearchMethod.Contains, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_Equals_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "thump";
            var expectedCount = 2;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Equals, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
               
        [TestMethod]
        public void PersonSearchableRepository_Search_Equals_TwoCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "onk";
            var expectedCount = 3;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString, SearchOperator.Or);
            search.AddArgument("FirstName", SearchMethod.Contains, searchString, SearchOperator.Or);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_StartsWith_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "had";
            var expectedCount = 1;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.StartsWith, searchString, SearchOperator.Or);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_StartsWith_TwoCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var expectedCount = 1;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.StartsWith, "thu", SearchOperator.And);
            search.AddArgument("FirstName", SearchMethod.StartsWith, "glad", SearchOperator.And);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_NotEqual_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "thump";
            var expectedCount = 6;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.IsNotEqual, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_EndsWith_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "lamb";
            var expectedCount = 1;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.EndsWith, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }

        [TestMethod]
        public void PersonSearchableRepository_Search_DoesNotContain_OneCriteria()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "bon";
            var expectedCount = 6;

            var search = new Search();
            search.AddArgument("LastName", SearchMethod.DoesNotContain, searchString);

            // act
            var actual = SystemUnderTest.Search(search);

            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
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
            returnValues.Add(CreatePerson("Gladys", "Thump"));
            returnValues.Add(CreatePerson("Bonk", "Errific"));

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
