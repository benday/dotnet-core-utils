using Benday.EfCore.Cosmos.TestApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Benday.EfCore.Cosmos.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected TestDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseCosmos(
                "https://localhost:8081", 
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", 
                databaseName: "BendayEfCoreCosmosTesting"); ;

            TestDbContext context = new TestDbContext(optionsBuilder.Options);

            return context;
        }

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }
    }
}
