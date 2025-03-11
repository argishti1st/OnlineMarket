using MediatR;
using OnlineMarket.Application.DtoS;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Orders.Queries
{
    public record GetOrderByIdQuery(int Id) : IRequest<Result<OrderDto>>;
}
