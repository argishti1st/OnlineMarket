using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineMarket.Api.Models;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities;
using OnlineMarket.Infrastructure.Data;

namespace OnlineMarket.Infrastructure.Repositiories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<int>> AddAsync(List<AddOrderApiModel> productQuantities, Guid userId, CancellationToken cancellationToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var productIds = productQuantities.Select(p => p.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync(cancellationToken);

                if (products.Count != productQuantities.Count)
                {
                    _logger.LogWarning("Some products were not found.");
                    return Result<int>.BadRequest("One or more products were not found.");
                }

                foreach (var pq in productQuantities)
                {
                    var product = products.FirstOrDefault(p => p.Id == pq.ProductId);
                    if (product == null || product.Quantity < pq.Quantity)
                    {
                        _logger.LogWarning("Product {ProductId} has less than {Quantity} left in stock.", pq.ProductId, pq.Quantity);
                        return Result<int>.BadRequest($"Product {pq.ProductId} has less than {pq.Quantity} left in stock.");
                    }
                }

                var order = new OrderDb();

                await _context.Orders.AddAsync(order, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var pq in productQuantities)
                {
                    var product = products.First(p => p.Id == pq.ProductId);

                    var orderProduct = new OrderProductDb
                    {
                        OrderId = order.Id,
                        ProductId = pq.ProductId,
                        Quantity = pq.Quantity
                    };

                    await _context.OrderProducts.AddAsync(orderProduct, cancellationToken);

                    product.Quantity -= pq.Quantity;
                }

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Order created successfully with ID: {OrderId}", order.Id);

                return Result<int>.Success(order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Transaction failed: {Error}", ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                return Result<int>.BadRequest("Failed to create order.");
            }
        }

        public async Task<OrderDb?> GetBySpecificationAsync(ISpecification<OrderDb> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<List<OrderDb>> ListBySpecificationAsync(ISpecification<OrderDb> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        private IQueryable<OrderDb> ApplySpecification(ISpecification<OrderDb> specification)
        {
            IQueryable<OrderDb> query = _context.Orders;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // ✅ Apply Includes
            foreach (var includeExpression in specification.Includes)
            {
                query = query.Include(includeExpression);
            }

            // ✅ Apply ThenInclude() (Nested Includes)
            foreach (var thenIncludeExpression in specification.ThenIncludeExpressions)
            {
                query = thenIncludeExpression(query);
            }

            return query;
        }
    }
}
