

using Restaurant.Business.Requests;
using Restaurant.Business.Responses;

namespace Restaurant.Business.Services
{
    public interface ICategoryService
    {
        Task<List<GeneralCategoryResponse>> GetAllCategoriesAsync();
        Task<List<GeneralCategoryResponse>> GetCategoryByIdAsync(string id);
        Task<MessageResponse> AddCategoryAsync(CategoryCreateRequest request);
        Task<MessageResponse> UpdateCategoryAsync(string id, CategoryUpdateRequest request);
        Task<MessageResponse> DeleteCategoryAsync(string id);
    }
}
