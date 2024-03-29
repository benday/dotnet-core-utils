using Benday.EfCore.SqlServer.TestApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Benday.Common;

namespace Benday.EfCore.SqlServer.IntegrationTests
{
    [TestClass]
    public class PersonSearchableRepositoryFixture
    {
        private const string _ConnectionString = "Server=localhost; Database=benday-efcore-sqlserver; User Id=sa; Password=Pa$$word;";
        
        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;
        }
        
        private SqlPersonRepository _systemUnderTest;
        public SqlPersonRepository SystemUnderTest
        {
            get
            {
                if (_systemUnderTest == null)
                {
                    _systemUnderTest = new SqlPersonRepository(GetDbContext());
                }
                
                return _systemUnderTest;
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
            
            using var context = GetDbContext();
            var reloaded = context.Persons.ToList();
            
            Assert.AreEqual<int>(data.Count, reloaded.Count, "Reloaded record count was wrong");
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
            
            using var context = GetDbContext();
            var reloaded = context.Persons.ToList();
            
            Assert.AreEqual<int>(data.Count, reloaded.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_OneCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "onk";
            var expectedCount = 2;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_TwoCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "onk";
            var expectedCount = 3;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString, SearchOperator.Or);
            search.AddArgument("FirstName", SearchMethod.Contains, searchString, SearchOperator.Or);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_TwoCriteria_NoResults()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "asdfqwer";
            var expectedCount = 0;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString);
            search.AddArgument("FirstName", SearchMethod.Contains, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_Contains_ChildEntitySearch()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "dal";
            var expectedCount = 1;
            
            var search = new Search();
            search.AddArgument("NoteText", SearchMethod.Contains, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_Equals_OneCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "thump";
            var expectedCount = 2;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Equals, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_Equals_TwoCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "onk";
            var expectedCount = 3;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Contains, searchString, SearchOperator.Or);
            search.AddArgument("FirstName", SearchMethod.Contains, searchString, SearchOperator.Or);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_StartsWith_OneCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "had";
            var expectedCount = 1;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.StartsWith, searchString, SearchOperator.Or);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_StartsWith_TwoCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 1;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.StartsWith, "thu", SearchOperator.And);
            search.AddArgument("FirstName", SearchMethod.StartsWith, "glad", SearchOperator.And);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_NotEqual_OneCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "thump";
            var expectedCount = 6;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.IsNotEqual, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_AddsIncludesForChildObjects()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "thump";
            var expectedCount = 6;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.IsNotEqual, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actuals = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actuals.Count, "Reloaded record count was wrong");
            
            foreach (var actual in actuals)
            {
                Assert.IsNotNull(actual.Notes, "Notes collection was null.");
                
                Assert.AreNotEqual<int>(0, actual.Notes.Count, "Note count should not be zero.");
            }
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_EndsWith_OneCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "lamb";
            var expectedCount = 1;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.EndsWith, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_NoCriteria_OneSort_Ascending()
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 8;
            
            var search = new Search();
            search.AddSort("LastName");
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            var expected = actual.OrderBy(x => x.LastName).ToList();
            
            AssertAreEqual(expected, actual, "Sorted by last name ascending");
        }
        
        [DataTestMethod]
        [DataRow("    ", DisplayName = "whitespace string")]
        [DataRow("", DisplayName = "empty string")]
        public void PersonSearchableRepository_Search_SingleSort_IgnoresEmptySort(string sortBy)
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 8;
            
            var search = new Search();
            search.AddSort(sortBy);
            
            Assert.AreEqual<int>(1, search.Sorts.Count, "Sort count was wrong");
            
            // act
            var unsortedResults = SystemUnderTest.Search(new Search()).Results;
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            var expected = unsortedResults.ToList();
            
            AssertAreEqual(expected, actual, "Sorted by nothing in particular");
        }
        
        [DataTestMethod]
        [DataRow("    ", DisplayName = "whitespace string")]
        [DataRow("", DisplayName = "empty string")]
        public void PersonSearchableRepository_Search_MultipleSorts_IgnoresEmptySort(string sortBy)
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 8;
            
            var search = new Search();
            search.AddSort(nameof(Person.FirstName));
            search.AddSort(sortBy);
            
            Assert.AreEqual<int>(2, search.Sorts.Count, "Sort count was wrong");
            
            var expectedDataSearch = new Search();
            expectedDataSearch.AddSort(nameof(Person.FirstName));
            
            // act
            var unsortedResults = SystemUnderTest.Search(expectedDataSearch).Results;
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            var expected = unsortedResults.ToList();
            
            AssertAreEqual(expected, actual, "Sorted by nothing in particular");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_NoCriteria_TwoSorts_Ascending()
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 8;
            
            var search = new Search();
            search.AddSort("LastName");
            search.AddSort("FirstName");
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            var expected = actual.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
            
            AssertAreEqual(expected, actual, "Sorted by last name ascending");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_NoCriteria_TwoSorts_AscendingAndDescending()
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 8;
            
            var search = new Search();
            search.AddSort("LastName");
            search.AddSort("FirstName", SearchConstants.SortDirectionDescending);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            var expected = actual.OrderBy(x => x.LastName).ThenByDescending(x => x.FirstName).ToList();
            
            AssertAreEqual(expected, actual, "Sorted by last name ascending");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_OneCriteria_OneSort_Ascending()
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 2;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Equals, "Thump");
            search.AddSort("FirstName", SearchConstants.SortDirectionAscending);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            
            Assert.AreEqual<string>("Gladys", actual[0].FirstName, "Unexpected first name item 0");
            Assert.AreEqual<string>("Mary Ann", actual[1].FirstName, "Unexpected first name item 1");
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_OneCriteria_OneSort_Descending()
        {
            // arrange
            CreateSamplePersonRecords();
            var expectedCount = 2;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.Equals, "Thump");
            search.AddSort("FirstName", SearchConstants.SortDirectionDescending);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            
            
            Assert.AreEqual<string>("Mary Ann", actual[0].FirstName, "Unexpected first name item 0");
            Assert.AreEqual<string>("Gladys", actual[1].FirstName, "Unexpected first name item 1");
        }
        
        private static void AssertAreEqual(List<Person> expected, IList<Person> actual, string message)
        {
            var actualAsList = actual.ToList();
            
            CollectionAssert.AreEqual(expected, actualAsList, message);
        }
        
        [TestMethod]
        public void PersonSearchableRepository_Search_DoesNotContain_OneCriteria()
        {
            // arrange
            CreateSamplePersonRecords();
            var searchString = "bon";
            var expectedCount = 6;
            
            var search = new Search();
            search.AddArgument("LastName", SearchMethod.DoesNotContain, searchString);
            
            // act
            var result = SystemUnderTest.Search(search);
            var actual = result.Results;
            
            // assert
            Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
        }
        
        private static List<Person> CreateSamplePersonRecords()
        {
            using var context = GetDbContext();
            DeleteExistingPersonRecords(context);
            
            return CreateSamplePersonRecords(context);
        }
        
        private static List<Person> CreateSamplePersonRecords(TestDbContext context)
        {
            var returnValues = new List<Person>
            {
                CreatePerson("James", "Ratt"),
                    CreatePerson("Mary", "Haddalitalamb"),
                    CreatePerson("Bing", "Bonkbonk"),
                    CreatePerson("Sally", "Kahbonka"),
                    CreatePerson("Turk", "Menistan"),
                    CreatePerson("Mary Ann", "Thump"),
                    CreatePerson("Gladys", "Thump"),
                    CreatePerson("Bonk", "Errific")
            };
            
            context.Persons.AddRange(returnValues);
            
            context.SaveChanges();
            
            return returnValues;
        }
        
        private static Person CreatePerson(string firstName, string lastName)
        {
            var temp = new Person
            {
                FirstName = firstName,
                    LastName = lastName
            };
            
            var noteText0 =
                string.Format("{0} {1} note {2}", firstName, lastName, "0");
            
            var noteText1 =
                string.Format("{0} {1} note {2}", firstName, lastName, "1");
            
            var noteText2 =
                string.Format("{0} {1} note {2}", firstName, lastName, "2");
            
            temp.Notes.Add(new PersonNote() { NoteText = noteText0 });
            temp.Notes.Add(new PersonNote() { NoteText = noteText1 });
            temp.Notes.Add(new PersonNote() { NoteText = noteText2 });
            
            return temp;
        }
        
        private static void DeleteExistingPersonRecords(TestDbContext context)
        {
            var existing = context.Persons.ToList();
            
            existing.ForEach(p => context.Persons.Remove(p));
            
            context.SaveChanges();
        }
        
        private static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                builder.AddConsole()
                .AddFilter(DbLoggerCategory.Database.Command.Name,
                LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                .GetService<ILoggerFactory>();
        }
        
        private static TestDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(_ConnectionString);
            
            var context = new TestDbContext(optionsBuilder.Options);
            
            return context;
        }
    }
}