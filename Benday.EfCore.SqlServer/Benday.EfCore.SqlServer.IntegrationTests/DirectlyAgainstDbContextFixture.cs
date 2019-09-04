using System;
using Benday.EfCore.SqlServer.TestApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

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
        public void CreateContainsOrQueryAgainstDbContext()
        {
            // arrange
            var data = CreateSamplePersonRecords();
            var searchString = "bonk";
            var expectedCount = 2;
                        
            using (var context = GetDbContext())
            {
                // act
                var actual = context.Persons.Where(
                    p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString)).ToList();

                // assert
                Assert.AreEqual<int>(expectedCount, actual.Count, "Reloaded record count was wrong");
            }
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

            return temp;
        }

        private void DeleteExistingPersonRecords(TestDbContext context)
        {
            var existing = context.Persons.ToList();

            existing.ForEach(p => context.Persons.Remove(p));

            context.SaveChanges();
        }

        private TestDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder.UseSqlServer(_ConnectionString);

            TestDbContext context = new TestDbContext(optionsBuilder.Options);

            return context;
        }
    }
}
