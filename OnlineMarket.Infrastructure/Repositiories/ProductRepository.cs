using Microsoft.EntityFrameworkCore;
using OnlineMarket.Application.Interfaces;
using OnlineMarket.Domain.Entities;
using OnlineMarket.Infrastructure.Data;

namespace OnlineMarket.Infrastructure.Repositiories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDb> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<ProductDb>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> AddAsync(ProductDb product)
        {
            var added = _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return added.Entity.Id;
        }

        public async Task UpdateAsync(ProductDb product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProductDb?> GetBySpecificationAsync(ISpecification<ProductDb> specification)
        {
            return await _context.Products
                .Where(specification.Criteria)
                .FirstOrDefaultAsync();
        }
    }
}
