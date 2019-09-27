using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Common.UnitTests
{
    [TestClass]
    public class SearchFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private Search _SystemUnderTest;
        public Search SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new Search();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void Sorts_NotNull_OnInit()
        {
            Assert.IsNotNull(SystemUnderTest.Sorts);
        }

        [TestMethod]
        public void Sorts_ItemCountShouldBeZero_OnInit()
        {
            Assert.AreEqual<int>(0, SystemUnderTest.Sorts.Count, "Item count was wrong.");
        }

        [TestMethod]
        public void AddSort_DefaultSortDirectionIsAscending()
        {
            // arrange
            var expectedSortByValue = "asdf";
            var expectedSortDirection = SearchConstants.SortDirectionAscending;

            // act
            SystemUnderTest.AddSort(expectedSortByValue);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count, 
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }

        [TestMethod]
        public void AddSort_IgnoresDuplicateSortValue()
        {
            // arrange
            var expectedSortByValue = "asdf";
            var expectedSortDirection = SearchConstants.SortDirectionAscending;

            // act
            SystemUnderTest.AddSort(expectedSortByValue);
            // add duplicate
            SystemUnderTest.AddSort(expectedSortByValue);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }

        [TestMethod]
        public void AddSort_AddTwoSorts()
        {
            // arrange
            var expectedSortByValue1 = "asdf";
            var expectedSortByValue2 = "qwer";
            var expectedSortDirection = SearchConstants.SortDirectionAscending;

            // act
            SystemUnderTest.AddSort(expectedSortByValue1);
            // add duplicate
            SystemUnderTest.AddSort(expectedSortByValue2);

            // assert
            Assert.AreEqual<int>(2, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];
            AssertSort(actual, expectedSortByValue1, expectedSortDirection);

            actual = SystemUnderTest.Sorts[1];
            AssertSort(actual, expectedSortByValue2, expectedSortDirection);
        }

        private void AssertSort(SortBy actual, string expectedSortByValue, string expectedSortDirection)
        {
            Assert.IsNotNull(actual, "sortby is null");

            Assert.AreEqual<string>(expectedSortByValue, actual.PropertyName, "SortByValue");
            Assert.AreEqual<string>(expectedSortDirection, actual.Direction, "SortDirection");
        }

        [TestMethod]
        public void AddSort_Ascending()
        {
            // arrange
            var expectedSortByValue = "asdf";
            var expectedSortDirection = SearchConstants.SortDirectionAscending;

            // act
            SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }


        [TestMethod]
        public void AddSort_Descending()
        {
            // arrange
            var expectedSortByValue = "asdf";
            var expectedSortDirection = SearchConstants.SortDirectionDescending;

            // act
            SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddSort_ThrowsExceptionOnBogusSortDirection()
        {
            // arrange
            var expectedSortByValue = "asdf";
            var expectedSortDirection = "garbage";

            // act
            SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSort_ThrowsExceptionOnNullSortDirection()
        {
            // arrange
            var expectedSortByValue = "asdf";
            string expectedSortDirection = null;

            // act
            SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddSort_ThrowsExceptionOnStringEmptySortDirection()
        {
            // arrange
            var expectedSortByValue = "asdf";
            string expectedSortDirection = String.Empty;

            // act
            SystemUnderTest.AddSort(expectedSortByValue, expectedSortDirection);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Sorts.Count,
                "Item count was wrong.");

            var actual = SystemUnderTest.Sorts[0];

            AssertSort(actual, expectedSortByValue, expectedSortDirection);
        }
    }
}
