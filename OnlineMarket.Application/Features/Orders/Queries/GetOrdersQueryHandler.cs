using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMarket.Application.DtoS;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Application.Specifications;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Orders.Queries
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Result<IEnumerable<OrderDto>>>,
        IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly ILogger<GetOrdersQueryHandler> _logger;
        private readonly IOrderRepository _repository;

        public GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IOrderRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetOrdersSpecification(request.Id);
            var order = await _repository.GetBySpecificationAsync(specification);

            if (order is null)
            {
                _logger.LogWarning("No orders found");
                return Result<OrderDto>.NotFound($"No order found with Id: {request.Id}.");
            }

            var orderDto = new OrderDto(
                order.Id,
                order.CreatedAt,
                order.CreatedBy,
                order.IsDeleted,
                order.OrderProducts.Select(op => new ProductDto(
                    op.Product.Id,
                    op.Product.Name,
                    op.Product.Description,
                    op.Product.Price,
                    op.Quantity,
                    op.Product.Category)));

            return Result<OrderDto>.Success(orderDto);
        }

        public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetOrdersSpecification();
            var orders = await _repository.ListBySpecificationAsync(specification);

            if (orders.Count == 0)
            {
                _logger.LogWarning("No orders found");
                return Result<IEnumerable<OrderDto>>.NotFound("No orders found.");
            }

            var orderDtos = orders.Select(o => new OrderDto(
                o.Id,
                o.CreatedAt,
                o.CreatedBy,
                o.IsDeleted,
                o.OrderProducts.Select(op => new ProductDto(
                    op.Product.Id,
                    op.Product.Name,
                    op.Product.Description,
                    op.Product.Price,
                    op.Quantity,
                    op.Product.Category))));

            return Result<IEnumerable<OrderDto>>.Success(orderDtos);
        }
    }
}
