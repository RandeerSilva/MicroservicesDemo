namespace Order.Application
{
    public interface IOrderRepository
    {
        Task AddAsync(Domain.Order order);
    }
}
