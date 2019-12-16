using Benday.Common;

namespace Benday.EfCore.Cosmos.TestApi
{
    public class EmailNewsletterSubscription : IInt32Identity
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
}
