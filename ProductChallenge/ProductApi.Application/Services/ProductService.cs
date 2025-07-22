using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using AutoMapper;

namespace ProductApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IDiscountService _discountService;
        private readonly IStatusCache _statusCache;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IDiscountService discountService, IStatusCache statusCache, IMapper mapper)
        {
            _repo = repo;
            _discountService = discountService;
            _statusCache = statusCache;
            _mapper = mapper;
        }

        public async Task<ProductResponse> CreateAsync(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            await _repo.AddAsync(product);
            var response = _mapper.Map<ProductResponse>(product);
            response.StatusName = await _statusCache.GetStatusNameAsync(product.Status);
            response.Discount = await _discountService.GetDiscountPercentageAsync(product.ProductId);
            response.FinalPrice = product.Price - (product.Price * response.Discount / 100);
            return response;
        }

        public async Task<bool> UpdateAsync(int id, ProductRequest request)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;
            _mapper.Map(request, product);
            await _repo.UpdateAsync(product);
            return true;
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return null;
            var response = _mapper.Map<ProductResponse>(product);
            response.StatusName = await _statusCache.GetStatusNameAsync(product.Status);
            response.Discount = await _discountService.GetDiscountPercentageAsync(product.ProductId);
            response.FinalPrice = product.Price - (product.Price * response.Discount / 100);
            return response;
        }
    }
}
