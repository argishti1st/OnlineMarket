using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMarket.Application.Features.Interfaces;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities;

namespace OnlineMarket.Application.Features.Products.Commands
{
    public class CUDProductsHandler : IRequestHandler<CreateProductCommand, Result<int>>,
        IRequestHandler<UpdateProductCommand, Result<int>>,
        IRequestHandler<DeleteProductCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CUDProductsHandler> _logger;

        public CUDProductsHandler(IProductRepository productRepository, ILogger<CUDProductsHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = new ProductDb(
                    request.Name,
                    request.Description,
                    request.Price,
                    request.Quantity,
                    request.Category
                );

                var productId = await _productRepository.AddAsync(product);
                _logger.LogInformation($"Product created successfully: {productId}");

                return Result<int>.Success(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                return Result<int>.BadRequest("An error occurred while creating the product.");
            }
        }

        /// ✅ **Update Product**
        public async Task<Result<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(request.ProductId);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {request.ProductId} not found");
                    return Result<int>.NotFound($"Product with ID {request.ProductId} not found.");
                }

                existingProduct.Name = request.Name;
                existingProduct.Price = request.Price;
                existingProduct.Description = request.Description;
                existingProduct.Quantity = request.Quantity;
                existingProduct.Category = request.Category;

                await _productRepository.UpdateAsync(existingProduct);
                _logger.LogInformation($"Product updated successfully: {request.ProductId}");

                return Result<int>.Success(request.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product ID: {request.ProductId}");
                return Result<int>.BadRequest("An error occurred while updating the product.");
            }
        }

        /// ✅ **Delete Product**
        public async Task<Result<int>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(request.ProductId);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {request.ProductId} not found");
                    return Result<int>.NotFound($"Product with ID {request.ProductId} not found.");
                }

                await _productRepository.DeleteAsync(request.ProductId);
                _logger.LogInformation($"Product deleted successfully: {request.ProductId}");

                return Result<int>.Success(request.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product ID: {request.ProductId}");
                return Result<int>.BadRequest("An error occurred while deleting the product.");
            }
        }
    }
}
