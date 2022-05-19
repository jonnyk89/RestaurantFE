using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<IOrderRepository> _orderRepository;
        private Mock<ITableRepository> _tableRepository;
        private Mock<IMapperService> _mapperService;
        private OrderService _orderService;

        [TestInitialize]
        public void Initialize()
        {
            _productRepository = new Mock<IProductRepository>();
            _orderRepository = new Mock<IOrderRepository>();
            _tableRepository = new Mock<ITableRepository>();
            _mapperService = new Mock<IMapperService>();
            _orderService = new OrderService(_orderRepository.Object, _productRepository.Object, _tableRepository.Object, _mapperService.Object);
        }

        [TestMethod]
        public async Task GetAllOrdersAsync_AllOrders_ShouldReturnListOfOrders()
        {
            // Arrange
            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            List<OrderGeneralResponse> responses = new List<OrderGeneralResponse>()
            {
                new OrderGeneralResponse()
                {
                    Id = orders[0].Id,
                    TableId = orders[0].TableId,
                    Waiter = "koko",
                    DateTime = "10",
                    Status = "OPA",
                    Price = orders[0].Price,
                    Products = new List<OrderProductResponse>(),
                },
                new OrderGeneralResponse()
                {
                    Id = orders[1].Id,
                    TableId = orders[1].TableId,
                    Waiter = "koko",
                    DateTime = "10",
                    Status = "OPA",
                    Price = orders[1].Price,
                    Products = new List<OrderProductResponse>(),
                },
            };

            _orderRepository.Setup(x => x.GetAllOrdersAsync(null, "Admin", 0, OrderStatus.Active, 0, 0, null, SortOptions.Ascending))
                .ReturnsAsync(orders);

            _mapperService.Setup(x => x.MapOrderCollection(orders))
                .Returns(responses);


            // Act
            var actualResult = await _orderService.GetAllOrdersAsync(null, "Admin", 0, OrderStatus.Active, 0, 0, null, SortOptions.Ascending);

            // Assert
            Assert.AreEqual(orders.Count, actualResult.Count, "Should match.");
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_ValidId_ShouldReturnOrder()
        {
            // Arrange
            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            _mapperService.Setup(x => x.MapOrder(orders[0]))
                .Returns(response);


            // Act
            var actualResult = await _orderService.GetOrderByIdAsync(orders[0].Id);

            // Assert
            Assert.AreEqual(response.Price, actualResult.Price, "Should match.");
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_InvalidId_ShouldNotReturnOrder()
        {
            // Arrange
            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            _mapperService.Setup(x => x.MapOrder(orders[0]))
                .Returns(response);


            // Act
            var actualResult = await _orderService.GetOrderByIdAsync(123);

            // Assert
            Assert.AreEqual(null, actualResult, "Should match.");
        }

        [TestMethod]
        public async Task CreateOrderAsync_ValidInput_ShouldOrderTableInvalidIdMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            var request = new OrderCreateRequest()
            {
                TableId = 1,
                Products = new List<OrderProductRequest>(),
            };

            _tableRepository.Setup(x => x.GetAllTablesAsync())
                .ReturnsAsync(tables);

            _tableRepository.Setup(x => x.GetTableByIdAsync(1))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _orderService.CreateOrderAsync(users[0].Id, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderCreated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task CreateOrderAsync_InvalidTableId_ShouldOrderTableInvalidIdMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            var request = new OrderCreateRequest()
            {
                TableId = 10,
                Products = new List<OrderProductRequest>(),
            };


            _tableRepository.Setup(x => x.GetAllTablesAsync())
                .ReturnsAsync(tables);

            _tableRepository.Setup(x => x.GetTableByIdAsync(1))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _orderService.CreateOrderAsync(users[0].Id, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderTableInvalidId).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateOrderAsync_ValidInput_ShouldOrderUpdatedMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            var request = new OrderUpdateRequest()
            {
                UserId = users[0].Id,
                TableId = 10,
                Products = new List<OrderProductRequest>(),
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            // Act
            var actualResult = await _orderService.UpdateOrderAsync("Admin", users[0].Id, orders[0].Id, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderUpdated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateOrderAsync_InvalidOrderId_ShouldOrderNotFoundMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            var request = new OrderUpdateRequest()
            {
                UserId = users[0].Id,
                TableId = 10,
                Products = new List<OrderProductRequest>(),
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            // Act
            var actualResult = await _orderService.UpdateOrderAsync("Admin", users[0].Id, 1234, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderNotFound).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task CompleteOrderByIdAsync_ValidInput_ShouldOrderCompletedMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Active,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _orderService.CompleteOrderByIdAsync("Admin", users[0].Id, orders[0].Id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderCompleted).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task CompleteOrderByIdAsync_OrderAlreadyCompleted_ShouldOrderAlreadyCompletedMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _orderService.CompleteOrderByIdAsync("Admin", users[0].Id, orders[0].Id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderAlreadyCompleted).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeleteOrderAsync_ValidId_ShouldOrderDeletedMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _orderService.DeleteOrderAsync("Admin", users[0].Id, orders[0].Id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderDeleted).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeleteOrderAsync_InvalidId_ShouldOrderNotFoundMessageResponse()
        {
            // Arrange
            List<User> users = new List<User>()
            {
                new User
                {
                    Email = "admin@mentormate.com",
                    FirstName = "Koko",
                    LastName = "Boko",
                    Role = UserRole.Admin,
                    UserName = "Koko",
                    Picture = null,
                },
            };

            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "oranges",
                },
                new Category()
                {
                    Name = "potatoes",
                },
            };
            List<Product> products = new List<Product>()
            {
                new Product
                {
                    Name = "orange",
                    Price = 10,
                    CategoryId = categories[0].Id,
                    Category = categories[0],
                },
                new Product
                {
                    Name = "potato",
                    Price = 10,
                    CategoryId = categories[1].Id,
                    Category = categories[1],
                },
            };

            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Free,
                    Capacity = 10,
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                }
            };

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 1,
                    Table = tables[0],
                    OrderProducts = new List<OrderProduct>(),
                },
                new Order()
                {
                    Status = OrderStatus.Complete,
                    Date = DateTime.Now,
                    Price = 10,
                    TableId = 2,
                    Table = tables[1],
                    OrderProducts = new List<OrderProduct>(),
                },
            };

            var response = new OrderResponse()
            {
                Id = orders[0].Id,
                TableId = orders[0].TableId,
                Waiter = "koko",
                Status = orders[0].Status.ToString(),
                Price = orders[0].Price,
                Products = new List<OrderProductResponse>(),
            };

            var newOrder = new Order()
            {
                Status = OrderStatus.Complete,
                Date = DateTime.Now,
                Price = 10,
                TableId = 1,
                Table = tables[0],
                OrderProducts = new List<OrderProduct>()
                {
                    new OrderProduct()
                    {
                        OrderId = orders[0].Id,
                        Order = orders[0],
                        ProductId = products[0].Id,
                        Product = products[0],
                        ProductQuantity = 10,
                        ProductPrice = 100,
                    }
                },
            };

            _orderRepository.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(orders[0]);

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _orderService.DeleteOrderAsync("Admin", users[0].Id, 1234);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.OrderNotFound).Message, actualResult.Message, "Should match.");
        }
    }
}
