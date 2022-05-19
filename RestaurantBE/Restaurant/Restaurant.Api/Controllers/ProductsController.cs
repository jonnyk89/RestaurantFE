using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("total"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetTotalProductsAsync()
        {
            var total = await _productService.GetTotalProductsAsync();
            var result = new TotalResponse(total);
            return Ok(result);
        }

        [HttpGet("category"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetProductsOfCategoryAndSubcategoriesAsync(string? categoryId, int page, int pageSize)
        {
            var result = await _productService.GetProductsOfCategoryAndSubcategoriesAsync(categoryId, page, pageSize);
            return Ok(result);
        }

        [HttpGet, Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetAllProductsAsync(string? product, string? categoryId, int page, int pageSize, string? sortBy, SortOptions sortDirection)
        {
            var result = await _productService.GetAllProductsAsync(product, categoryId, page, pageSize, sortBy, sortDirection);
            return Ok(result);
        }

        [HttpGet("{productId}"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] string productId)
        {
            var targetProduct = await _productService.GetProductByIdAsync(productId);
            if (targetProduct == null)
            {
                return NotFound(Messages.ProductNotFound);
            }

            return Ok(targetProduct);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProductAsync(ProductCreateRequest request)
        {
            var response = await _productService.CreateProductAsync(request);

            return Ok(response);
        }

        [HttpPut("{productId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] string productId, ProductUpdateRequest request)
        {
            var response = await _productService.UpdateProductAsync(productId, request);

            return Ok(response);
        }

        [HttpDelete("{productId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] string productId)
        {
            var response = await _productService.DeleteProductAsync(productId);

            return Ok(response);
        }
    }
}
