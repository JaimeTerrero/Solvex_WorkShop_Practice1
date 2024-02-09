using Application.DTOs;
using Application.Repository;
using AutoMapper;
using Database;
using Microsoft.AspNetCore.Mvc;

namespace Solvex_Workshop1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;

        public ProductController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _productRepository = new(applicationDbContext, mapper);
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var product = await _productRepository.GetAllAsync();

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult> CreateProduct(ProductDto productDto)
        {
            var product = await _productRepository.AddAsync(productDto);

            return Ok(product);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            await _productRepository.UpdateAsync(id, productDto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}