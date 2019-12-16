﻿using Microsoft.EntityFrameworkCore;

namespace Benday.EfCore.Cosmos.TestApi
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<EmailNewsletterSubscription> EmailNewsletterSubscriptions { get; set; }

    }
}