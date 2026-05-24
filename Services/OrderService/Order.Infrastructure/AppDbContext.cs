using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Order> Orders => Set<Domain.Order>();
    }
}
