using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.Initialization;

namespace Restaurant.Data
{
    public class DbInitializer
    {
        private readonly AppDbContext _appDbContext;
        private UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext appDbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await ApplyMigrationsAsync();
            await SeedAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            var pendingMigrations = await _appDbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await _appDbContext.Database.MigrateAsync();
            }
        }

        private async Task SeedAsync()
        {
            await SeedCategoriesAsync();
            await SeedProductsAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedTablesAsync();
            await SeedOrdersAsync();
            await SeedOrderProductsAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (await _appDbContext.Roles.AnyAsync())
            {
                return;
            }

            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Waiter"));
        }

        private async Task SeedUsersAsync()
        {
            if (await _appDbContext.Users.AnyAsync())
            {
                return;
            }

            var adminUser = new User
            {
                Email = "admin@mentormate.com",
                FirstName = "John",
                LastName = "Doe",
                Role = UserRole.Admin,
                UserName = "admin",
                Picture = null,
            };

            await _userManager.CreateAsync(adminUser, "admin12345");
            await _userManager.AddToRoleAsync(adminUser, "Admin");

            var user1 = new User
            {
                Email = "jonny@mentormate.com",
                FirstName = "Jonny",
                LastName = "Bravo",
                Role = UserRole.Waiter,
                UserName = "jonny",
                Picture = null,
            };

            await _userManager.CreateAsync(user1, "jonny12345");
            await _userManager.AddToRoleAsync(user1, "Waiter");

            var user2 = new User
            {
                Email = "mia@mentormate.com",
                FirstName = "Mia",
                LastName = "Farrow",
                Role = UserRole.Waiter,
                UserName = "miafarrow",
                Picture = null,
            };

            await _userManager.CreateAsync(user2, "mia12345");
            await _userManager.AddToRoleAsync(user2, "Waiter");

            var user3 = new User
            {
                Email = "danny@mentormate.com",
                FirstName = "Danny",
                LastName = "DeVito",
                Role = UserRole.Waiter,
                UserName = "danny",
                Picture = null,
            };

            await _userManager.CreateAsync(user3, "danny12345");
            await _userManager.AddToRoleAsync(user3, "Waiter");

            var user4 = new User
            {
                Email = "ken@mentormate.com",
                FirstName = "Ken",
                LastName = "Allen",
                Role = UserRole.Waiter,
                UserName = "ken",
                Picture = null,
            };

            await _userManager.CreateAsync(user4, "ken12345");
            await _userManager.AddToRoleAsync(user4, "Waiter");

            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedCategoriesAsync()
        {
            if (await _appDbContext.Categories.AnyAsync())
            {
                return;
            }

            var categories = FoodCategories.GetCategories();

            await _appDbContext.Categories.AddRangeAsync(categories);
            await _appDbContext.SaveChangesAsync();

        }

        private async Task SeedProductsAsync()
        {
            if (await _appDbContext.Products.AnyAsync())
            {
                return;
            }

            var categories = await _appDbContext.Categories.ToListAsync();

            Dictionary<string, string> categoryDictionary = categories.ToDictionary(x => x.Name, x => x.Id);


            var products = FoodProducts.GetProducts(categoryDictionary);

            await _appDbContext.Products.AddRangeAsync(products);

            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedOrderProductsAsync()
        {
            if (await _appDbContext.OrderProducts.AnyAsync())
            {
                return;
            }

            var rnd = new Random();

            var dbOrders = await _appDbContext.Orders.ToListAsync();

            var orderProducts = new List<OrderProduct>();

            foreach (var order in dbOrders)
            {
                var count = rnd.Next(1, 6);

                for (int j = 0; j < count; j++)
                {
                    var productsCount = _appDbContext.Products.Count();

                    var product = _appDbContext.Products.ToList().OrderBy(x => Guid.NewGuid()).First();

                    var productCount = rnd.Next(1, 4);

                    if (!orderProducts.Any(x => x.ProductId == product.Id && x.OrderId == order.Id))
                    {
                        var op = new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            ProductQuantity = productCount,
                            ProductPrice = product.Price,
                        };

                        orderProducts.Add(op);
                    }
                }
            }

            _appDbContext.OrderProducts.AddRange(orderProducts);
            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedTablesAsync()
        {
            if (await _appDbContext.Tables.AnyAsync())
            {
                return;
            }

            var waiterId = _userManager.GetUsersInRoleAsync("Waiter").Result.FirstOrDefault().Id;

            var tables = new List<Table>();

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                tables.Add(new Table
                {
                    Capacity = rnd.Next(2, 13),
                    Status = TableStatus.Free,
                    WaiterId = null,
                    Orders = new List<Order>(),
                });
            }

            await _appDbContext.Tables.AddRangeAsync(tables);
            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedOrdersAsync()
        {
            if (await _appDbContext.Orders.AnyAsync())
            {
                return;
            }

            var rnd = new Random();

            var orders = new List<Order>();

            //var tables = _appDbContext.Tables.ToList();

            for (int i = 1; i <= 4; i++)
            {
                var tableId = 1;
                while (orders.Any(o => o.TableId == tableId))
                {
                    tableId = rnd.Next(1, 11);
                }

                var waiterId = _userManager.GetUsersInRoleAsync("Waiter").Result[i - 1].Id;

                var order = new Order
                {
                    TableId = tableId,
                    UserId = waiterId,
                    User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == waiterId),
                    Date = new DateTime(2022, 1, i),
                };

                orders.Add(order);
            }

            //for (int i = 0; i < tables.Count; i++)
            //{
            //    var targetOrder = orders.FirstOrDefault(o => o.TableId == tables[i].Id);
            //    if (targetOrder != null)
            //    {
            //        tables[i].WaiterId = targetOrder.UserId;
            //        _appDbContext.Tables.Update(tables[i]);
            //    }
            //}

            await _appDbContext.Orders.AddRangeAsync(orders);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
