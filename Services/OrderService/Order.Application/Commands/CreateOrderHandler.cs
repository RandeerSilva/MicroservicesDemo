using Microsoft.Extensions.Logging;
using Shared.Contracts;
using Shared.Messaging.Abstractions;
using Shared.Messaging.Cap;

namespace Order.Application.Commands
{
    public class CreateOrderHandler(IOrderRepository repo, IEventBus eventBus, ILogger<CreateOrderHandler> logger)
    {
        public async Task<Guid> Handle()
        {
            var order = Order.Domain.Order.Create();

            await repo.AddAsync(order);

            var items = new List<OrderItem>
            {
                new(Guid.NewGuid(), 2)
            };

            try
            {
                await eventBus.PublishAsync(CapTopics.OrderCreated,
                    new OrderCreatedEvent(order.Id, items));
            }
            catch (Exception ex) when (ex.Message.Contains("No responders are available for the request", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogError(ex, "Failed to publish 'order.created'. Ensure a subscriber/consumer is running and configured for this subject.");
                throw new InvalidOperationException("Event publish failed because no responders/consumers are available for 'order.created'.", ex);
            }

            return order.Id;
        }
    }
}
