using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Contracts
{
    public interface ITableRepository
    {
        Task<List<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int id);

        Task UpdateTableAsync(Table table);
    }
}
