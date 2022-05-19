using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Api.Extensions;
using Restaurant.Domain.Entities.MessageResponses;
using Restaurant.Business.Requests;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoeyService)
        {
            _categoryService = categoeyService;
        }

        [HttpGet, Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result);
        }

        // Not needed?
        [HttpGet("{id}"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetCategoryByIdAsync(string id)
        {
            var targetCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (targetCategory == null)
            {
                return BadRequest(Messages.CategoryNotFound);
            }

            return Ok(targetCategory);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategoryAsync(CategoryCreateRequest request)
        {
            var response = await _categoryService.AddCategoryAsync(request);

            return Ok(response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory([FromRoute] string id, CategoryUpdateRequest request)
        {
            var response = await _categoryService.UpdateCategoryAsync(id, request);

            return Ok(response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);

            return Ok(response);
        }

    }
}
