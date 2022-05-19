using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _productRepository.GetTotalProductsAsync();
        }

        public async Task<List<ProductResponse>> GetProductsOfCategoryAndSubcategoriesAsync(string? categoryId, int page, int pageSize)
        {
            var products = await _productRepository.GetProductsOfCategoryAndSubcategoriesAsync(categoryId, page, pageSize);
            var result = new List<ProductResponse>();
            foreach (var product in products)
            {
                result.Add(new ProductResponse(product));
            }
            return result;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(string productName, string categoryId, int page, int pageSize, string sortBy, SortOptions sortDirection)
        {
            var products = await _productRepository.GetAllProductsAsync(productName, categoryId, page, pageSize, sortBy, sortDirection);
            var result = new List<ProductResponse>();
            foreach (var product in products)
            {
                result.Add(new ProductResponse(product));
            }

            return result;
        }

        public async Task<ProductResponse> GetProductByIdAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            var result = new ProductResponse(product);
            return result;
        }

        public async Task<MessageResponse> CreateProductAsync(ProductCreateRequest request)
        {
            var targetCategory = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (targetCategory == null)
            {
                return new MessageResponse(Messages.ProductCategoryInvalid);
            }
            if (request.Description == null)
            {
                request.Description = "";
            }
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
            };
            await _productRepository.CreateProductAsync(product);
            return new MessageResponse(Messages.ProductCreated, product.Id);
        }

        public async Task<MessageResponse> UpdateProductAsync(string id, ProductUpdateRequest request)
        {
            var targetCategory = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
            if (request.CategoryId != null && targetCategory == null)
            {
                return new MessageResponse(Messages.ProductCategoryInvalid);
            }
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return new MessageResponse(Messages.ProductNotFound);
            }

            existingProduct.Name = request.Name ?? existingProduct.Name;
            existingProduct.Description = request.Description ?? existingProduct.Description;
            existingProduct.CategoryId = request.CategoryId ?? existingProduct.CategoryId;
            existingProduct.Price = request.Price ?? existingProduct.Price;

            await _productRepository.UpdateProductAsync(existingProduct);
            return new MessageResponse(Messages.ProductUpdated, existingProduct.Id);
        }

        public async Task<MessageResponse> DeleteProductAsync(string id)
        {
            var target = await _productRepository.GetProductByIdAsync(id);
            if (target == null)
            {
                return new MessageResponse(Messages.ProductNotFound);
            }
            await _productRepository.DeleteProductAsync(target);
            return new MessageResponse(Messages.ProductDeleted, id);
        }
    }
}
