using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Stock.Application;

namespace Stock.Infrastructure.Repositories
{
    public class StockRepository(StockServiceDbContext context) : IStockRepository
    {
        public async Task<Domain.Stock?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await context.Stocks
                .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        }

        public async Task AddAsync(Domain.Stock stock, CancellationToken cancellationToken = default)
        {
            await context.Stocks.AddAsync(stock, cancellationToken);
        }
    }
}
