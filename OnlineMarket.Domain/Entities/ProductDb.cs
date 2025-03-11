namespace OnlineMarket.Domain.Entities
{
    public class ProductDb
    {
        public ProductDb()
        {
        }

        public ProductDb(string name, string description, decimal price, int quantity, string category)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Category { get; set; }

        public ICollection<OrderProductDb> OrderProducts { get; set; } = new List<OrderProductDb>();
    }
}
