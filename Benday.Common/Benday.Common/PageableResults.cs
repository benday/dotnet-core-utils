using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Benday.Common
{
    public class PageableResults<T> : IPageableResults
    {
        private IList<T> _results;
        private int _currentPage;

        public PageableResults()
        {
            ItemsPerPage = 10;
        }

        public void Initialize(IList<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            Results = values;

            PageCount = CalculatePageCount();
            SetCurrentPage(1);
        }

        private void SetCurrentPage(int pageNumber)
        {
            if (pageNumber >= PageCount)
            {
                _currentPage = PageCount;
            }
            else if (pageNumber < 1)
            {
                _currentPage = 1;
            }
            else
            {
                _currentPage = pageNumber;
            }

            PopulatePageValues();
        }

        private void PopulatePageValues()
        {
            if (CurrentPage == 1)
            {
                PageValues = Results.Take(ItemsPerPage).ToList();
            }
            else
            {
                PageValues = Results
                    .Skip((CurrentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage).ToList();
            }
        }

        private int CalculatePageCount()
        {
            if (ItemsPerPage == 0)
            {
                return 0;
            }
            else if (ItemsPerPage < 0)
            {
                return 0;
            }
            else
            {
                var pageCount = TotalCount / ItemsPerPage;
                var remainder = TotalCount % ItemsPerPage;

                if (remainder == 0)
                {
                    return pageCount;
                }
                else
                {
                    return pageCount + 1;
                }
            }
        }

        [JsonIgnore]
        public IList<T> Results
        {
            get
            {
                if (_results == null)
                {
                    _results = new List<T>();
                }

                return _results;
            }
            private set => _results = value;
        }

        public int TotalCount => Results.Count;

        public int ItemsPerPage { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage
        {
            get => _currentPage;
            set => SetCurrentPage(value);
        }

        public IList<T> PageValues { get; private set; }
    }
}
