using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Common
{
    public class Search
    {
        public Search()
        {
            Arguments = new List<SearchArgument>();
            Sorts = new List<SortBy>();
            MaxNumberOfResults = -1;
            Name = string.Empty;
        }

        /// <summary>
        /// Name of the search to run.  (Empty string or null string is the default.)
        /// </summary>
        public string Name { get; set; }

        public List<SearchArgument> Arguments { get; }

        public void AddArgument(
            string propertyName,
            SearchMethod method,
            string value,
            SearchOperator combineWithOtherArgumentsAs = SearchOperator.And)
        {
            Arguments.Add(
                new SearchArgument(propertyName, method, value, combineWithOtherArgumentsAs));
        }

        public void AddArgument(
            string propertyName,
            SearchMethod method,
            int value,
            SearchOperator combineWithOtherArgumentsAs = SearchOperator.And)
        {
            Arguments.Add(
                new SearchArgument(propertyName, method, value, combineWithOtherArgumentsAs));
        }

        public int MaxNumberOfResults { get; set; }

        public List<SortBy> Sorts { get; set; }

        public void AddSort(string sortByPropertyName,
            string direction = SearchConstants.SortDirectionAscending)
        {
            if (sortByPropertyName is null)
            {
                throw new System.ArgumentNullException(nameof(sortByPropertyName));
            }

            if (direction is null)
            {
                throw new System.ArgumentNullException(nameof(direction));
            }

            string directionCleaned;

            if (string.Compare(direction,
                SearchConstants.SortDirectionAscending, true) == 0)
            {
                directionCleaned = SearchConstants.SortDirectionAscending;
            }
            else if (string.Compare(direction,
                SearchConstants.SortDirectionDescending, true) == 0)
            {
                directionCleaned = SearchConstants.SortDirectionDescending;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(direction),
                    string.Format("Value should be '{0}' or '{1}'.",
                    SearchConstants.SortDirectionAscending,
                    SearchConstants.SortDirectionDescending));
            }

            AddSort(new SortBy()
            {
                PropertyName = sortByPropertyName,
                Direction = directionCleaned
            });
        }

        private void AddSort(SortBy sortBy)
        {
            if (sortBy is null)
            {
                throw new ArgumentNullException(nameof(sortBy));
            }

            var match = (from temp in Sorts
                         where
                         string.Compare(temp.PropertyName, sortBy.PropertyName, true) == 0
                         select temp).FirstOrDefault();

            if (match == null)
            {
                Sorts.Add(sortBy);
            }
        }
    }
}
