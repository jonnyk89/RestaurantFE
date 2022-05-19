using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Contracts;
using Restaurant.Data.Paging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
//using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> GetTotalProductsAsync()
        {
            int total = await _appDbContext.Products.CountAsync();
            return total;
        }

        public async Task<List<Product>> GetProductsOfCategoryAndSubcategoriesAsync(string? categoryId, int page, int pageSize)
        {
            List<Product> filteredProducts = await _appDbContext.Products.ToListAsync();
            List<Category> categories = await _appDbContext.Categories.ToListAsync();

            if (categoryId == null)
            {
                filteredProducts = GetAllSubProducts(categories, filteredProducts, new List<Product>()).ToList();
            }
            else
            {
                var topCategories = categories.Where(c => c.Id == categoryId).ToList();
                var childrenByParentId = categories.Where(c => c.ParentId != null).ToLookup(c => c.ParentId);
                List<Category> subCategories = GetAllSubCategories(topCategories, childrenByParentId);

                filteredProducts = GetAllSubProducts(subCategories, filteredProducts, new List<Product>());
            }

            if (page != 0 && pageSize != 0)
            {
                filteredProducts = new Paginator<Product>().Paginate(filteredProducts, page, pageSize);
            }

            return filteredProducts;
        }

        public async Task<List<Product>> GetAllProductsAsync(string productName, string categoryId, int page, int pageSize, string sortBy, SortOptions sortDirection)
        {
            IQueryable<Product> filteredProducts = _appDbContext.Products.Include(x => x.Category);
            const string category = "category";
            const string product = "product";

            if (productName != null)
            {
                filteredProducts = filteredProducts.Where(p => p.Name.Contains(productName));
            }

            if (categoryId != null)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == categoryId);
            }

            if (sortBy != null)
            {
                if (sortBy.Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    if (sortDirection == SortOptions.Ascending)
                    {
                        filteredProducts = filteredProducts.OrderBy(p => p.Category.Name).ThenBy(p => p.Name);
                    }
                    else if (sortDirection == SortOptions.Descending)
                    {
                        filteredProducts = filteredProducts.OrderByDescending(p => p.Category.Name).ThenByDescending(p => p.Name);
                    }
                }
                else if (sortBy.Equals(product, StringComparison.OrdinalIgnoreCase))
                {
                    if (sortDirection == SortOptions.Ascending)
                    {
                        filteredProducts = filteredProducts.OrderBy(p => p.Name).ThenBy(p => p.Category);
                    }
                    else if (sortDirection == SortOptions.Descending)
                    {
                        filteredProducts = filteredProducts.OrderByDescending(p => p.Name).ThenByDescending(p => p.Category);
                    }
                }
            }
            else
            {
                if (sortDirection == SortOptions.Ascending)
                {
                    filteredProducts = filteredProducts.OrderBy(p => p.Id);
                }
                else if (sortDirection == SortOptions.Descending)
                {
                    filteredProducts = filteredProducts.OrderByDescending(p => p.Id);
                }
            }

            if (page != 0 && pageSize != 0)
            {
                filteredProducts = new Paginator<Product>().Paginate(filteredProducts, page, pageSize);
            }

            return await filteredProducts.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = await _appDbContext.Products.Include(x => x.Category).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task CreateProductAsync(Product product)
        {
            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(Product product)
        {
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
        }

        private List<Product> GetAllSubProducts(List<Category> categories, List<Product> products, List<Product> subProducts)
        {
            if (categories.Count() > 0 && products.Count() > 0)
            {
                foreach (var category in categories)
                {
                    var targetProducts = products.Where(p => p.CategoryId == category.Id);
                    if (targetProducts.Count() > 0)
                    {
                        foreach (var product in targetProducts)
                        {
                            subProducts.Add(product);
                            products = products.Where(p => p != product).ToList();
                        }
                    }
                    GetAllSubProducts(category.Children.ToList(), products, subProducts);
                }
            }
            return subProducts;
        }

        private List<Category> GetAllSubCategories(List<Category> categories, ILookup<string?, Category> childrenByParentId)
        {
            var result = new List<Category>();
            if (categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    category.Children = GetAllSubCategories(childrenByParentId[category.Id].ToList(), childrenByParentId);
                    result.Add(category);
                }
            }
            return result;
        }

        //public async Task<IEnumerable<GeneralCategoryResponse>> MapCategoryCollection(IEnumerable<Category> categories, ILookup<string?, Category> childrenByParentId)
        //{
        //    var result = new List<GeneralCategoryResponse>();
        //    foreach (var category in categories.OrderBy(c => c.Name))
        //    {
        //        var categoryModel = new GeneralCategoryResponse
        //        {
        //            Id = category.Id,
        //            Name = category.Name,
        //            Subcategories = (await MapCategoryCollection(childrenByParentId[category.Id], childrenByParentId)).ToList()
        //        };
        //        result.Add(categoryModel);
        //    }

        //    return result;
        //}
    }
}
