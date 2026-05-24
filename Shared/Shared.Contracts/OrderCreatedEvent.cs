
namespace Shared.Contracts
{
    public record OrderCreatedEvent(
        Guid OrderId,
        List<OrderItem> Items
    );

    public record OrderItem(
        Guid ProductId,
        int Quantity
    );
}
