using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Restaurant.Api.Controllers;
using Restaurant.Business.Responses;
using Restaurant.Business.Services;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Tests.Controllers
{
    [TestClass]
    public class TablesControllerTests
    {
        private Mock<ITableService> _tableService;
        private TablesController _tablesController;

        [TestInitialize]
        public void Initialize()
        {
            _tableService = new Mock<ITableService>();
            _tablesController = new TablesController(_tableService.Object);
        }

        [TestMethod]
        public async Task GetAllTablesAsync_AllTables_ShouldReturnStatusCode200()
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

            List<TableGeneralResponse> responses = new List<TableGeneralResponse>()
            {
                new TableGeneralResponse()
                {
                    Id = tables[0].Id,
                    Status = tables[0].Status.ToString(),
                    Capacity = tables[0].Capacity,
                    Waiter = "waiter",
                    Bill = "bill",
                },
                new TableGeneralResponse()
                {
                    Id = tables[1].Id,
                    Status = tables[1].Status.ToString(),
                    Capacity = tables[1].Capacity,
                    Waiter = "waiter",
                    Bill = "bill",
                },
            };

            _tableService.Setup(x => x.GetAllTablesAsync())
                .ReturnsAsync(responses);

            // Act
            var actualResult = await _tablesController.GetAllTablesAsync() as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task GetTableByIdAsync_ValidId_ShouldReturnStatusCode200()
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

            List<TableGeneralResponse> responses = new List<TableGeneralResponse>()
            {
                new TableGeneralResponse()
                {
                    Id = tables[0].Id,
                    Status = tables[0].Status.ToString(),
                    Capacity = tables[0].Capacity,
                    Waiter = "waiter",
                    Bill = "bill",
                },
                new TableGeneralResponse()
                {
                    Id = tables[1].Id,
                    Status = tables[1].Status.ToString(),
                    Capacity = tables[1].Capacity,
                    Waiter = "waiter",
                    Bill = "bill",
                },
            };

            var response = new TableResponse()
            {
                Id = tables[0].Id,
                Status = tables[0].Status.ToString(),
                Capacity = tables[0].Capacity,
                Waiter = "waiter",
                Bill = "bill",
            };

            _tableService.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(response);

            // Act
            var actualResult = await _tablesController.GetTableByIdAsync(tables[0].Id) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, actualResult.StatusCode, "Should match.");
        }

        [TestMethod]
        public async Task GetTableByIdAsync_InvalidId_ShouldReturnNull()
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

            List<TableGeneralResponse> responses = new List<TableGeneralResponse>()
            {
                new TableGeneralResponse()
                {
                    Id = tables[0].Id,
                    Status = tables[0].Status.ToString(),
                    Capacity = tables[0].Capacity,
                    Waiter = "waiter",
                    Bill = "bill",
                },
                new TableGeneralResponse()
                {
                    Id = tables[1].Id,
                    Status = tables[1].Status.ToString(),
                    Capacity = tables[1].Capacity,
                    Waiter = "waiter",
                    Bill = "bill",
                },
            };

            var response = new TableResponse()
            {
                Id = tables[0].Id,
                Status = tables[0].Status.ToString(),
                Capacity = tables[0].Capacity,
                Waiter = "waiter",
                Bill = "bill",
            };

            // Act
            var actualResult = await _tablesController.GetTableByIdAsync(10) as OkObjectResult;

            // Assert
            Assert.IsNull(actualResult.Value, "Should be null.");
        }
    }
}
