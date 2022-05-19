using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userService;
        private UsersController _userController;

        [TestInitialize]
        public void Initialize()
        {
            _userService = new Mock<IUserService>();
            _userController = new UsersController(_userService.Object);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_GetAllUsers_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            _userService.Setup(x => x.GetAllUsersAsync(page, pageSize))
                .ReturnsAsync(users);

            // Act
            var actualResult = await _userController.GetAllUsersAsync(page, pageSize) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ValidId_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            _userService.Setup(x => x.GetUserByIdAsync(users[0].Id))
                .ReturnsAsync(users[0]);

            // Act
            var actualResult = await _userController.GetUserByIdAsync(users[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_InvalidId_ShouldReturnNull()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            _userService.Setup(x => x.GetUserByIdAsync(users[0].Id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == users[0].Id));

            // Act
            var actualResult = await _userController.GetUserByIdAsync("123") as OkObjectResult;

            // Assert
            Assert.IsNull(actualResult, "Should be null");
        }

        [TestMethod]
        public async Task GetCurrentUserAsync_CurrentUser_ShouldReturnCurrentUser()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _userController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _userService.Setup(x => x.GetUserByIdAsync(currentUserID))
                .ReturnsAsync(users[0]);

            // Act
            var actualResult = await _userController.GetUserByIdAsync(currentUserID) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task AddUserAsync_ValidInput_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var request = new UserCreateRequest()
            {
                Email = "koko@abv.bg",
            };

            var password = "123";

            _userService.Setup(x => x.CreateUserAsync(request, password))
                .ReturnsAsync(new MessageResponse(Messages.UserCreated));

            // Act
            var actualResult = await _userController.AddUserAsync(request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task AddUserAsync_InvalidUserEmail_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var request = new UserCreateRequest()
            {
                Email = "admin@mentormate.com",
            };

            var password = "123";

            _userService.Setup(x => x.CreateUserAsync(request, password))
                .ReturnsAsync(new MessageResponse(Messages.UserEmailTaken));

            // Act
            var actualResult = await _userController.AddUserAsync(request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateUserAsync_ValidId_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var request = new UserUpdateRequest()
            {
                Email = "admin@mentormate.com",
            };

            var password = "123";

            _userService.Setup(x => x.UpdateUserAsync(request, password, users[0].Id))
                .ReturnsAsync(new MessageResponse(Messages.UserUpdated));

            // Act
            var actualResult = await _userController.UpdateUserAsync(users[0].Id, request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateUserAsync_InvalidId_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var request = new UserUpdateRequest()
            {
                Email = "admin@mentormate.com",
            };

            var password = "123";

            _userService.Setup(x => x.UpdateUserAsync(request, password, "123"))
                .ReturnsAsync(new MessageResponse(Messages.UserNotFound));

            // Act
            var actualResult = await _userController.UpdateUserAsync("123", request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task UpdateCurrentUserAsync_CurrentUser_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, users[0].Id),
                                        new Claim(ClaimTypes.Name, users[0].Email),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _userController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var request = new UserUpdateRequest()
            {
                Email = "admin@mentormate.com",
            };

            var password = "123";

            _userService.Setup(x => x.UpdateUserAsync(request, password, currentUserID))
                .ReturnsAsync(new MessageResponse(Messages.UserUpdated));

            // Act
            var actualResult = await _userController.UpdateUserAsync(currentUserID, request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteUserAsync_ValidId_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var password = "123";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                                        new Claim(ClaimTypes.Role, "Admin"),
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _userController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _userService.Setup(x => x.DeleteUserAsync(currentUserID, users[2].Id))
                .ReturnsAsync(new MessageResponse(Messages.UserDeleted));

            // Act
            var actualResult = await _userController.DeleteUserAsync(users[2].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task DeleteUserAsync_InvalidId_ShouldReturnStatusCode200()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
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

            var password = "123";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
                                        new Claim(ClaimTypes.Name, "gunnar@somecompany.com")
                                        // other required and custom claims
                                   }, "TestAuthentication"));

            _userController.ControllerContext = new ControllerContext();
            _userController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var currentUser = _userController.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _userService.Setup(x => x.DeleteUserAsync(currentUserID, users[2].Id))
                .ReturnsAsync(new MessageResponse(Messages.UserDeleted));

            // Act
            var actualResult = await _userController.DeleteUserAsync("123") as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }
    }
}
