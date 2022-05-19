using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Contracts;
using Restaurant.Data.Paging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;

namespace Restaurant.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private UserManager<User> _userManager;

        public OrderRepository(AppDbContext appDbContext, UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            int total = await _appDbContext.Orders.CountAsync();
            return total;
        }

        public async Task<List<Order>> GetAllOrdersAsync(string? userName, string role, int tableId, OrderStatus? status, int page, int pageSize, string? sortBy, SortOptions? sortDirection)
        {
            IQueryable<Order> filteredOrders = _appDbContext.Orders.Include(x => x.User).Include(x => x.OrderProducts).ThenInclude(x => x.Product);

            if (role == "Waiter")
            {
                User targetUser = await _userManager.Users.FirstOrDefaultAsync(u => (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(userName.ToLower()));
                filteredOrders = filteredOrders.Where(o => o.UserId == targetUser.Id);
            }
            else if (role == "Admin")
            {
                if (userName != null)
                {
                    filteredOrders = filteredOrders.Where(o => (o.User.FirstName.ToLower() + " " + o.User.LastName.ToLower()).Contains(userName.ToLower()));
                }
            }

            if (status != null)
            {
                filteredOrders = filteredOrders.Where(o => o.Status == status);
            }

            if (tableId != 0) {
                filteredOrders = filteredOrders.Where(o => o.TableId == tableId);
            }

            if (sortBy != null)
            {
                if (sortBy.ToLower() == "username")
                {
                    if (sortDirection != null)
                    {
                        if (sortDirection == SortOptions.Ascending)
                        {
                            filteredOrders = filteredOrders.OrderBy(o => o.User.FirstName).ThenBy(o => o.User.LastName).ThenBy(o => o.TableId);
                        }
                        else if (sortDirection == SortOptions.Descending)
                        {
                            filteredOrders = filteredOrders.OrderByDescending(o=> o.User.FirstName).ThenByDescending(o => o.User.LastName).ThenByDescending(o => o.TableId);
                        }
                    }
                }
                else if (sortBy.ToLower() == "table")
                {
                    if (sortDirection != null)
                    {
                        if (sortDirection == SortOptions.Ascending)
                        {
                            filteredOrders = filteredOrders.OrderBy(o => o.TableId).ThenBy(o => o.User.FirstName).ThenBy(o => o.User.LastName).ThenBy(o => o.TableId);
                        }
                        else if (sortDirection == SortOptions.Descending)
                        {
                            filteredOrders = filteredOrders.OrderByDescending(o => o.TableId).ThenByDescending(o => o.User.FirstName).ThenByDescending(o => o.User.LastName).ThenByDescending(o => o.TableId);
                        }
                    }
                }
                else if (sortBy.ToLower() == "date")
                {
                    if (sortDirection != null)
                    {
                        if (sortDirection == SortOptions.Ascending)
                        {
                            filteredOrders = filteredOrders.OrderBy(o => o.Date).ThenBy(o => o.User.FirstName).ThenBy(o => o.User.LastName).ThenBy(o => o.TableId);
                        }
                        else if (sortDirection == SortOptions.Descending)
                        {
                            filteredOrders = filteredOrders.OrderByDescending(o => o.Date).ThenByDescending(o => o.User.FirstName).ThenByDescending(o => o.User.LastName).ThenByDescending(o => o.TableId);
                        }
                    }
                }
                else if (sortBy.ToLower() == "order")
                {
                    if (sortDirection != null)
                    {
                        if (sortDirection == SortOptions.Ascending)
                        {
                            filteredOrders = filteredOrders.OrderBy(o => o.Id).ThenBy(o => o.User.FirstName).ThenBy(o => o.User.LastName).ThenBy(o => o.TableId);
                        }
                        else if (sortDirection == SortOptions.Descending)
                        {
                            filteredOrders = filteredOrders.OrderByDescending(o => o.Id).ThenByDescending(o => o.User.FirstName).ThenByDescending(o => o.User.LastName).ThenByDescending(o => o.TableId);
                        }
                    }
                }
            }

            if (page != 0 && pageSize != 0)
            {
                filteredOrders = new Paginator<Order>().Paginate(filteredOrders, page, pageSize);
            }

            return await filteredOrders.ToListAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            IQueryable<Order> AllOrders = _appDbContext.Orders.Include(x => x.User).Include(x => x.OrderProducts).ThenInclude(x => x.Product);
            return await AllOrders.FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task CreateOrderAsync(Order order)
        {
            _appDbContext.Orders.Add(order);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _appDbContext.Orders.Update(order);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
