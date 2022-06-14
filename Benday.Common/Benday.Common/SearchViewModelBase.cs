using System.ComponentModel.DataAnnotations;

namespace Benday.Common
{
    public class SearchViewModelBase<T> : SortableViewModelBase<T>
    {
        public SearchViewModelBase()
        {
            IsSimpleSearch = true;
        }

        [Display(Name = "Simple Search")]
        public bool IsSimpleSearch { get; set; }

        [Display(Name = "Simple Search Value")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SimpleSearchValue { get; set; }
    }
}
