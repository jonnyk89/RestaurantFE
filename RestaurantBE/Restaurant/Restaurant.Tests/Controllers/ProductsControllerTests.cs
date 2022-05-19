using Microsoft.AspNetCore.Mvc;
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
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        private Mock<IProductService> _productService;
        private ProductsController _productsController;

        [TestInitialize]
        public void Initialize()
        {
            _productService = new Mock<IProductService>();
            _productsController = new ProductsController(_productService.Object);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_AllProducts_ShouldReturnStatusCode200()
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
            List<ProductResponse> responses = new List<ProductResponse>()
            {
                new ProductResponse(products[0]),
                new ProductResponse(products[1]),
            };


            _productService.Setup(x => x.GetAllProductsAsync(null, null, 0, 0, null, SortOptions.Ascending))
                .ReturnsAsync(responses);


            // Act
            var actualResult = await _productsController.GetAllProductsAsync(null, null, 0, 0, null, SortOptions.Ascending) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ValidId_ShouldReturnStatusCode200()
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
            List<ProductResponse> responses = new List<ProductResponse>()
            {
                new ProductResponse(products[0]),
                new ProductResponse(products[1]),
            };


            _productService.Setup(x => x.GetProductByIdAsync(responses[0].productId))
                .ReturnsAsync(responses[0]);


            // Act
            var actualResult = await _productsController.GetProductByIdAsync(products[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task GetProductByIdAsync_InvalidId_ShouldReturnNull()
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
            List<ProductResponse> responses = new List<ProductResponse>()
            {
                new ProductResponse(products[0]),
                new ProductResponse(products[1]),
            };


            _productService.Setup(x => x.GetProductByIdAsync("123"))
                .ReturnsAsync(responses.FirstOrDefault(x => x.productId == "123"));


            // Act
            var actualResult = await _productsController.GetProductByIdAsync("123") as OkObjectResult;

            // Assert
            Assert.IsNull(actualResult, "Should return null.");
        }

        [TestMethod]
        public async Task CreateProductAsync_ValidInput_ShouldReturnStatusCode200()
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
                Name = products[0].Name,
                Description = "desc",
                Price = products[0].Price,
                CategoryId=categories[0].Id,
            };


            _productService.Setup(x => x.CreateProductAsync(request))
                .ReturnsAsync(new MessageResponse(Messages.ProductCreated));


            // Act
            var actualResult = await _productsController.CreateProductAsync(request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task CreateProductAsync_InvalidCategoryId_ShouldReturnStatusCode200()
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
                Name = products[0].Name,
                Description = "desc",
                Price = products[0].Price,
                CategoryId = "123",
            };


            _productService.Setup(x => x.CreateProductAsync(request))
                .ReturnsAsync(new MessageResponse(Messages.ProductCategoryInvalid));


            // Act
            var actualResult = await _productsController.CreateProductAsync(request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProductAsync_ValidId_ShouldReturnStatusCode200()
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
                Name = products[0].Name,
                Description = "desc",
                Price = products[0].Price,
                CategoryId = products[0].CategoryId,
            };


            _productService.Setup(x => x.UpdateProductAsync(products[0].Id, request))
                .ReturnsAsync(new MessageResponse(Messages.ProductUpdated));


            // Act
            var actualResult = await _productsController.UpdateProductAsync(products[0].Id, request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task UpdateProductAsync_InvalidId_ShouldReturnStatusCode200()
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
                Name = products[0].Name,
                Description = "desc",
                Price = products[0].Price,
                CategoryId = products[0].CategoryId,
            };


            _productService.Setup(x => x.UpdateProductAsync("123", request))
                .ReturnsAsync(new MessageResponse(Messages.ProductNotFound));


            // Act
            var actualResult = await _productsController.UpdateProductAsync("123", request) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task DeleteProductAsync_ValidId_ShouldReturnStatusCode200()
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


            _productService.Setup(x => x.DeleteProductAsync(products[0].Id))
                .ReturnsAsync(new MessageResponse(Messages.ProductUpdated));


            // Act
            var actualResult = await _productsController.DeleteProductAsync(products[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task DeleteProductAsync_InvalidId_ShouldReturnStatusCode200()
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


            _productService.Setup(x => x.DeleteProductAsync("123"))
                .ReturnsAsync(new MessageResponse(Messages.ProductNotFound));


            // Act
            var actualResult = await _productsController.DeleteProductAsync("123") as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }
    }
}
