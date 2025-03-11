using OnlineMarket.Domain.Common;
using Microsoft.Extensions.Logging;
using MediatR;
using OnlineMarket.Application.Features.Orders.Commands;
using OnlineMarket.Application.Interfaces;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<int>>
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IOrderRepository repository, ILogger<CreateOrderCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        return await _repository.AddAsync(request.Products, request.UserId, cancellationToken);
    }
}
