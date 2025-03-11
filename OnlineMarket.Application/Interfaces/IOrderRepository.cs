using OnlineMarket.Api.Models;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities;

namespace OnlineMarket.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderDb?> GetBySpecificationAsync(ISpecification<OrderDb> specification);
        Task<List<OrderDb>> ListBySpecificationAsync(ISpecification<OrderDb> specification);

        Task<Result<int>> AddAsync(List<AddOrderApiModel> productQuantities, Guid userId, CancellationToken cancellationToken);
    }
}
