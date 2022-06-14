using Benday.EfCore.SqlServer.TestApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.EfCore.SqlServer.IntegrationTests
{
    [TestClass]
    public static class EfCoreUnitTestSetup
    {
        private static readonly string _ConnectionString = "Server=localhost; Database=benday-efcore-sqlserver; User Id=sa; Password=Pa$$word;";
        
        [AssemblyInitializeAttribute]
        #pragma warning disable IDE0060 // Remove unused parameter
        public static void OnAssemblyInitialize(TestContext testContext)
            #pragma warning restore IDE0060 // Remove unused parameter
        {
            using var dbcontext = GetDbContext();
            dbcontext.Database.EnsureCreated();
        }
        
        private static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                builder.AddConsole()
                .AddFilter(DbLoggerCategory.Database.Command.Name,
                LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                .GetService<ILoggerFactory>();
        }
        
        private static TestDbContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(_ConnectionString);
            
            var context = new TestDbContext(optionsBuilder.Options);
            
            return context;
        }
    }
}