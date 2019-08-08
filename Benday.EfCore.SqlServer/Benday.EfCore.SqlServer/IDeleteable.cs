namespace Benday.EfCore.SqlServer
{
    public interface IDeleteable
    {
        bool IsMarkedForDelete { get; set; }
    }
}
