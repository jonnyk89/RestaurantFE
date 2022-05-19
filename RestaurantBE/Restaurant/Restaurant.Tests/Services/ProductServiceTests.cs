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
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<ICategoryRepository> _categoryRepository;
        private ProductService _productService;

        [TestInitialize]
        public void Initialize()
        {
            _productRepository = new Mock<IProductRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _productService = new ProductService(_productRepository.Object, _categoryRepository.Object);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_AllProducts_ShouldReturnListOfProducts()
        {
            // Arrange
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

            _productRepository.Setup(x => x.GetAllProductsAsync(null, null, 0, 0, null, SortOptions.Ascending))
                .ReturnsAsync(products);
                

            // Act
            var actualResult = await _productService.GetAllProductsAsync(null, null, 0, 0, null, SortOptions.Ascending);

            // Assert
            Assert.AreEqual(products.Count, actualResult.Count, "Should match.");
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ValidId_ShouldReturnProduct()
        {
            // Arrange
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

            string id = products[0].Id;

            _productRepository.Setup(x => x.GetProductByIdAsync(id))
                .ReturnsAsync(products[0]);


            // Act
            var actualResult = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.AreEqual(products[0], actualResult, "Should match.");
        }

        [TestMethod]
        public async Task GetProductByIdAsync_InvalidId_ShouldNotProduct()
        {
            // Arrange
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

            string id = "123";

            _productRepository.Setup(x => x.GetProductByIdAsync(id))
                .ReturnsAsync(products.FirstOrDefault(p => p.Id == id));


            // Act
            var actualResult = await _productService.GetProductByIdAsync(id);

            // Assert
            Assert.AreEqual(null, actualResult, "Should match.");
        }

        [TestMethod]
        public async Task CreateProductAsync_ValidInput_ShouldReturnProductCreatedMessageResponse()
        {
            // Arrange
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

            var request = new ProductCreateRequest()
            {
                Name = "tomato",
                Price = 10,
                CategoryId = categories[1].Id,
            };

            var newProduct = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
            };

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(request.CategoryId))
                .ReturnsAsync(categories.FirstOrDefault(c => c.Id == request.CategoryId));


            // Act
            var actualResult = await _productService.CreateProductAsync(request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductCreated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task CreateProductAsync_InvalidCategoryId_ShouldReturnProductCategoryInvalidMessageResponse()
        {
            // Arrange
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

            var request = new ProductCreateRequest()
            {
                Name = "tomato",
                Price = 10,
                CategoryId = "12312321",
            };

            var newProduct = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
            };

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(request.CategoryId))
                .ReturnsAsync(categories.FirstOrDefault(c => c.Id == request.CategoryId));


            // Act
            var actualResult = await _productService.CreateProductAsync(request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductCategoryInvalid).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProductAsync_ValidInput_ShouldReturnProductUpdatedMessageResponse()
        {
            // Arrange
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

            var request = new ProductUpdateRequest()
            {
                Name = "tomato",
                Price = 10,
                Description = "desc",
                CategoryId = categories[1].Id,
            };

            var existingProduct = new Product()
            {
                Id = products[0].Id,
                Description = "desc",
                Name = products[0].Name,
                Price = products[0].Price,
                CategoryId = products[0].CategoryId,
            };

            string validId = existingProduct.Id;

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(request.CategoryId))
                .ReturnsAsync(categories.FirstOrDefault(c => c.Id == request.CategoryId));

            _productRepository.Setup(x => x.GetProductByIdAsync(validId))
                .ReturnsAsync(products.FirstOrDefault(p => p.Id == validId));

            // Act
            var actualResult = await _productService.UpdateProductAsync(validId, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductUpdated).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProductAsync_InvalidCategoryId_ShouldReturnProductCategoryInvalidMessageResponse()
        {
            // Arrange
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

            var request = new ProductUpdateRequest()
            {
                Name = "tomato",
                Price = 10,
                Description = "desc",
                CategoryId = categories[1].Id,
            };

            var existingProduct = new Product()
            {
                Id = products[0].Id,
                Description = "desc",
                Name = products[0].Name,
                Price = products[0].Price,
                CategoryId = products[0].CategoryId,
            };

            string validId = existingProduct.Id;

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync("123"))
                .ReturnsAsync(categories.FirstOrDefault(c => c.Id == "123"));

            _productRepository.Setup(x => x.GetProductByIdAsync(validId))
                .ReturnsAsync(products.FirstOrDefault(p => p.Id == validId));

            // Act
            var actualResult = await _productService.UpdateProductAsync(validId, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductCategoryInvalid).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProductAsync_InvalidProductId_ShouldReturnProductNotFoundMessageResponse()
        {
            // Arrange
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

            var request = new ProductUpdateRequest()
            {
                Name = "tomato",
                Price = 10,
                Description = "desc",
                CategoryId = categories[1].Id,
            };

            var existingProduct = new Product()
            {
                Id = products[0].Id,
                Description = "desc",
                Name = products[0].Name,
                Price = products[0].Price,
                CategoryId = products[0].CategoryId,
            };

            string inValidId = "123";

            _categoryRepository.Setup(x => x.GetCategoryByIdAsync(request.CategoryId))
                .ReturnsAsync(categories.FirstOrDefault(c => c.Id == request.CategoryId));

            _productRepository.Setup(x => x.GetProductByIdAsync(inValidId))
                .ReturnsAsync(products.FirstOrDefault(p => p.Id == inValidId));

            // Act
            var actualResult = await _productService.UpdateProductAsync(inValidId, request);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductNotFound).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeleteProductAsync_ValidId_ShouldReturnProductNotFoundMessageResponse()
        {
            // Arrange
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

            string id = products[0].Id;

            _productRepository.Setup(x => x.GetProductByIdAsync(id))
                .ReturnsAsync(products[0]);

            // Act
            var actualResult = await _productService.DeleteProductAsync(id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductDeleted).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task DeleteProductAsync_InvalidId_ShouldReturnProductNotFoundMessageResponse()
        {
            // Arrange
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

            string id = "123";

            _productRepository.Setup(x => x.GetProductByIdAsync(id))
                .ReturnsAsync(products.FirstOrDefault(p => p.Id == id));

            // Act
            var actualResult = await _productService.DeleteProductAsync(id);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.ProductNotFound).Message, actualResult.Message, "Should match.");
        }
    }
}
