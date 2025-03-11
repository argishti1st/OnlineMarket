using OnlineMarket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OnlineMarket.Application.Specifications
{
    public class GetOrdersSpecification : BaseSpecification<OrderDb>
    {
        public GetOrdersSpecification(int? orderId = null)
        : base(o => (!orderId.HasValue || o.Id == orderId))
        {
            AddInclude(o => o.OrderProducts);
            AddThenInclude(o => o.Include(o => o.OrderProducts)
                                  .ThenInclude(op => op.Product));
        }
    }
}
