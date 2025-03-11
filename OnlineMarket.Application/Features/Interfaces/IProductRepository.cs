using OnlineMarket.Domain.Entities;

namespace OnlineMarket.Application.Features.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductDb> GetByIdAsync(int id);
        Task<IEnumerable<ProductDb>> GetAllAsync();
        Task<int> AddAsync(ProductDb product);
        Task UpdateAsync(ProductDb product);
        Task DeleteAsync(int id);
    }
}
