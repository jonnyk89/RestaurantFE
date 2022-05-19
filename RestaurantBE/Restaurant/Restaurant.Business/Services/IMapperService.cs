using Restaurant.Business.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Business.Services
{
    public interface IMapperService
    {
        Task<IEnumerable<GeneralCategoryResponse>> MapCategoryCollection(IEnumerable<Category> categories, ILookup<string?, Category> childrenByParentId);
        IEnumerable<OrderGeneralResponse> MapOrderCollection(IEnumerable<Order> orders);
        OrderResponse MapOrder(Order order);
        IEnumerable<TableGeneralResponse> MapTableCollection(IEnumerable<Table> tables);
        TableResponse MapTable(Table table);
    }
}
