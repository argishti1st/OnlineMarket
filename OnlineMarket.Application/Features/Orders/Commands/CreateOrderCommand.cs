using MediatR;
using OnlineMarket.Api.Models;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Orders.Commands
{
    public record CreateOrderCommand(
        List<AddOrderApiModel> Products,
        Guid UserId) : IRequest<Result<int>>;
}
