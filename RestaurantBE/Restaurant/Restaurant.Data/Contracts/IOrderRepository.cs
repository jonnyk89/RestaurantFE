using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Entities.Enums;

namespace Restaurant.Data.Contracts
{
    public interface IOrderRepository
    {
        Task<int> GetTotalOrdersAsync();
        Task<List<Order>> GetAllOrdersAsync(string? userName, string role, int tableId, OrderStatus? status, int page, int pageSize, string? sortBy, SortOptions? sortDirection);
        Task<Order> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
    }
}
