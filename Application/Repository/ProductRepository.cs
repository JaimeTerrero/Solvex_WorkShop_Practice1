using Application.Interfaces;
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
    public class ProductRepository : IGenericRepository<Product>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _applicationDbContext.Products.AddAsync(product);
            await _applicationDbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            _applicationDbContext.Set<Product>().Remove(product);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _applicationDbContext.Set<Product>().ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Set<Product>().FindAsync(id);
        }

        public async Task UpdateAsync(Product product)
        {
            _applicationDbContext.Entry(product).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
