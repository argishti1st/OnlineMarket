using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineMarket.Application.Features.Products.Commands;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnlineMarket.Tests.FeatureTests.Commands
{
    public class CreateProductCommandTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ILogger<CUDProductsHandler>> _loggerMock;
        private readonly CUDProductsHandler _handler;

        public CreateProductCommandTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<CUDProductsHandler>>();

            _handler = new CUDProductsHandler(_productRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenProductAlreadyExists()
        {
            // Arrange
            var request = new CreateProductCommand("Laptop", "Gaming Laptop", 1500, 5, "Electronics");

            var existingProduct = new ProductDb("Laptop", "Gaming Laptop", 1500, 5, "Electronics") { Id = 1 };

            _productRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ProductSpecification>()))
                .ReturnsAsync(existingProduct);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product 'Laptop' with price 1500 already exists.");
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenNewProductIsAdded()
        {
            // Arrange
            var request = new CreateProductCommand("Laptop", "Gaming Laptop", 1500, 5, "Electronics");

            _productRepositoryMock.Setup(repo => repo.GetBySpecificationAsync(It.IsAny<ProductSpecification>()))
                .ReturnsAsync((ProductDb)null);

            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ProductDb>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ProductDb>()), Times.Once);
        }
    }
}
