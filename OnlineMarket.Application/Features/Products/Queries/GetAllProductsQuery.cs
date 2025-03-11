using MediatR;
using OnlineMarket.Application.DtoS;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities;

namespace OnlineMarket.Application.Features.Products.Queries
{
    public record GetAllProductsQuery
        : IRequest<Result<IEnumerable<ProductDto>>>;
}
