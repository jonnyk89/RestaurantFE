using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;

namespace Restaurant.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITableRepository _tableRepository;
        private IMapperService _mapperService;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ITableRepository tableRepository, IMapperService mapperService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _tableRepository = tableRepository;
            _mapperService = mapperService;
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _orderRepository.GetTotalOrdersAsync();
        }

        public async Task<List<OrderGeneralResponse>> GetAllOrdersAsync(string? userName, string role, int tableId, OrderStatus? status, int page, int pageSize, string? sortBy, SortOptions? sortDirection)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(userName, role, tableId, status, page, pageSize, sortBy, sortDirection);
            var result = _mapperService.MapOrderCollection(orders).ToList();
            return result;
        }
        public async Task<OrderResponse> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            var result = _mapperService.MapOrder(order);
            return result;
        }
        public async Task<MessageResponse> CreateOrderAsync(string UserId, OrderCreateRequest request)
        {
            var tables = await _tableRepository.GetAllTablesAsync();
            var orders = await _orderRepository.GetAllOrdersAsync(null, "Admin", 0, null, 0, 0, null, SortOptions.Ascending);
            if (!tables.Any(t => t.Id == request.TableId))
            {
                return new MessageResponse(Messages.OrderTableInvalidId);
            }
            if (orders.Any(o => o.TableId == request.TableId && o.Status == OrderStatus.Active))
            {
                return new MessageResponse(Messages.OrderTableOccupied);
            }
            var order = new Order();
            order.Status = OrderStatus.Active;
            order.UserId = UserId;
            order.Date = DateTime.Now;
            var orderProducts = new List<OrderProduct>();
            foreach (var p in request.Products)
            {
                var product = await _productRepository.GetProductByIdAsync(p.productId);
                if (product == null)
                {
                    return new MessageResponse(Messages.OrderInvalidProductId, p.productId);
                }
                var op = new OrderProduct();
                op.OrderId = order.Id;
                op.Order = order;
                op.ProductId = p.productId;
                op.Product = product;
                op.ProductQuantity = p.quantity;
                op.ProductPrice = product.Price * p.quantity;
                orderProducts.Add(op);
            }
            order.OrderProducts = orderProducts;

            order.TableId = request.TableId;
            var table = await _tableRepository.GetTableByIdAsync(request.TableId);
            table.Status = TableStatus.Active;
            table.WaiterId = order.UserId;
            order.Table = table;

            await _orderRepository.CreateOrderAsync(order);
            return new MessageResponse(Messages.OrderCreated);
        }

        public async Task<MessageResponse> UpdateOrderAsync(string CurrentUserRole, string CurrentUserID, int id, OrderUpdateRequest request)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return new MessageResponse(Messages.OrderNotFound);
            }
            if (CurrentUserRole != "Admin" && existingOrder.UserId != CurrentUserID)
            {
                return new MessageResponse(Messages.OrderAccessDenied);
            }

            existingOrder.UserId = request.UserId ?? existingOrder.UserId;
            existingOrder.TableId = request.TableId ?? existingOrder.TableId;

            if (request.Products != null)
            {
                var orderProducts = new List<OrderProduct>();
                foreach (var p in request.Products)
                {
                    var product = await _productRepository.GetProductByIdAsync(p.productId);
                    var op = new OrderProduct();
                    op.OrderId = existingOrder.Id;
                    op.Order = existingOrder;
                    op.ProductId = p.productId;
                    op.Product = product;
                    op.ProductQuantity = p.quantity;
                    op.ProductPrice = product.Price * p.quantity;
                    orderProducts.Add(op);
                }
                existingOrder.OrderProducts = orderProducts;
            }

            await _orderRepository.UpdateOrderAsync(existingOrder);
            return new MessageResponse(Messages.OrderUpdated);
        }

        public async Task<MessageResponse> CompleteOrderByIdAsync(string CurrentUserRole, string CurrentUserID, int id)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return new MessageResponse(Messages.OrderNotFound);
            }
            if (CurrentUserRole != "Admin" && existingOrder.UserId != CurrentUserID)
            {
                return new MessageResponse(Messages.OrderAccessDenied);
            }
            if (existingOrder.Status == OrderStatus.Complete)
            {
                return new MessageResponse(Messages.OrderAlreadyCompleted);
            }

            existingOrder.Status = OrderStatus.Complete;
            var table = await _tableRepository.GetTableByIdAsync(existingOrder.TableId);
            table.Status = TableStatus.Free;

            await _orderRepository.UpdateOrderAsync(existingOrder);
            return new MessageResponse(Messages.OrderCompleted);
        }

        public async Task<MessageResponse> DeleteOrderAsync(string CurrentUserRole, string CurrentUserID, int id)
        {
            var target = await _orderRepository.GetOrderByIdAsync(id);
            if (target == null)
            {
                return new MessageResponse(Messages.OrderNotFound);
            }
            if (CurrentUserRole != "Admin" && target.UserId != CurrentUserID)
            {
                return new MessageResponse(Messages.OrderAccessDenied);
            }

            await _orderRepository.DeleteOrderAsync(target);
            return new MessageResponse(Messages.OrderDeleted);
        }
    }
}
