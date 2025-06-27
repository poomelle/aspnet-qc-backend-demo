using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.QcAveTestTimeKpi;
using ChemsonLab.API.Repositories.QcAveTestTimeKpiRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.ControllerTests
{
    public class QcAveTestTimeKpiControllerTests
    {
        private readonly Mock<IQcAveTestTimeKpiRepository> _qcAveTestTimeKpiRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<QcAveTestTimeKpiController>> _loggerMock;
        private readonly QcAveTestTimeKpiController _controller;

        public QcAveTestTimeKpiControllerTests()
        {
            _qcAveTestTimeKpiRepositoryMock = new Mock<IQcAveTestTimeKpiRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<QcAveTestTimeKpiController>>();

            _controller = new QcAveTestTimeKpiController(
                _qcAveTestTimeKpiRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of QcAveTestTimeKpiController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var qcAveTestTimeKpiList = new List<QcAveTestTimeKpi>
            {
                new QcAveTestTimeKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    AveTestTime = 5000,
                    Product = new Product { Id = 1, Name = "Test Product" },
                    Machine = new Machine { Id = 1, Name = "Test Machine" }
                }
            };

            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, true))
                .ReturnsAsync(qcAveTestTimeKpiList);

            _mapperMock.Setup(mapper => mapper.Map<List<QcAveTestTimeKpiDTO>>(It.IsAny<List<QcAveTestTimeKpi>>()))
                .Returns(new List<QcAveTestTimeKpiDTO>
                {
                    new QcAveTestTimeKpiDTO
                    {
                        Id = 1,
                        Year = "2024",
                        Month = "01",
                        TotalTest = 100,
                        AveTestTime = 5000,
                        Product = new Product { Id = 1, Name = "Test Product" },
                        Machine = new Machine { Id = 1, Name = "Test Machine" }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<QcAveTestTimeKpiDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests the GetAll method with filters to ensure it passes parameters correctly.
        /// </summary>
        [Fact]
        public async Task GetAll_WithFilters_ReturnsOkResult()
        {
            // Arrange
            var productName = "Test Product";
            var machineName = "Test Machine";
            var year = "2024";
            var month = "01";
            var sortBy = "Year";
            var isAscending = false;

            var qcAveTestTimeKpiList = new List<QcAveTestTimeKpi>
            {
                new QcAveTestTimeKpi
                {
                    Id = 1,
                    Year = year,
                    Month = month,
                    TotalTest = 100,
                    AveTestTime = 5000
                }
            };

            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.GetAllAsync(productName, machineName, year, month, sortBy, isAscending))
                .ReturnsAsync(qcAveTestTimeKpiList);

            _mapperMock.Setup(mapper => mapper.Map<List<QcAveTestTimeKpiDTO>>(It.IsAny<List<QcAveTestTimeKpi>>()))
                .Returns(new List<QcAveTestTimeKpiDTO>());

            // Act
            var result = await _controller.GetAll(productName, machineName, year, month, sortBy, isAscending);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _qcAveTestTimeKpiRepositoryMock.Verify(repo => repo.GetAllAsync(productName, machineName, year, month, sortBy, isAscending), Times.Once);
        }

        /// <summary>
        /// Tests the GetAll method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetAll_ThrowsException_Returns500Error()
        {
            // Arrange
            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, true);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the GetById method of QcAveTestTimeKpiController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var qcAveTestTimeKpi = new QcAveTestTimeKpi
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                AveTestTime = 5000,
                Product = new Product { Id = 1, Name = "Test Product" },
                Machine = new Machine { Id = 1, Name = "Test Machine" }
            };

            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(qcAveTestTimeKpi);

            _mapperMock.Setup(mapper => mapper.Map<QcAveTestTimeKpiDTO>(It.IsAny<QcAveTestTimeKpi>()))
                .Returns(new QcAveTestTimeKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    AveTestTime = 5000,
                    Product = new Product { Id = 1, Name = "Test Product" },
                    Machine = new Machine { Id = 1, Name = "Test Machine" }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcAveTestTimeKpiDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
            Assert.Equal("01", returnValue.Month);
        }

        /// <summary>
        /// Tests the GetById method of QcAveTestTimeKpiController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((QcAveTestTimeKpi?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests the GetById method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetById_ThrowsException_Returns500Error()
        {
            // Arrange
            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Create method of QcAveTestTimeKpiController to ensure it returns CreatedAtAction for valid creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidQcAveTestTimeKpi_ReturnsCreatedAtAction()
        {
            // Arrange
            var newQcAveTestTimeKpi = new AddQcAveTestTimeKpiRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                AveTestTime = 5000
            };

            _mapperMock.Setup(mapper => mapper.Map<QcAveTestTimeKpi>(It.IsAny<AddQcAveTestTimeKpiRequestDTO>()))
                .Returns(new QcAveTestTimeKpi { ProductId = 1, MachineId = 1, Year = "2024", Month = "01", TotalTest = 100, AveTestTime = 5000 });

            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<QcAveTestTimeKpi>()))
                .ReturnsAsync(new QcAveTestTimeKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    AveTestTime = 5000,
                    Product = new Product { Id = 1, Name = "Test Product" },
                    Machine = new Machine { Id = 1, Name = "Test Machine" }
                });

            _mapperMock.Setup(mapper => mapper.Map<QcAveTestTimeKpiDTO>(It.IsAny<QcAveTestTimeKpi>()))
                .Returns(new QcAveTestTimeKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    AveTestTime = 5000,
                    Product = new Product { Id = 1, Name = "Test Product" },
                    Machine = new Machine { Id = 1, Name = "Test Machine" }
                });

            // Act
            var result = await _controller.Create(newQcAveTestTimeKpi);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<QcAveTestTimeKpiDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
            Assert.Equal("01", returnValue.Month);
            Assert.Equal(nameof(QcAveTestTimeKpiController.GetById), createdResult.ActionName);
        }

        /// <summary>
        /// Tests the Create method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Create_ThrowsException_Returns500Error()
        {
            // Arrange
            var newQcAveTestTimeKpi = new AddQcAveTestTimeKpiRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                AveTestTime = 5000
            };

            _mapperMock.Setup(mapper => mapper.Map<QcAveTestTimeKpi>(It.IsAny<AddQcAveTestTimeKpiRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Create(newQcAveTestTimeKpi);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Delete method of QcAveTestTimeKpiController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var qcAveTestTimeKpiToDelete = new QcAveTestTimeKpi
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                AveTestTime = 5000,
                Product = new Product { Id = 1, Name = "Test Product" },
                Machine = new Machine { Id = 1, Name = "Test Machine" }
            };

            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(qcAveTestTimeKpiToDelete);

            _mapperMock.Setup(mapper => mapper.Map<QcAveTestTimeKpiDTO>(It.IsAny<QcAveTestTimeKpi>()))
                .Returns(new QcAveTestTimeKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    AveTestTime = 5000,
                    Product = new Product { Id = 1, Name = "Test Product" },
                    Machine = new Machine { Id = 1, Name = "Test Machine" }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcAveTestTimeKpiDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
        }

        /// <summary>
        /// Tests the Delete method of QcAveTestTimeKpiController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((QcAveTestTimeKpi?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests the Delete method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Delete_ThrowsException_Returns500Error()
        {
            // Arrange
            _qcAveTestTimeKpiRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
