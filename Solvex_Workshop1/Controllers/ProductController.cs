using Application.DTOs;
using Application.Services;
using AutoMapper;
using Database;
using Microsoft.AspNetCore.Mvc;

namespace Solvex_Workshop1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductServices _productServices;

        public ProductController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _productServices = new(applicationDbContext, mapper);
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var product = await _productServices.GetAll();

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult> CreateProduct(ProductDto productDto)
        {
            var product = await _productServices.Add(productDto);

            return Ok(product);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _productServices.GetById(id);

            return Ok(product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            await _productServices.Update(id, productDto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productServices.Delete(id);

            return NoContent();
        }
    }
}
