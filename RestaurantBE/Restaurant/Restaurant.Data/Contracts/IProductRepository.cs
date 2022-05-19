using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Contracts
{
    public interface IProductRepository
    {
        Task<int> GetTotalProductsAsync();
        Task<List<Product>> GetProductsOfCategoryAndSubcategoriesAsync(string? categoryId, int page, int pageSize);
        Task<List<Product>> GetAllProductsAsync(string productName, string categoryId, int page, int pageSize, string sortBy, SortOptions sortDirection);
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
