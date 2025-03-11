using MediatR;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Products.Commands
{
    public record DeleteProductCommand(
        int ProductId) : IRequest<Result<int>>;
}
