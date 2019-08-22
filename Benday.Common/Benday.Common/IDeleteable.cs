namespace Benday.Common
{
    public interface IDeleteable
    {
        bool IsMarkedForDelete { get; set; }
    }
}
