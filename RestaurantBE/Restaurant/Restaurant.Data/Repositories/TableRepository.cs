using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly AppDbContext _appDbContext;

        public TableRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Table>> GetAllTablesAsync()
            => await _appDbContext.Tables
            .Include(x => x.Waiter)
            .Include(x => x.Orders.Where(o => o.Status == OrderStatus.Active))
            .ThenInclude(x => x.OrderProducts)
            .ThenInclude(x => x.Product)
            .ToListAsync();

        public async Task<Table> GetTableByIdAsync(int id)
            => await _appDbContext.Tables
            .Include(x => x.Waiter)
            .Include(x => x.Orders.Where(o => o.Status == OrderStatus.Active))
            .ThenInclude(x => x.OrderProducts)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateTableAsync(Table table)
        {
            _appDbContext.Tables.Update(table);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
