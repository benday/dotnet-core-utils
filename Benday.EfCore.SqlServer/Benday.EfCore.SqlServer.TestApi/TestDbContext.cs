using Microsoft.EntityFrameworkCore;

namespace Benday.EfCore.SqlServer.TestApi
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
