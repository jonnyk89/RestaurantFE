
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Services
{
    public interface IProductService
    {
        Task<int> GetTotalProductsAsync();
        Task<List<ProductResponse>> GetProductsOfCategoryAndSubcategoriesAsync(string? categoryId, int page, int pageSize);
        Task<List<ProductResponse>> GetAllProductsAsync(string product, string categoryId, int page, int pageSize, string sortBy, SortOptions sortDirection);
        Task<ProductResponse> GetProductByIdAsync(string id);
        Task<MessageResponse> CreateProductAsync(ProductCreateRequest product);
        Task<MessageResponse> UpdateProductAsync(string id, ProductUpdateRequest request);
        Task<MessageResponse> DeleteProductAsync(string id);
    }
}
