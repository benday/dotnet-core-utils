using Benday.Common;

namespace Benday.EfCore.Cosmos.TestApi
{
    public class EmailNewsletterSubscription : EntityBase
    {
        public string EmailAddress { get; set; }
    }
}
