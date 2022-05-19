using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Restaurant.Api.Controllers;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerTests
    {
        private Mock<IOrderService> _orderService;
        private UserManager<User> _userManager;
        private OrdersController _ordersController;

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        private List<User> _users = new List<User>
        {
            new User(),
            new User(),
        };

        [TestInitialize]
        public void Initialize()
        {
            _orderService = new Mock<IOrderService>();
             _userManager = MockUserManager<User>(_users).Object; 
            _ordersController = new OrdersController(_orderService.Object, _userManager);
        }

        [TestMethod]
        public async Task GetAllOrdersAsync_AllOrders_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.GetAllOrdersAsync(null, "Admin", 0, OrderStatus.Active, 0, 0, null, SortOptions.Ascending))
                .ReturnsAsync(responses);

            // Act
            var actualResult = await _ordersController.GetAllOrdersAsync(null, 0, OrderStatus.Active, 0, 0, null, SortOptions.Ascending) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_ValidId_ShouldReturnStatusCode200()
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

            _orderService.Setup(x => x.GetOrderByIdAsync(orders[0].Id))
                .ReturnsAsync(response);

            // Act
            var actualResult = await _ordersController.GetOrderByIdAsync(orders[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task GetOrderByIdAsync_InvalidId_ShouldReturnNull()
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


            // Act
            var actualResult = await _ordersController.GetOrderByIdAsync(123) as OkObjectResult;

            // Assert
            Assert.IsNull(actualResult, "Should return null.");
        }

        [TestMethod]
        public async Task CreateOrderAsync_ValidInput_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var request = new OrderCreateRequest()
            {
                TableId = 1,
                Products = new List<OrderProductRequest>(),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.CreateOrderAsync(currentUserID, request))
                .ReturnsAsync(new MessageResponse(Messages.OrderCreated));

            // Act
            var actualResult = await _ordersController.CreateOrderAsync(request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task CreateOrderAsync_InvalidTableId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var request = new OrderCreateRequest()
            {
                TableId = 10,
                Products = new List<OrderProductRequest>(),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.CreateOrderAsync(currentUserID, request))
                .ReturnsAsync(new MessageResponse(Messages.OrderTableInvalidId));

            // Act
            var actualResult = await _ordersController.CreateOrderAsync(request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task UpdateOrderAsync_ValidId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var request = new OrderUpdateRequest()
            {
                TableId = 1,
                Products = new List<OrderProductRequest>(),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.UpdateOrderAsync("Admin", currentUserID, orders[0].Id, request))
                .ReturnsAsync(new MessageResponse(Messages.OrderCreated));

            // Act
            var actualResult = await _ordersController.UpdateOrderAsync(orders[0].Id, request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task UpdateOrderAsync_InvalidId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var request = new OrderUpdateRequest()
            {
                TableId = 1,
                Products = new List<OrderProductRequest>(),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.UpdateOrderAsync("Admin", currentUserID, 123, request))
                .ReturnsAsync(new MessageResponse(Messages.OrderNotFound));

            // Act
            var actualResult = await _ordersController.UpdateOrderAsync(123, request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task CompleteOrderAsync_ValidId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.CompleteOrderByIdAsync("Admin", currentUserID, orders[0].Id))
                .ReturnsAsync(new MessageResponse(Messages.OrderDeleted));

            // Act
            var actualResult = await _ordersController.CompleteOrderAsync(orders[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task CompleteOrderAsync_InvalidId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.CompleteOrderByIdAsync("Admin", currentUserID, 123))
                .ReturnsAsync(new MessageResponse(Messages.OrderNotFound));

            // Act
            var actualResult = await _ordersController.CompleteOrderAsync(123) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task DeleteOrderAsync_ValidId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.DeleteOrderAsync("Admin", currentUserID, orders[0].Id))
                .ReturnsAsync(new MessageResponse(Messages.OrderDeleted));

            // Act
            var actualResult = await _ordersController.DeleteOrderAsync(orders[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task DeleteOrderAsync_InvalidId_ShouldReturnStatusCode200()
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
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                },
                new User
                {
                    Email = "mia@mentormate.com",
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Picture = null,
                }
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _ordersController.ControllerContext = new ControllerContext();
            _ordersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _ordersController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _orderService.Setup(x => x.DeleteOrderAsync("Admin", currentUserID, 123))
                .ReturnsAsync(new MessageResponse(Messages.OrderNotFound));

            // Act
            var actualResult = await _ordersController.DeleteOrderAsync(123) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }
    }
}
