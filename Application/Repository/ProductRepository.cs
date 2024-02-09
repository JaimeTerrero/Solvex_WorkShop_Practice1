using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<Product> AddAsync(ProductDto productdto)
        {
            var product = _mapper.Map<Product>(productdto);

            await _applicationDbContext.Products.AddAsync(product);
            await _applicationDbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _applicationDbContext.Products.FindAsync(id);
            _applicationDbContext.Set<Product>().Remove(product);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _applicationDbContext.Set<Product>().ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Product product = await _applicationDbContext.Set<Product>().FindAsync(id);

            Product pr = new();
            pr.Id = product.Id;
            pr.Name = product.Name;
            pr.Details = product.Details;
            pr.Price = product.Price;

            return pr;
        }

        public async Task UpdateAsync(int id, ProductDto productDto)
        {
            Product product = await _applicationDbContext.Set<Product>().FindAsync(id);

            _mapper.Map(productDto, product);

            _applicationDbContext.Entry(product).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}