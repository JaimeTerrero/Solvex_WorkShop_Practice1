using Application.DTOs;
using Application.Repository;
using AutoMapper;
using Database;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductServices
    {
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductServices(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _productRepository = new(applicationDbContext);
            _mapper = mapper;
        }

        public async Task<Product> Add(ProductDto productResponse)
        {
            var product = _mapper.Map<Product>(productResponse);
            await _productRepository.AddAsync(product);
            return product;
        }

        public async Task Update(int id, ProductDto productResponse)
        {
            Product product = await _productRepository.GetByIdAsync(id);

            _mapper.Map(productResponse, product);

            await _productRepository.UpdateAsync(product);
        }

        public async Task Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            Product pr = new();
            pr.Id = product.Id;
            pr.Name = product.Name;
            pr.Details = product.Details;
            pr.Price = product.Price;

            return pr;
        }

        public async Task<List<Product>> GetAll()
        {
            var productList = await _productRepository.GetAllAsync();

            return productList;
        }
    }
}
