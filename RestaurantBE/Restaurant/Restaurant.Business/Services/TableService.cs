
using Restaurant.Business.Responses;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapperService _mapperService;
        private readonly IOrderRepository _orderRepository;

        public TableService(ITableRepository tableRepository, IMapperService mapperService, IOrderRepository orderRepository)
        {
            _tableRepository = tableRepository;
            _mapperService = mapperService;
            _orderRepository = orderRepository;
        }

        public async Task<List<TableGeneralResponse>> GetAllTablesAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync(null, "Admin", 0, null, 0, 0, null, SortOptions.Ascending);
            var tables = await _tableRepository.GetAllTablesAsync();
            for (int i = 1; i <= tables.Count; i++)
            {
                var targetOrders = orders.Where(o => o.TableId == i);
                if (targetOrders.Count() != 0)
                {
                    foreach (var order in targetOrders)
                    {
                        order.Table = tables[i - 1];
                    }
                    var activeOrder = targetOrders.FirstOrDefault(o => o.Status == OrderStatus.Active);
                    if(activeOrder != null)
                    {
                        tables[i - 1].Orders = targetOrders.ToList();
                        tables[i - 1].Waiter = activeOrder.User;
                        tables[i - 1].WaiterId = activeOrder.UserId;
                    }
                }
            }
            var result = _mapperService.MapTableCollection(tables).ToList();
            return result;
        }
        public async Task<TableResponse> GetTableByIdAsync(int id)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(null, "Admin", 0, null, 0, 0, null, SortOptions.Ascending);
            var table = await _tableRepository.GetTableByIdAsync(id);
            
            var targetOrders = orders.Where(o => o.TableId == table.Id);
            if (targetOrders.Count() != 0)
            {
                foreach (var order in targetOrders)
                {
                    order.Table = table;
                }
                var activeOrder = targetOrders.FirstOrDefault(o => o.Status == OrderStatus.Active);
                if (activeOrder != null)
                {
                    table.Orders = targetOrders.ToList();
                    table.Waiter = activeOrder.User;
                    table.WaiterId = activeOrder.UserId;
                }
            }
            var result = _mapperService.MapTable(table);
            return result;
        }

        public async Task<MessageResponse> ClearAsync(int tableId)
        {
            var response = new MessageResponse();

            var table = await _tableRepository.GetTableByIdAsync(tableId);

            if (table == null)
            {
                return new MessageResponse(Messages.TableNotFound);
            }

            if (table.Status != TableStatus.Active)
            {
                return new MessageResponse(Messages.TableNotActive);
            }

            table.Status = TableStatus.Free;

            table.Orders.FirstOrDefault(o => o.Status == OrderStatus.Active).Status = OrderStatus.Complete;

            await _tableRepository.UpdateTableAsync(table);

            response = new MessageResponse(Messages.TableCleared, table.Id.ToString());

            return response;
        }
    }
}
