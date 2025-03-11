using MediatR;
using OnlineMarket.Application.DtoS;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Products.Queries
{
    public record GetAllProductsQuery
        : IRequest<Result<IEnumerable<ProductDto>>>;
}
