using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Infrastructure
{
    public class StockServiceDbContext(DbContextOptions<StockServiceDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Stock> Stocks => Set<Domain.Stock>();
    }
}
