using DotNetCore.CAP;
using Shared.Contracts;
using Shared.Messaging.Cap;
using Stock.Application;

namespace Stock.Infrastructure.Handlers
{
    public sealed class StockSubscriber(IStockService stockService) : ICapSubscribe
    {
        private readonly IStockService _stockService = stockService;

        [CapSubscribe(CapTopics.OrderCreated)]
        public async Task<CapAck> Handle(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            foreach (var item in @event.Items)
            {
                await _stockService.ReduceStock(item.ProductId, item.Quantity);
            }

            return CapAck.Ok();
        }

        public sealed record CapAck(bool Success, string? Message = null)
        {
            public static CapAck Ok(string? message = null) => new(true, message);
            public static CapAck Fail(string message) => new(false, message);
        }
    }
}
