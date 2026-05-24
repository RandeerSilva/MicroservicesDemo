namespace Stock.Application
{
    public interface IStockRepository 
    {
        Task<Domain.Stock?> GetByProductIdAsync(Guid productId ,CancellationToken cancellationToken = default);
        Task AddAsync(Domain.Stock stock, CancellationToken cancellationToken = default);
    }
}
