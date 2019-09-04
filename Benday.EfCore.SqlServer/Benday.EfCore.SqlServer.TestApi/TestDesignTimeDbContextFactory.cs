using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Benday.EfCore.SqlServer.TestApi
{

    public class TestDesignTimeDbContextFactory :
      IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext Create()
        {
            var environmentName =
       Environment.GetEnvironmentVariable(
        "ASPNETCORE_ENVIRONMENT");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        public TestDbContext CreateDbContext(string[] args)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        private TestDbContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("default");

            if (String.IsNullOrWhiteSpace(connstr) == true)
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'default'.");
            }
            else
            {
                return Create(connstr);
            }
        }

        private TestDbContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
                   $"{nameof(connectionString)} is null or empty.",
                   nameof(connectionString));

            var optionsBuilder =
              new DbContextOptionsBuilder<TestDbContext>();

            Console.WriteLine(
             "TestDesignTimeDbContextFactory.Create(string): Connection string: {0}",
             connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            return new TestDbContext(optionsBuilder.Options);
        }
    }
}
