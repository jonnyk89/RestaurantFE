using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Restaurant.Api.Controllers;
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTests
    {
        private Mock<ICategoryService> _categoryService;
        private CategoriesController _categoriesController;

        [TestInitialize]
        public void Initialize()
        {
            _categoryService = new Mock<ICategoryService>();
            _categoriesController = new CategoriesController(_categoryService.Object);
        }

        [TestMethod]
        public async Task GetAllCategoriesAsync_GetAllCategories_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            _categoryService.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(responses);

            // Act
            var actualResult = await _categoriesController.GetAllCategoriesAsync() as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task GetCategoryByIdAsync_ValidId_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            _categoryService.Setup(x => x.GetCategoryByIdAsync(categories[0].Id))
                .ReturnsAsync(responses);

            // Act
            var actualResult = await _categoriesController.GetCategoryByIdAsync(categories[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task GetCategoryByIdAsync_InvalidId_ShouldReturnNull()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            // Act
            var actualResult = await _categoriesController.GetCategoryByIdAsync("123") as OkObjectResult;

            // Assert
            Assert.IsNull(actualResult, "Should be null");
        }

        [TestMethod]
        public async Task AddCategoryAsync_ValidInput_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            var newCategoryRequest = new CategoryCreateRequest()
            {
                Name = "Chicknes",
            };

            var newCategory = new Category()
            {
                Name = newCategoryRequest.Name,
            };

            _categoryService.Setup(x => x.AddCategoryAsync(newCategoryRequest))
                .ReturnsAsync(new MessageResponse(Messages.CategoryCreated));

            // Act
            var actualResult = await _categoriesController.AddCategoryAsync(newCategoryRequest) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task AddCategoryAsync_CategoryNameTaken_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            var newCategoryRequest = new CategoryCreateRequest()
            {
                Name = "Potatoes",
            };

            var newCategory = new Category()
            {
                Name = newCategoryRequest.Name,
            };

            _categoryService.Setup(x => x.AddCategoryAsync(newCategoryRequest))
                .ReturnsAsync(new MessageResponse(Messages.CategoryExists));

            // Act
            var actualResult = await _categoriesController.AddCategoryAsync(newCategoryRequest) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task UpdateCategory_ValidId_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            var existingCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Peaches",
            };

            _categoryService.Setup(x => x.UpdateCategoryAsync(categories[0].Id, existingCategoryRequest))
                .ReturnsAsync(new MessageResponse(Messages.CategoryUpdated));

            // Act
            var actualResult = await _categoriesController.UpdateCategory(categories[0].Id, existingCategoryRequest) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task UpdateCategory_InvalidId_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            var existingCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Peaches",
            };

            _categoryService.Setup(x => x.UpdateCategoryAsync("123", existingCategoryRequest))
                .ReturnsAsync(new MessageResponse(Messages.CategoryNotFound));

            // Act
            var actualResult = await _categoriesController.UpdateCategory("123", existingCategoryRequest) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task DeleteCategory_ValidId_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            var existingCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Peaches",
            };

            _categoryService.Setup(x => x.DeleteCategoryAsync(categories[0].Id))
                .ReturnsAsync(new MessageResponse(Messages.CategoryDeleted));

            // Act
            var actualResult = await _categoriesController.DeleteCategory(categories[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }

        [TestMethod]
        public async Task DeleteCategory_InvalidId_ShouldReturnStatusCode200()
        {
            // Arrange
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Potatoes",
                },
                new Category()
                {
                    Name = "Tomatoes",
                },
                new Category()
                {
                    Name = "Pizzas",
                },
                new Category()
                {
                    Name = "Oranges",
                },
            };

            var responses = new List<GeneralCategoryResponse>()
                {
                    new GeneralCategoryResponse()
                    {
                        Id = categories[0].Id,
                        Name = categories[0].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[1].Id,
                        Name = categories[1].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[2].Id,
                        Name = categories[2].Name,
                        Subcategories = null,
                    },
                    new GeneralCategoryResponse()
                    {
                        Id = categories[3].Id,
                        Name = categories[3].Name,
                        Subcategories = null,
                    },
                };

            var existingCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Peaches",
            };

            _categoryService.Setup(x => x.DeleteCategoryAsync("123"))
                .ReturnsAsync(new MessageResponse(Messages.CategoryNotFound));

            // Act
            var actualResult = await _categoriesController.DeleteCategory("123") as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should be equal");
        }
    }
}
