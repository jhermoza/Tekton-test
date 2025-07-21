using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int productId);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
    }
}
