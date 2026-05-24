// PSEUDOCODE PLAN:
// 1) Add/keep a design-time factory implementing IDesignTimeDbContextFactory<AppDbContext>
//    so `dotnet ef migrations add ...` can instantiate the DbContext.
// 2) Read the connection string from environment variables commonly used by .NET config.
// 3) Fail fast with a clear error if no connection string is provided.
// 4) Configure the relational provider (PostgreSQL via Npgsql) and set the migrations assembly.
// 5) Return a new AppDbContext with the configured options.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Order.Infrastructure
{
    public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=mydb;Username=postgres;Password=postgres",
                npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}