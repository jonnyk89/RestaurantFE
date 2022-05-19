using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
namespace Restaurant.Business.Services
{
    public interface IOrderService
    {
        Task<int> GetTotalOrdersAsync();
        Task<List<OrderGeneralResponse>> GetAllOrdersAsync(string? userName, string role, int tableId, OrderStatus? status, int page, int pageSize, string? sortBy, SortOptions? sortDirection);
        Task<OrderResponse> GetOrderByIdAsync(int id);
        Task<MessageResponse> CreateOrderAsync(string UserId, OrderCreateRequest request);
        Task<MessageResponse> UpdateOrderAsync(string CurrentUserRole, string CurrentUserID, int id, OrderUpdateRequest request);
        Task<MessageResponse> CompleteOrderByIdAsync(string CurrentUserRole, string CurrentUserID, int id);
        Task<MessageResponse> DeleteOrderAsync(string CurrentUserRole, string CurrentUserID, int id);
    }
}
