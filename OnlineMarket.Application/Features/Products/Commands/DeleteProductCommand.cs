using MediatR;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities.Identity;

namespace OnlineMarket.Application.Features.Products.Commands
{
    public record DeleteProductCommand(
        int ProductId) : IRequest<Result<int>>;
}
