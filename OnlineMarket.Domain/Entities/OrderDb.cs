using OnlineMarket.Domain.Common;

namespace OnlineMarket.Domain.Entities
{
    public class OrderDb : AuditableEntity
    {
        public int Id { get; set; }
        public ICollection<OrderProductDb> OrderProducts { get; set; } = new List<OrderProductDb>();
    }
}
