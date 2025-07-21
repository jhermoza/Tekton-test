using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Application.DTOs;

namespace ProductApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">Product data to create</param>
        /// <returns>The newly created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid product data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            var response = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.ProductId }, response);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">Unique product identifier</param>
        /// <param name="request">Product data to update</param>
        /// <returns>No content if update is successful</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid product data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            var updated = await _service.UpdateAsync(id, request);
            if (!updated)
                return NotFound(new { message = $"Product with ID {id} was not found." });
            return NoContent();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">Unique product identifier</param>
        /// <returns>The product details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponse>> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            if (response == null)
                return NotFound(new { message = $"Product with ID {id} was not found." });
            return Ok(response);
        }
    }
}
