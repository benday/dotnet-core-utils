namespace Benday.Common
{
    public interface IDeleteable : IInt32Identity
    {
        bool IsMarkedForDelete { get; set; }
    }
}
