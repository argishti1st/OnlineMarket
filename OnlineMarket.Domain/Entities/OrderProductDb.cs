namespace OnlineMarket.Domain.Entities
{
    public class OrderProductDb
    {
        public int OrderId { get; set; }
        public OrderDb Order { get; set; }

        public int ProductId { get; set; }
        public ProductDb Product { get; set; }

        public int Quantity { get; set; }
    }

}
