using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Restaurant.Business.Services;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Data.Contracts;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Domain.Entities.MessageResponses;
using Restaurant.Business.Requests;

namespace Restaurant.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<IMapperService> _mapperService;
        private CategoryService _categoryService;

        [TestInitialize]
        public void Initialize()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _mapperService = new Mock<IMapperService>();
            _categoryService = new CategoryService(_categoryRepository.Object, _mapperService.Object);
        }

        [TestMethod]
        public async Task GetAllCategoriesAsync_GetAllCategories_ShouldReturnListOfAllCateogires()
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

            var topCategories = categories.Where(c => c.ParentId == null);
            var childrenByParentId = categories.Where(c => c.ParentId != null).ToLookup(c => c.ParentId);


            _mapperService.Setup(x => x.MapCategoryCollection(topCategories, childrenByParentId))
                .ReturnsAsync(new List<GeneralCategoryResponse>() 
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
                });

            _categoryRepository.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var actualResult = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.AreEqual(_categoryRepository.Object.GetAllCategoriesAsync().Result.Count, actualResult.Count, "Should be equal.");
        }

        [TestMethod]
        public async Task GetCategoryByIdAsync_ValidId_ShouldReturnCategory()
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

            var validId = categories[0].Id;

            var topCategories = categories.Where(c => c.Id == validId);
            var childrenByParentId = categories.Where(c => c.ParentId != null).ToLookup(c => c.ParentId);

            _mapperService.Setup(x => x.MapCategoryCollection(topCategories, childrenByParentId))
                .ReturnsAsync(new List<GeneralCategoryResponse>()
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
                });

            _categoryRepository.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var actualResult = await _categoryService.GetCategoryByIdAsync(validId);

            // Assert
            Assert.AreEqual(_categoryRepository.Object.GetAllCategoriesAsync().Result[0].Name, actualResult[0].Name, "Should be equal.");
        }

        [TestMethod]
        public async Task GetCategoryByIdAsync_InvalidId_ShouldNotReturnCategory()
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

            var validId = categories[0].Id;
            var invalidId = "123";

            var topCategories = categories.Where(c => c.Id == validId);
            var childrenByParentId = categories.Where(c => c.ParentId != null).ToLookup(c => c.ParentId);

            _mapperService.Setup(x => x.MapCategoryCollection(topCategories, childrenByParentId))
                .ReturnsAsync(new List<GeneralCategoryResponse>()
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
                });

            _categoryRepository.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var actualResult = await _categoryService.GetCategoryByIdAsync(invalidId);

            // Assert
            Assert.AreNotEqual(_categoryRepository.Object.GetAllCategoriesAsync().Result.Count, actualResult.Count, "Should not be equal.");
        }

        [TestMethod]
        public async Task AddCategoryAsync_ValidCategory_ShouldReturnSuccessfullyCreatedMessageResponse()
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

            var newCategoryRequest = new CategoryCreateRequest()
            {
                Name = "Chicknes",
            };

            var newCategory = new Category()
            {
                Name = newCategoryRequest.Name,
            };

            _categoryRepository.Setup(x => x.GetCategoryByNameAsync(newCategory.Name))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Name == newCategory.Name));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(newCategory.ParentId))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == newCategory.ParentId));

            // Act
            var actualResult = await _categoryService.AddCategoryAsync(newCategoryRequest);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryCreated).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task AddCategoryAsync_AlreadyExistingCategoryName_ShouldReturnCategoryExistsMessageResponse()
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

            var newCategoryRequest = new CategoryCreateRequest()
            {
                Name = "Pizzas",
            };

            var newCategory = new Category()
            {
                Name = newCategoryRequest.Name,
            };

            _categoryRepository.Setup(x => x.GetCategoryByNameAsync(newCategory.Name))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Name == newCategory.Name));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(newCategory.ParentId))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == newCategory.ParentId));

            // Act
            var actualResult = await _categoryService.AddCategoryAsync(newCategoryRequest);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryExists).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task AddCategoryAsync_InvalidCategoryParentId_ShouldReturnCategoryParentInvalidIdMessageResponse()
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

            var newCategoryRequest = new CategoryCreateRequest()
            {
                Name = "Chickens",
                ParentId = "3",
            };

            var newCategory = new Category()
            {
                Name = newCategoryRequest.Name,
                ParentId = "123",
            };

            _categoryRepository.Setup(x => x.GetCategoryByNameAsync(newCategory.Name))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Name == newCategory.Name));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(newCategory.ParentId))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == newCategory.ParentId));

            // Act
            var actualResult = await _categoryService.AddCategoryAsync(newCategoryRequest);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryParentInvalidId).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task UpdateCategoryAsync_ValidCategory_ShouldReturnCategoryUpdatedMessageResponse()
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

            var newCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Potatoes",
            };

            var existingCategory = new Category()
            {
                Id = categories[0].Id,
                Name = newCategoryRequest.Name,
                ParentId = categories[0].Id,
            };

            _categoryRepository.Setup(x => x.GetCategoryByNameAsync(existingCategory.Name))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Name == "koko"));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.ParentId))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.ParentId));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.Id))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.Id));

            // Act
            var actualResult = await _categoryService.UpdateCategoryAsync(existingCategory.Id, newCategoryRequest);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryUpdated).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task UpdateCategoryAsync_InvalidCategoryId_ShouldReturnCategoryNotFoundMessageResponse()
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

            var newCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Potatoes",
            };

            var existingCategory = new Category()
            {
                Id = categories[0].Id,
                Name = newCategoryRequest.Name,
                ParentId = categories[0].Id,
            };

            _categoryRepository.Setup(x => x.GetCategoryByNameAsync(existingCategory.Name))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Name == "koko"));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.ParentId))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.ParentId));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.Id))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.Id));

            // Act
            var actualResult = await _categoryService.UpdateCategoryAsync("123", newCategoryRequest);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryNotFound).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task UpdateCategoryAsync_InvalidCategoryParentId_ShouldReturnCategoryParentInvalidIdMessageResponse()
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

            var newCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Potatoes",
                ParentId = "3",
            };

            var existingCategory = new Category()
            {
                Id = categories[0].Id,
                Name = newCategoryRequest.Name,
                ParentId = "123",
            };

            _categoryRepository.Setup(x => x.GetCategoryByNameAsync(existingCategory.Name))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Name == "koko"));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.ParentId))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.ParentId));

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.Id))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.Id));

            // Act
            var actualResult = await _categoryService.UpdateCategoryAsync(existingCategory.Id, newCategoryRequest);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryParentInvalidId).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task DeleteCategoryAsync_ValidCategoryId_ShouldReturnCategoryDeletedMessageResponse()
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

            var newCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Potatoes",
            };

            var existingCategory = new Category()
            {
                Id = categories[0].Id,
                Name = newCategoryRequest.Name,
            };

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(existingCategory.Id))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == existingCategory.Id));

            // Act
            var actualResult = await _categoryService.DeleteCategoryAsync(existingCategory.Id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryDeleted).Message, actualResult.Message, "Should be equal.");
        }

        [TestMethod]
        public async Task DeleteCategoryAsync_InvalidCategoryId_ShouldReturnCategoryNotFoundMessageResponse()
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

            var newCategoryRequest = new CategoryUpdateRequest()
            {
                Name = "Potatoes",
            };

            var existingCategory = new Category()
            {
                Id = categories[0].Id,
                Name = newCategoryRequest.Name,
            };

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync("3"))
                .ReturnsAsync(categories.FirstOrDefault(x => x.Id == "3"));

            // Act
            var actualResult = await _categoryService.DeleteCategoryAsync("3");

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.CategoryNotFound).Message, actualResult.Message, "Should be equal.");
        }
    }
}
