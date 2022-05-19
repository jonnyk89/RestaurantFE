using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class TableServiceTests
    {
        private Mock<ITableRepository> _tableRepository;
        private Mock<IMapperService> _mapperService;
        private Mock<IOrderRepository> _orderRepository;
        private TableService _tableService;

        [TestInitialize]
        public void Initialize()
        {
            _tableRepository = new Mock<ITableRepository>();
            _mapperService = new Mock<IMapperService>();
            _orderRepository = new Mock<IOrderRepository>();
            _tableService = new TableService(_tableRepository.Object, _mapperService.Object, _orderRepository.Object);
        }

        [TestMethod]
        public async Task GetAllTablesAsync_AllTables_ShouldReturnListOfTables()
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

            _tableRepository.Setup(x => x.GetAllTablesAsync())
                .ReturnsAsync(tables);

            _mapperService.Setup(x => x.MapTableCollection(tables))
                .Returns(responses);

            // Act
            var actualResult = await _tableService.GetAllTablesAsync();

            // Assert
            Assert.AreEqual(tables.Count, actualResult.Count, "Should match.");
        }

        [TestMethod]
        public async Task GetTableByIdAsync_ValidId_ShouldReturnTable()
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

            var response = new TableResponse()
            {
                Id = tables[0].Id,
                Status = tables[0].Status.ToString(),
                Capacity = tables[0].Capacity,
                Waiter = "waiter",
                Bill = "bill",
            };

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            _mapperService.Setup(x => x.MapTable(tables[0]))
                .Returns(response);

            // Act
            var actualResult = await _tableService.GetTableByIdAsync(tables[0].Id);

            // Assert
            Assert.AreEqual(response, actualResult, "Should match.");
        }

        [TestMethod]
        public async Task GetTableByIdAsync_InvalidId_ShouldReturnTable()
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

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _tableService.GetTableByIdAsync(123);

            // Assert
            Assert.AreEqual(null, actualResult, "Should match.");
        }

        [TestMethod]
        public async Task ClearAsync_TableActive_ShouldReturnTableClearedMessageResponse()
        {
            // Arrange
            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Active,
                    Capacity = 10,
                    Orders = new List<Order>()
                    {
                        new Order()
                        {
                            Status = OrderStatus.Active,
                            Date = DateTime.Now,
                            Price = 10,
                            TableId = 1,
                            OrderProducts = new List<OrderProduct>(),
                        }
                    },
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                    Orders = new List<Order>()
                    {
                        new Order()
                        {
                            Status = OrderStatus.Active,
                            Date = DateTime.Now,
                            Price = 10,
                            TableId = 1,
                            OrderProducts = new List<OrderProduct>(),
                        }
                    },
                }
            };

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _tableService.ClearAsync(1);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.TableCleared).Message, actualResult.Message, "Should match.");
        }

        [TestMethod]
        public async Task ClearAsync_TableAlreadyFree_ShouldReturnTableNotActiveMessageResponse()
        {
            // Arrange
            List<Table> tables = new List<Table>()
            {
                new Table()
                {
                    Id = 1,
                    Status = TableStatus.Active,
                    Capacity = 10,
                    Orders = new List<Order>()
                    {
                        new Order()
                        {
                            Status = OrderStatus.Active,
                            Date = DateTime.Now,
                            Price = 10,
                            TableId = 1,
                            OrderProducts = new List<OrderProduct>(),
                        }
                    },
                },
                new Table()
                {
                    Id = 2,
                    Status = TableStatus.Free,
                    Capacity = 10,
                    Orders = new List<Order>()
                    {
                        new Order()
                        {
                            Status = OrderStatus.Active,
                            Date = DateTime.Now,
                            Price = 10,
                            TableId = 1,
                            OrderProducts = new List<OrderProduct>(),
                        }
                    },
                }
            };

            _tableRepository.Setup(x => x.GetTableByIdAsync(tables[0].Id))
                .ReturnsAsync(tables[0]);

            // Act
            var actualResult = await _tableService.ClearAsync(1);

            // Assert
            Assert.AreEqual(new MessageResponse(Messages.TableNotActive).Message, actualResult.Message, "Should match.");
        }
    }
}
