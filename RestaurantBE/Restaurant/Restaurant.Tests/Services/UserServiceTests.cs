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
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private UserService _userService;

        [TestInitialize]
        public void Initialize()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_FirstTwoUsersFound_ShouldReturnListOfTwoUsers()
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

            _userRepository.Setup(x => x.GetAllUsersAsync(page, pageSize))
                .ReturnsAsync(new List<User>()
                {
                    users[0], users[1]
                });

            // Act
            var actualResult = await _userService.GetAllUsersAsync(page, pageSize);

            // Assert
            Assert.AreEqual(_userRepository.Object.GetAllUsersAsync(page, pageSize).Result.Count, actualResult.Count, "Should match.");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ValidId_ShouldReturnUser()
        {
            // Arrange
            string userIdValid = "77691534-77dc-474a-9e4c-e6bff47493f9";

            _userRepository.Setup(x => x.GetUserByIdAsync(userIdValid))
                .ReturnsAsync(new User()
                {
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Email = "mia@mentormate.com",
                    Picture = null,
                });

            // Act
            var actualResult = await _userService.GetUserByIdAsync(userIdValid);

            // Assert
            Assert.AreEqual(_userRepository.Object.GetUserByIdAsync(userIdValid).Result, actualResult, "Should match.");
        }

        [TestMethod]
        public async Task GetUserByIdAsync_InvalidId_ShouldNotReturnUser()
        {
            // Arrange
            string userValidId = "77691534-77dc-474a-9e4c-e6bff47493f9";
            string userInvalidId = "77691534-77dc-474a-9e4c-e6bff474sad93f9";
            _userRepository.Setup(x => x.GetUserByIdAsync(userValidId))
                .ReturnsAsync(new User()
                {
                    Id = userValidId,
                    FirstName = "Mia",
                    LastName = "Farrow",
                    Role = UserRole.Waiter,
                    UserName = "miafarrow",
                    Email = "mia@mentormate.com",
                    Picture = null,
                });

            // Act
            var actualResult = await _userService.GetUserByIdAsync(userInvalidId);

            // Assert
            Assert.AreNotEqual(_userRepository.Object.GetUserByIdAsync(userValidId).Result, actualResult, "Should not match.");
        }

        [TestMethod]
        public async Task CreateUserAsync_ValidUser_ShouldReturnUserCreatedMessageResponse()
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

            var request = new UserCreateRequest() 
            { 
                Email = "koko@abv.bg",
            };


            var newUser = new User()
            {
                Email =request.Email,
            };

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            // Act
            var actualResult = await _userService.CreateUserAsync(request, password);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserCreated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task CreateUserAsync_InvalidUserEmail_ShouldReturnUserEmailTakenMessageResponse()
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

            var request = new UserCreateRequest()
            {
                Email = "mia@mentormate.com",
            };


            var newUser = new User()
            {
                Email = request.Email,
            };

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            // Act
            var actualResult = await _userService.CreateUserAsync(request, password);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserEmailTaken).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateUserAsync_ValidUser_ShouldReturnUserUpdatedMessageResponse()
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

            var request = new UserUpdateRequest()
            {
                Email = "koko@gmail.com",
            };


            var newUser = new User()
            {
                Email = request.Email,
            };

            string id = users[0].Id;

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.UpdateUserAsync(request, password, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserUpdated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateUserAsync_InvalidUserId_ShouldReturnUserUpdatedMessageResponse()
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

            var request = new UserUpdateRequest()
            {
                Email = "koko@gmail.com",
            };


            var newUser = new User()
            {
                Email = request.Email,
            };

            string id = "1";

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.UpdateUserAsync(request, password, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserNotFound).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateUserAsync_InvalidUserEmail_ShouldReturnUserEmailTakenMessageResponse()
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

            var request = new UserUpdateRequest()
            {
                Email = "mia@mentormate.com",
            };


            var newUser = new User()
            {
                Email = request.Email,
            };

            string id = users[0].Id;

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.UpdateUserAsync(request, password, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserEmailTaken).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProfileAsync_ValidUser_ShouldReturnUserUpdatedMessageResponse()
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

            var request = new UserUpdateProfileRequest()
            {
                Email = "koko@gmail.com",
            };


            var newUser = new User()
            {
                Email = request.Email,
            };

            string id = users[0].Id;

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.UpdateProfileAsync(request, password, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserUpdated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProfileAsync_InvalidUserEmail_ShouldReturnUserEmailTakenMessageResponse()
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

            var request = new UserUpdateProfileRequest()
            {
                Email = "mia@mentormate.com",
            };


            var newUser = new User()
            {
                Email = request.Email,
            };

            string id = users[0].Id;

            string password = "123";

            _userRepository.Setup(x => x.GetUserByEmailAsync(newUser.Email))
                .ReturnsAsync(users.FirstOrDefault(u => u.Email == newUser.Email));

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.UpdateProfileAsync(request, password, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserEmailTaken).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeletUserAsync_ValidUser_ShouldReturnUserDeletedMessageResponse()
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


            string id = users[0].Id;

            string currentUserId = "123";

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.DeleteUserAsync(currentUserId, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserDeleted).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeletUserAsync_InvalidUserId_ShouldReturnUserNotFoundMessageResponse()
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


            string id = "1";

            string currentUserId = "123";

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.DeleteUserAsync(currentUserId, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserNotFound).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeletUserAsync_UserIdEqualsCurrentUserId_ShouldReturnUserAdminCannotDeleteSelfMessageResponse()
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


            string id = users[0].Id;

            string currentUserId = id;

            _userRepository.Setup(x => x.GetUserByIdAsync(id))
                .ReturnsAsync(users.FirstOrDefault(u => u.Id == id));

            // Act
            var actualResult = await _userService.DeleteUserAsync(currentUserId, id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.UserAdminCannotDeleteSelf).Message, actualResult.Message, "Should match.");
        }
    }
}
