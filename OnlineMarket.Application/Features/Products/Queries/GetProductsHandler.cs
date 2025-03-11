using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMarket.Application.DtoS;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Domain.Common;

namespace OnlineMarket.Application.Features.Products.Queries
{
    public class GetProductsHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>,
        IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly ILogger<GetProductsHandler> _logger;
        private readonly IProductRepository _productRepository;

        public GetProductsHandler(ILogger<GetProductsHandler> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {request.ProductId} not found.");
                    return Result<ProductDto>.NotFound($"Product with ID {request.ProductId} not found.");
                }

                var productDto = new ProductDto(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Quantity,
                    product.Category);

                _logger.LogInformation($"Retrieved product with ID {request.ProductId}.");
                return Result<ProductDto>.Success(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching product with ID {request.ProductId}");
                return Result<ProductDto>.BadRequest("An error occurred while fetching the product.");
            }
        }

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                if (!products.Any())
                {
                    _logger.LogWarning("No products found.");
                    return Result<IEnumerable<ProductDto>>.NotFound("No products available.");
                }

                var productDtos = products.Select(product => new ProductDto(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Quantity,
                    product.Category));

                _logger.LogInformation($"Retrieved {productDtos.Count()} products.");
                return Result<IEnumerable<ProductDto>>.Success(productDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products.");
                return Result<IEnumerable<ProductDto>>.BadRequest("An error occurred while fetching the products.");
            }
        }
    }
}
