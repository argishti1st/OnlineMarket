using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMarket.Application.Interfaces;
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
            _logger.LogInformation("Checking if product already exists: {Name}, {Price}", request.Name, request.Price);

            var specification = new ProductSpecification(request.Name, request.Price);
            var existingProduct = await _productRepository.GetBySpecificationAsync(specification);

            if (existingProduct != null)
            {
                _logger.LogWarning("Product already exists: {ProductId}", existingProduct.Id);
                return Result<int>.BadRequest($"Product '{request.Name}' with price {request.Price} already exists.");
            }

            var newProduct = new ProductDb(request.Name, request.Description, request.Price, request.Quantity, request.Category);
            await _productRepository.AddAsync(newProduct);

            _logger.LogInformation("Product added successfully: {ProductId}", newProduct.Id);
            return Result<int>.Success(newProduct.Id);
        }

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
