using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Application
{
    public interface IStockService
    {
        Task ReduceStock(Guid productId, int quantity ,CancellationToken cancellationToken = default);
    }
}
