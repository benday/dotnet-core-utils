using Benday.Common;

namespace Benday.EfCore.SqlServer.TestApi
{
    public class EmailNewsletterSubscription : IInt32Identity
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
}
