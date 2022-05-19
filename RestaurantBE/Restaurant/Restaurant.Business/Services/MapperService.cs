using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Business.Responses;
using Restaurant.Data.Contracts;

namespace Restaurant.Business.Services
{
    public class MapperService : IMapperService
    {
        public async Task<IEnumerable<GeneralCategoryResponse>> MapCategoryCollection(IEnumerable<Category> categories, ILookup<string?, Category> childrenByParentId)
        {
            var result = new List<GeneralCategoryResponse>();
            foreach (var category in categories.OrderBy(c => c.Name))
            {
                var categoryModel = new GeneralCategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    Subcategories = (await MapCategoryCollection(childrenByParentId[category.Id], childrenByParentId)).ToList()
                };
                result.Add(categoryModel);
            }

            return result;
        }

        public IEnumerable<OrderGeneralResponse> MapOrderCollection(IEnumerable<Order> orders) =>
            orders.Select(x => new OrderGeneralResponse
            {
                Id = x.Id,
                TableId = x.TableId,
                Waiter = x.User.FirstName + " " + x.User.LastName,
                DateTime = x.Date.ToString("dddd, dd MMMM yyyy"),
                Status = x.Status.ToString(),
                Price = x.OrderProducts.Select(x => x.Product.Price * x.ProductQuantity).Sum(),
                Products = x.OrderProducts.Select(x => new OrderProductResponse
                {
                    Name = x.Product.Name,
                    Quantity = x.ProductQuantity,
                    Price = x.Product.Price,
                    TotalPrice = x.Product.Price * x.ProductQuantity,
                }).ToList(),
            }).ToList();

        public OrderResponse MapOrder(Order order) =>
            new OrderResponse
            {
                Id = order.Id,
                TableId = order.TableId,
                Waiter = order.User.FirstName + " " + order.User.LastName,
                Status = order.Status.ToString(),
                Price = order.OrderProducts.Select(x => x.Product.Price * x.ProductQuantity).Sum(),
                Products = order.OrderProducts.Select(x => new OrderProductResponse
                {
                    Name = x.Product.Name,
                    Quantity = x.ProductQuantity,
                    Price = x.Product.Price,
                    TotalPrice = x.Product.Price * x.ProductQuantity,
                }).ToList(),
            };

        public IEnumerable<TableGeneralResponse> MapTableCollection(IEnumerable<Table> tables) =>
            tables.Select(x => new TableGeneralResponse
            {
                Id = x.Id,
                Status = x.Orders.Any(o => o.Status == OrderStatus.Active) == false ? TableStatus.Free.ToString() : TableStatus.Active.ToString(),
                Capacity = x.Capacity,
                Waiter = x.Orders.Any(o => o.Status == OrderStatus.Active) == true
                ? x.Waiter.FirstName + " " + x.Waiter.LastName
                : "N/A",
                Bill = x.Orders.Any(o => o.Status == OrderStatus.Active) == true
                ? x.Orders.FirstOrDefault(o => o.Status == OrderStatus.Active).OrderProducts.Select(v => v.Product.Price * v.ProductQuantity).Sum().ToString() + " BGN"
                : "0 BGN",
            }).ToList();

        public TableResponse MapTable(Table table) =>
            new TableResponse
            {
                Id = table.Id,
                Status = table.Orders.Any(o => o.Status == OrderStatus.Active) == false ? TableStatus.Free.ToString() : TableStatus.Active.ToString(),
                Capacity = table.Capacity,
                Waiter = table.Orders.Any(o => o.Status == OrderStatus.Active) == true
                ? table.Orders.FirstOrDefault(o => o.Status == OrderStatus.Active).User.FirstName + " " + table.Orders.FirstOrDefault(o => o.Status == OrderStatus.Active).User.LastName
                : "N/A",
                Bill = table.Orders.Any(o => o.Status == OrderStatus.Active) == true
                ? table.Orders.FirstOrDefault(o => o.Status == OrderStatus.Active).OrderProducts.Select(v => v.Product.Price * v.ProductQuantity).Sum().ToString() + " BGN"
                : "0 BGN",
                Orders = table.Orders.Select(x => new OrderResponse
                {
                    Id = x.Id,
                    TableId = x.TableId,
                    Waiter = x.User.FirstName + " " + x.User.LastName,
                    Status = x.Status.ToString(),
                    Price = x.OrderProducts.Select(x => x.ProductPrice * x.ProductQuantity).Sum(),
                    Products = x.OrderProducts.Select(x => new OrderProductResponse
                    {
                        Name = x.Product.Name,
                        Quantity = x.ProductQuantity,
                        Price = x.ProductPrice,
                        TotalPrice = x.ProductPrice * x.ProductQuantity,
                    }).ToList(),
                }).ToList(),
            };
    }
}
