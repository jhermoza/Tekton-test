using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync(ProductRequest request);
        Task<bool> UpdateAsync(int id, ProductRequest request);
        Task<ProductResponse?> GetByIdAsync(int id);
    }
}
