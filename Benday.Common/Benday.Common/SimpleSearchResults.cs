using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Benday.Common
{
    public class SimpleSearchResults<T> : ISortableResult
    {
        public SimpleSearchResults()
        {
            CurrentSortProperty = string.Empty;
            CurrentSortDirection = SearchConstants.SortDirectionAscending;
        }

        [Display(Name = "Current Sort Property")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrentSortProperty { get; set; }

        [Display(Name = "Current Sort Direction")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CurrentSortDirection { get; set; }

        [JsonPropertyName("simpleSearchValue")]
        public string SearchValue { get; set; }

        public int CurrentPage { get; set; }

        public int TotalCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PageCount { get; set; }

        public IList<T> CurrentPageValues { get; set; }
    }
}
