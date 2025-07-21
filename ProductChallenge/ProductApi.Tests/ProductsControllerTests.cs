using Xunit;
using Moq;
using ProductApi.API.Controllers;
using ProductApi.Application.Interfaces;
using ProductApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProductApi.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _service = new();
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _controller = new ProductsController(_service.Object);
        }

        [Fact]
        public async Task Create_ReturnsCreated_WhenProductIsValid()
        {
            var request = new ProductRequest { Name = "Test", Status = 1, Stock = 10, Description = "Desc", Price = 100 };
            var response = new ProductResponse { ProductId = 1, Name = "Test", Status = 1, StatusName = "Active", Stock = 10, Description = "Desc", Price = 100, Discount = 10, FinalPrice = 90 };
            _service.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);
            var result = await _controller.Create(request);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, created.StatusCode);
            Assert.Equal(response, created.Value);
        }


        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var request = new ProductRequest { Name = "Test", Status = 1, Stock = 10, Description = "Desc", Price = 100 };
            _service.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(true);
            var result = await _controller.Update(1, request);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenUpdateFails()
        {
            var request = new ProductRequest { Name = "Test", Status = 1, Stock = 10, Description = "Desc", Price = 100 };
            _service.Setup(s => s.UpdateAsync(1, request)).ReturnsAsync(false);
            var result = await _controller.Update(1, request);
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenProductExists()
        {
            var response = new ProductResponse { ProductId = 1, Name = "Test", Status = 1, StatusName = "Active", Stock = 10, Description = "Desc", Price = 100, Discount = 10, FinalPrice = 90 };
            _service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(response);
            var result = await _controller.GetById(1);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(response, ok.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _service.Setup(s => s.GetByIdAsync(2)).ReturnsAsync((ProductResponse?)null);
            var result = await _controller.GetById(2);
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFound.StatusCode);
        }
    }
}
