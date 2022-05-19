using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Api.Extensions;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;
using System.Security.Claims;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private UserManager<User> _userManager;

        public OrdersController(IOrderService orderService, UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [HttpGet("total"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetTotalOrdersAsync()
        {
            int total = await _orderService.GetTotalOrdersAsync();
            var result = new TotalResponse(total);
            return Ok(result);
        }

        [HttpGet, Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetAllOrdersAsync(string? userName,
                                                        int tableId,
                                                        OrderStatus? status,
                                                        int page,
                                                        int pageSize,
                                                        string? sortBy,
                                                        SortOptions? sortDirection)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserRole = ClaimsPrincipalExtensions.GetRole(currentUser);

            if (currentUserRole == "Waiter")
            {
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                User targetUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentUserID);
                userName = targetUser.UserName;
            }

            var result = await _orderService.GetAllOrdersAsync(userName, currentUserRole, tableId, status, page, pageSize, sortBy, sortDirection);
            return Ok(result);
        }

        [HttpGet("{orderId}"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int orderId)
        {
            var targetOrder = await _orderService.GetOrderByIdAsync(orderId);
            if (targetOrder == null)
            {
                return NotFound(Messages.OrderNotFound);
            }
            var result = await _orderService.GetOrderByIdAsync(orderId);

            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> CreateOrderAsync(OrderCreateRequest request)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _orderService.CreateOrderAsync(currentUserID, request);

            return Ok(response);
        }

        [HttpPut("{orderId}"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> UpdateOrderAsync([FromRoute] int orderId, OrderUpdateRequest request)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserRole = ClaimsPrincipalExtensions.GetRole(currentUser);
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _orderService.UpdateOrderAsync(currentUserRole, currentUserID, orderId, request);

            return Ok(response);
        }

        [HttpPut("{orderId}/Complete"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> CompleteOrderAsync([FromRoute] int orderId)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserRole = ClaimsPrincipalExtensions.GetRole(currentUser);
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _orderService.CompleteOrderByIdAsync(currentUserRole, currentUserID, orderId);

            return Ok(response);
        }

        [HttpDelete("{orderId}"), Authorize(Roles = "Admin,Waiter")]
        public async Task<IActionResult> DeleteOrderAsync([FromRoute] int orderId)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserRole = ClaimsPrincipalExtensions.GetRole(currentUser);
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _orderService.DeleteOrderAsync(currentUserRole, currentUserID, orderId);
            return Ok(response);
        }
    }
}
