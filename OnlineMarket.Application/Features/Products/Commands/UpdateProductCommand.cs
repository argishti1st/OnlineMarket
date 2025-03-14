﻿using MediatR;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Products.Commands
{
    public record UpdateProductCommand(
        int ProductId,
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        string Category) : IRequest<Result<int>>;
}
