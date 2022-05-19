using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Api.Extensions;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet, Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetAllTablesAsync()
        {
            var result = await _tableService.GetAllTablesAsync();

            return Ok(result);
        }

        [HttpGet("{tableId}"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetTableByIdAsync([FromRoute] int tableId)
        {
            var result = await _tableService.GetTableByIdAsync(tableId);

            return Ok(result);
        }

        //[HttpPut("{tableId}/Clear")]
        //public async Task<IActionResult> ClearTableAsync(int tableId)
        //{
        //    var response = await _tableService.ClearAsync(tableId);

        //    return Ok(response);
        //}
    }
}
