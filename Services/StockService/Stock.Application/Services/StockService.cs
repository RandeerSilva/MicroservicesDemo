namespace Stock.Application.Services
{
    public class StockService(IStockRepository repository) : IStockService
    {
        public async Task ReduceStock(Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var stock = await repository.GetByProductIdAsync(productId, cancellationToken);

            if (stock == null)
            {
                throw new InvalidOperationException($"Stock not found for product {productId}");
            }

            stock.Reduce(quantity);

            await repository.AddAsync(stock, cancellationToken);
        }
    }
}
