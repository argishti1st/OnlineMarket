using OnlineMarket.Domain.Common;

namespace OnlineMarket.Domain.Entities
{
    public class OrderDb : AuditableEntity
    {
        public int Id { get; set; }
        public ICollection<ProductDb> Products { get; set; } = new List<ProductDb>();
    }
}
