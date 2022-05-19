using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;

namespace Restaurant.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapperService _mapperService;

        public CategoryService(ICategoryRepository categoryRepository, IMapperService mapperService)
        {
            _categoryRepository = categoryRepository;
            _mapperService = mapperService;
        }

        public async Task<List<GeneralCategoryResponse>> GetAllCategoriesAsync()
        {
            List<Category> allCategories = await _categoryRepository.GetAllCategoriesAsync();
            var topCategories = allCategories.Where(c => c.ParentId == null);
            var childrenByParentId = allCategories.Where(c => c.ParentId != null).ToLookup(c => c.ParentId);
            var result = await _mapperService.MapCategoryCollection(topCategories, childrenByParentId);
            return result.ToList();
        }
        public async Task<List<GeneralCategoryResponse>> GetCategoryByIdAsync(string id)
        {
            List<Category> allCategories = await _categoryRepository.GetAllCategoriesAsync();
            var topCategories = allCategories.Where(c => c.Id == id);
            var childrenByParentId = allCategories.Where(c => c.ParentId != null).ToLookup(c => c.ParentId);
            var result = await _mapperService.MapCategoryCollection(topCategories, childrenByParentId);
            return result.ToList();
        }
        public async Task<MessageResponse> AddCategoryAsync(CategoryCreateRequest request)
        {
            var targetCategoryByName = await _categoryRepository.GetCategoryByNameAsync(request.Name);
            var targetCategoryByParentId = await _categoryRepository.GetCategoryByIdAsync(request.ParentId);
            
            if (targetCategoryByName != null)
            {
                return new MessageResponse(Messages.CategoryExists);
            }

            if (request.ParentId != null && targetCategoryByParentId == null)
            {
                return new MessageResponse(Messages.CategoryParentInvalidId);
            }

            var category = new Category()
            {
                 Name = request.Name,
                 ParentId = request.ParentId
            };

            await _categoryRepository.AddCategoryAsync(category);

            return new MessageResponse(Messages.CategoryCreated, category.Id);
        }
        public async Task<MessageResponse> UpdateCategoryAsync(string id, CategoryUpdateRequest request)
        {
            var targetCategoryByName = await _categoryRepository.GetCategoryByNameAsync(request.Name);
            var targetCategoryByParentId = await _categoryRepository.GetCategoryByIdAsync(request.ParentId);
            var targetCategory = await _categoryRepository.GetCategoryByIdAsync(id);

            if (targetCategory == null)
            {
                return new MessageResponse(Messages.CategoryNotFound);
            }

            if (targetCategoryByName != null)
            {
                return new MessageResponse(Messages.CategoryExists);
            }

            if ((request.ParentId != null && targetCategoryByParentId == null))
            {
                return new MessageResponse(Messages.CategoryParentInvalidId);
            }

            targetCategory.Name = request.Name ?? targetCategory.Name;
            targetCategory.ParentId = request.ParentId;

            await _categoryRepository.UpdateCategoryAsync(targetCategory);

            return new MessageResponse(Messages.CategoryUpdated, id);
        }
        public async Task<MessageResponse> DeleteCategoryAsync(string id)
        {
            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                return new MessageResponse(Messages.CategoryNotFound);
            }

            await _categoryRepository.DeleteCategoryAsync(existingCategory);

            return new MessageResponse(Messages.CategoryDeleted, id);
        }
    }
}
