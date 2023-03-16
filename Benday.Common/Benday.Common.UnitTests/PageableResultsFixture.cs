using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Common.UnitTests
{
    [TestClass]
    public class PageableResultsFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;
        }

        private PageableResults<string> _systemUnderTest;
        public PageableResults<string> SystemUnderTest
        {
            get
            {
                if (_systemUnderTest == null)
                {
                    _systemUnderTest = new PageableResults<string>();
                }

                return _systemUnderTest;
            }
        }

        [TestMethod]
        public void Results_WhenUninitialized_IsNotNull()
        {
            // arrange

            // act

            // assert
            Assert.IsNotNull(SystemUnderTest.Results, "Results should not be null.");
        }

        [TestMethod]
        public void TotalCount_WhenUninitialized_IsZero()
        {
            // arrange

            // act

            // assert
            Assert.AreEqual<int>(0, SystemUnderTest.TotalCount, "TotalCount be zero.");
        }

        [TestMethod]
        public void ItemsPerPage_DefaultValue()
        {
            // arrange
            var expected = 10;

            // act
            var actual = SystemUnderTest.ItemsPerPage;

            // assert
            Assert.AreEqual<int>(expected, actual, "ItemsPerPage was wrong.");
        }

        [TestMethod]
        public void TotalCountReturnsTotalOfAllRecords()
        {
            // arrange
            var expectedNumberOfRecords = 300;

            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            // act
            var actual = SystemUnderTest.TotalCount;

            // assert

            Assert.AreEqual<int>(expectedNumberOfRecords, actual,
                "TotalCount was wrong.");
        }

        [TestMethod]
        public void PageCount_NoRemainder()
        {
            // arrange
            var expectedNumberOfRecords = 300;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedPageCount = 30;

            // act
            var actual = SystemUnderTest.PageCount;

            // assert

            Assert.AreEqual<int>(expectedPageCount, actual,
                "PageCount was wrong.");
        }

        [TestMethod]
        public void PageCount_Remainder()
        {
            // arrange
            var expectedNumberOfRecords = 305;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedPageCount = 31;

            // act
            var actual = SystemUnderTest.PageCount;

            // assert

            Assert.AreEqual<int>(expectedPageCount, actual,
                "PageCount was wrong.");
        }

        [TestMethod]
        public void PageCount_LessThanOnePage()
        {
            // arrange
            var expectedNumberOfRecords = 5;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedPageCount = 1;

            // act
            var actual = SystemUnderTest.PageCount;

            // assert

            Assert.AreEqual<int>(expectedPageCount, actual,
                "PageCount was wrong.");
        }

        [TestMethod]
        public void PageCount_NoRecords()
        {
            // arrange
            var expectedNumberOfRecords = 0;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(
                CreateModels(expectedNumberOfRecords));

            var expectedPageCount = 0;

            // act
            var actual = SystemUnderTest.PageCount;

            // assert

            Assert.AreEqual<int>(expectedPageCount, actual,
                "PageCount was wrong.");
        }

        [TestMethod]
        public void CurrentPage_SetToOneOnInitialize()
        {
            // arrange
            var expectedNumberOfRecords = 305;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedCurrentPage = 1;

            // act
            var actual = SystemUnderTest.CurrentPage;

            // assert
            Assert.AreEqual<int>(expectedCurrentPage, actual,
                "CurrentPage was wrong.");
        }

        [TestMethod]
        public void CurrentPage_SetToHigherThanPageCountSetsToHighestPage()
        {
            // arrange
            var expectedNumberOfRecords = 100;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedCurrentPage = 10;

            // act
            SystemUnderTest.CurrentPage = 300;

            // assert
            var actual = SystemUnderTest.CurrentPage;

            Assert.AreEqual<int>(expectedCurrentPage, actual,
                "CurrentPage was wrong.");
        }

        [TestMethod]
        public void CurrentPage_SetToLowerThanPageCountSetsToOne()
        {
            // arrange
            var expectedNumberOfRecords = 100;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedCurrentPage = 1;

            // act
            SystemUnderTest.CurrentPage = -300;

            // assert
            var actual = SystemUnderTest.CurrentPage;

            Assert.AreEqual<int>(expectedCurrentPage, actual,
                "CurrentPage was wrong.");
        }

        [TestMethod]
        public void CurrentPage_SetToValidPageNumber()
        {
            // arrange
            var expectedNumberOfRecords = 100;

            SystemUnderTest.ItemsPerPage = 10;
            SystemUnderTest.Initialize(CreateModels(expectedNumberOfRecords));

            var expectedCurrentPage = 5;

            // act
            SystemUnderTest.CurrentPage = expectedCurrentPage;

            // assert
            var actual = SystemUnderTest.CurrentPage;

            Assert.AreEqual<int>(expectedCurrentPage, actual,
                "CurrentPage was wrong.");
        }

        [TestMethod]
        public void PageValues_InitializesToPage1Values()
        {
            // arrange
            var expectedNumberOfRecords = 100;

            var expectedItemsPerPage = 10;

            SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
            var allValues = CreateModels(expectedNumberOfRecords);
            SystemUnderTest.Initialize(allValues);

            var expectedPage1Values =
                allValues.Take(expectedItemsPerPage).ToList();

            // act
            var actualPageValues = SystemUnderTest.PageValues;

            // assert
            AssertAreEqual(expectedPage1Values, actualPageValues,
                "Page values was wrong.");

            Assert.AreEqual<int>(expectedItemsPerPage,
                actualPageValues.Count,
                "Number of values on page was wrong.");
        }

        private static void AssertAreEqual(List<string> expected, IList<string> actual, string message)
        {
            var actualAsList = actual.ToList();

            CollectionAssert.AreEqual(expected, actualAsList, message);
        }

        [TestMethod]
        public void PageValues_ChangePageUpdatesPageValues_Page2()
        {
            // arrange
            var expectedNumberOfRecords = 100;

            var expectedItemsPerPage = 10;

            SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
            var allValues = CreateModels(expectedNumberOfRecords);
            SystemUnderTest.Initialize(allValues);

            var expectedPage2Values =
                allValues
                .Skip(expectedItemsPerPage)
                .Take(expectedItemsPerPage).ToList();

            // act
            SystemUnderTest.CurrentPage = 2;
            var actualPageValues = SystemUnderTest.PageValues;

            // assert
            AssertAreEqual(expectedPage2Values, actualPageValues,
                "Page values was wrong.");

            Assert.AreEqual<int>(expectedItemsPerPage,
                actualPageValues.Count,
                "Number of values on page was wrong.");
        }

        [TestMethod]
        public void PageValues_ChangePageUpdatesPageValues_Page3()
        {
            // arrange
            var expectedNumberOfRecords = 100;

            var expectedItemsPerPage = 10;

            SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
            var allValues = CreateModels(expectedNumberOfRecords);
            SystemUnderTest.Initialize(allValues);

            var expectedPage3Values =
                allValues
                .Skip(expectedItemsPerPage * 2)
                .Take(expectedItemsPerPage).ToList();

            // act
            SystemUnderTest.CurrentPage = 3;
            var actualPageValues = SystemUnderTest.PageValues;

            // assert
            AssertAreEqual(expectedPage3Values, actualPageValues,
                "Page values was wrong.");

            Assert.AreEqual<int>(expectedItemsPerPage,
                actualPageValues.Count,
                "Number of values on page was wrong.");
        }

        [TestMethod]
        public void PageValues_ChangePageUpdatesPageValues_LastPage_Remainder()
        {
            // arrange
            var expectedNumberOfRecords = 25;

            var expectedItemsPerPage = 10;

            SystemUnderTest.ItemsPerPage = expectedItemsPerPage;
            var allValues = CreateModels(expectedNumberOfRecords);
            SystemUnderTest.Initialize(allValues);

            var expectedPage3Values =
                allValues
                .Skip(expectedItemsPerPage * 2)
                .Take(expectedItemsPerPage).ToList();

            // act
            SystemUnderTest.CurrentPage = 3;
            var actualPageValues = SystemUnderTest.PageValues;

            // assert
            AssertAreEqual(expectedPage3Values, actualPageValues,
                "Page values was wrong.");

            Assert.AreEqual<int>(5, actualPageValues.Count,
                "Number of values on page was wrong.");
        }

        private static List<string> CreateModels(int expectedNumberOfRecords)
        {
            var returnValues = new List<string>();

            for (var i = 0; i < expectedNumberOfRecords; i++)
            {
                returnValues.Add(string.Format("item {0}", i));
            }

            return returnValues;
        }
    }
}
