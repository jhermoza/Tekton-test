using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using AutoMapper;

namespace ProductApi.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repo = new();
        private readonly Mock<IDiscountService> _discountService = new();
        private readonly Mock<IStatusCache> _statusCache = new();
        private readonly IMapper _mapper;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductRequest, Product>();
                cfg.CreateMap<Product, ProductResponse>();
            });
            _mapper = config.CreateMapper();
            _service = new ProductService(_repo.Object, _discountService.Object, _statusCache.Object, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnProductResponse_WithBasicFields()
        {
            var request = new ProductRequest { Name = "Test", Status = 1, Stock = 10, Description = "Desc", Price = 200 };
            _repo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _statusCache.Setup(s => s.GetStatusNameAsync(1)).ReturnsAsync("Active");
            _discountService.Setup(d => d.GetDiscountPercentageAsync(It.IsAny<int>())).ReturnsAsync(10);
            var response = await _service.CreateAsync(request);
            Assert.NotNull(response);
            Assert.Equal("Test", response.Name);
            Assert.Equal(1, response.Status);
            Assert.Equal("Active", response.StatusName);
            Assert.Equal(10, response.Stock);
            Assert.Equal("Desc", response.Description);
            Assert.Equal(200, response.Price);
            Assert.Equal(10, response.Discount);
            Assert.Equal(180, response.FinalPrice);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenProductExists()
        {
            var product = new Product { ProductId = 1, Name = "Test", Status = 1, Stock = 10, Description = "Desc", Price = 100 };
            _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _repo.Setup(r => r.UpdateAsync(product)).Returns(Task.CompletedTask);
            var request = new ProductRequest { Name = "Updated", Status = 0, Stock = 5, Description = "New", Price = 50 };
            var result = await _service.UpdateAsync(1, request);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            _repo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Product?)null);
            var request = new ProductRequest { Name = "Updated", Status = 0, Stock = 5, Description = "New", Price = 50 };
            var result = await _service.UpdateAsync(2, request);
            Assert.False(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProductResponse_WhenProductExists()
        {
            var product = new Product { ProductId = 1, Name = "Test", Status = 1, Stock = 10, Description = "Desc", Price = 100 };
            _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _statusCache.Setup(s => s.GetStatusNameAsync(1)).ReturnsAsync("Active");
            _discountService.Setup(d => d.GetDiscountPercentageAsync(1)).ReturnsAsync(10);
            var response = await _service.GetByIdAsync(1);
            Assert.NotNull(response);
            Assert.Equal(1, response.ProductId);
            Assert.Equal("Test", response.Name);
            Assert.Equal(10, response.Discount);
            Assert.Equal("Active", response.StatusName);
            Assert.Equal(90, response.FinalPrice);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            _repo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Product?)null);
            var response = await _service.GetByIdAsync(2);
            Assert.Null(response);
        }
    }
}
