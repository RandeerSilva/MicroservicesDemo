namespace Order.Domain
{
    public class Order
    {
        public Guid Id { get; private set; }

        private Order() { }

        public static Order Create()
        {
            return new Order
            {
                Id = Guid.NewGuid()
            };
        }
    }
}
