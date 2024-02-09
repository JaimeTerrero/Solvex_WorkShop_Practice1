using Application.DTOs;
using Application.Interfaces;
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
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var product = await _productRepository.GetAllAsync();

            return Ok(product);
        }

        [HttpGet("GetProductByName")]
        public async Task<ActionResult> GetProductByName(string name)
        {
            var product = await _productRepository.GetProductByName(name);

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