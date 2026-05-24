using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Infrastructure
{
    public sealed class StockDbContextFactory : IDesignTimeDbContextFactory<StockServiceDbContext>
    {
        public StockServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockServiceDbContext>();
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=mydb;Username=postgres;Password=postgres",
                npgsql => npgsql.MigrationsAssembly(typeof(StockServiceDbContext).Assembly.FullName));

            return new StockServiceDbContext(optionsBuilder.Options);
        }
    }
}
