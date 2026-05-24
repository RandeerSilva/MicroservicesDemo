using Order.Application;
namespace Order.Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public async Task AddAsync(Order.Domain.Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
    }
}
