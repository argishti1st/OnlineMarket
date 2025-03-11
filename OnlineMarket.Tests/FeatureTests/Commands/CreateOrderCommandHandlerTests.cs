using Moq;
using Microsoft.Extensions.Logging;
using OnlineMarket.Application.Features.Orders.Commands;
using OnlineMarket.Domain.Entities;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Api.Models;
using Xunit;
using OnlineMarket.Domain.Common;
using OnlineMarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ILogger<CreateOrderCommandHandler>> _loggerMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
        _handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrderIsCreated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateOrderCommand(new List<AddOrderApiModel>
        {
            new AddOrderApiModel(1, 2)
        }, userId);

        _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<List<AddOrderApiModel>>(), userId, CancellationToken.None))
            .ReturnsAsync(Result<int>.Success(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
        _orderRepositoryMock.Verify(repo => repo.AddAsync(command.Products, command.UserId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOrderCreationFails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateOrderCommand(new List<AddOrderApiModel>
        {
            new AddOrderApiModel(1, 2)
        }, userId);

        _orderRepositoryMock.Setup(repo => repo.AddAsync(command.Products, command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<int>.BadRequest("Failed to create order."));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Failed to create order.");
        _orderRepositoryMock.Verify(repo => repo.AddAsync(command.Products, command.UserId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
