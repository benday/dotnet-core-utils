namespace Benday.Common
{
    public interface ISortableResult
    {
        string CurrentSortDirection { get; set; }
        string CurrentSortProperty { get; set; }
    }
}
