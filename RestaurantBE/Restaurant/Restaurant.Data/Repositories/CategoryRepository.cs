using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var allCategories = await _appDbContext.Categories.ToListAsync();
            return allCategories;
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            var result = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return result;
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            var result = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
            return result;
        }

        public async Task AddCategoryAsync(Category category)
        {
            _appDbContext.Add(category);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _appDbContext.Update(category);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(Category category)
        {
            _appDbContext.Remove(category);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
