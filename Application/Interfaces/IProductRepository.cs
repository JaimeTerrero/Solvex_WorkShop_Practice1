using Application.DTOs;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(ProductDto entity);
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<List<Product>> GetProductByName(string name);
        Task UpdateAsync(int id, ProductDto entity);
    }
}