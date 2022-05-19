
using Restaurant.Business.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Business.Services
{
    public interface ITableService
    {
        Task<List<TableGeneralResponse>> GetAllTablesAsync();
        Task<TableResponse> GetTableByIdAsync(int id);
        Task<MessageResponse> ClearAsync(int tableId);
    }
}
