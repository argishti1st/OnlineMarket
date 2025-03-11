using OnlineMarket.Application.Specifications;
using OnlineMarket.Domain.Entities;

public class ProductSpecification : BaseSpecification<ProductDb>
{
    public ProductSpecification(string name, decimal price)
        : base(p => (p.Name == name) && (p.Price == price))
    {
    }
}
