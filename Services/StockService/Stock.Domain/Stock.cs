namespace Stock.Domain
{
    public class Stock
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        private Stock() { } // EF Core

        public Stock(Guid productId, int initialQuantity)
        {
            if (initialQuantity < 0)
                throw new ArgumentException("Initial quantity cannot be negative");

            ProductId = productId;
            Quantity = initialQuantity;
        }

        // 🔥 Business Rule: Reduce Stock
        public void Reduce(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            if (Quantity < quantity)
                throw new InvalidOperationException("Insufficient stock");

            Quantity -= quantity;
        }

        // Optional: Increase Stock
        public void Increase(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            Quantity += quantity;
        }
    }
}
