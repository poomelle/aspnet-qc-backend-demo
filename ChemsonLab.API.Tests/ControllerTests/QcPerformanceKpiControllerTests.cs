using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.QcPerformanceKpi;
using ChemsonLab.API.Repositories.QcPerformanceKpiRepository;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.ControllerTests
{
    public class QcPerformanceKpiControllerTests
    {
        private readonly QcPerformanceKpiController _controller;
        private readonly Mock<IQcPerformanceKpiRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<QcPerformanceKpiController>> _mockLogger;

        public QcPerformanceKpiControllerTests()
        {
            _mockRepository = new Mock<IQcPerformanceKpiRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<QcPerformanceKpiController>>();

            _controller = new QcPerformanceKpiController(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }


        /// <summary>
        /// Tests the GetAll method of QcPerformanceKpiController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var qcPerformanceKpiList = new List<QcPerformanceKpi>
            {
                new QcPerformanceKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 10,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, true))
                .ReturnsAsync(qcPerformanceKpiList);

            _mockMapper.Setup(mapper => mapper.Map<List<QcPerformanceKpiDTO>>(It.IsAny<List<QcPerformanceKpi>>()))
                .Returns(new List<QcPerformanceKpiDTO>
                {
                    new QcPerformanceKpiDTO
                    {
                        Id = 1,
                        Year = "2024",
                        Month = "01",
                        TotalTest = 100,
                        FirstPass = 85,
                        SecondPass = 10,
                        ThirdPass = 5,
                        Product = new Product { Id = 1, Name = "Test Product", Status = true },
                        Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<QcPerformanceKpiDTO>>(okResult.Value);
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

            var qcPerformanceKpiList = new List<QcPerformanceKpi>
            {
                new QcPerformanceKpi
                {
                    Id = 1,
                    Year = year,
                    Month = month,
                    TotalTest = 100,
                    FirstPass = 85
                }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, machineName, year, month, sortBy, isAscending))
                .ReturnsAsync(qcPerformanceKpiList);

            _mockMapper.Setup(mapper => mapper.Map<List<QcPerformanceKpiDTO>>(It.IsAny<List<QcPerformanceKpi>>()))
                .Returns(new List<QcPerformanceKpiDTO>());

            // Act
            var result = await _controller.GetAll(productName, machineName, year, month, sortBy, isAscending);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, machineName, year, month, sortBy, isAscending), Times.Once);
        }

        /// <summary>
        /// Tests the GetAll method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetAll_ThrowsException_Returns500Error()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, true);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the GetById method of QcPerformanceKpiController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var qcPerformanceKpi = new QcPerformanceKpi
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                FirstPass = 85,
                SecondPass = 10,
                ThirdPass = 5,
                Product = new Product { Id = 1, Name = "Test Product", Status = true },
                Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(qcPerformanceKpi);

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpiDTO>(It.IsAny<QcPerformanceKpi>()))
                .Returns(new QcPerformanceKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 10,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcPerformanceKpiDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
            Assert.Equal("01", returnValue.Month);
            Assert.Equal(100, returnValue.TotalTest);
            Assert.Equal(85, returnValue.FirstPass);
        }

        /// <summary>
        /// Tests the GetById method of QcPerformanceKpiController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((QcPerformanceKpi?)null);

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
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Create method of QcPerformanceKpiController to ensure it returns CreatedAtAction for valid creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidQcPerformanceKpi_ReturnsCreatedAtAction()
        {
            // Arrange
            var newQcPerformanceKpi = new AddQcPerformanceKpiRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                FirstPass = 85,
                SecondPass = 10,
                ThirdPass = 5
            };

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpi>(It.IsAny<AddQcPerformanceKpiRequestDTO>()))
                .Returns(new QcPerformanceKpi
                {
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 10,
                    ThirdPass = 5
                });

            _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<QcPerformanceKpi>()))
                .ReturnsAsync(new QcPerformanceKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 10,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                });

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpiDTO>(It.IsAny<QcPerformanceKpi>()))
                .Returns(new QcPerformanceKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 10,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.Create(newQcPerformanceKpi);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<QcPerformanceKpiDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
            Assert.Equal("01", returnValue.Month);
            Assert.Equal(100, returnValue.TotalTest);
            Assert.Equal(nameof(QcPerformanceKpiController.GetById), createdResult.ActionName);
        }

        /// <summary>
        /// Tests the Create method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Create_ThrowsException_Returns500Error()
        {
            // Arrange
            var newQcPerformanceKpi = new AddQcPerformanceKpiRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                FirstPass = 85
            };

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpi>(It.IsAny<AddQcPerformanceKpiRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Create(newQcPerformanceKpi);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Update method of QcPerformanceKpiController to ensure it returns Ok for valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidQcPerformanceKpi_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateQcPerformanceKpi = new UpdateQcPerformanceKpiRequestDTO
            {
                Id = 1,
                ProductId = 1,
                MachineId = 2,
                Year = "2024",
                Month = "02",
                TotalTest = 150,
                FirstPass = 130,
                SecondPass = 15,
                ThirdPass = 5
            };

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpi>(It.IsAny<UpdateQcPerformanceKpiRequestDTO>()))
                .Returns(new QcPerformanceKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 2,
                    Year = "2024",
                    Month = "02",
                    TotalTest = 150,
                    FirstPass = 130,
                    SecondPass = 15,
                    ThirdPass = 5
                });

            _mockRepository.Setup(repo => repo.UpdateAsync(1, It.IsAny<QcPerformanceKpi>()))
                .ReturnsAsync(new QcPerformanceKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 2,
                    Year = "2024",
                    Month = "02",
                    TotalTest = 150,
                    FirstPass = 130,
                    SecondPass = 15,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 2, Name = "Updated Machine", Status = true }
                });

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpiDTO>(It.IsAny<QcPerformanceKpi>()))
                .Returns(new QcPerformanceKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "02",
                    TotalTest = 150,
                    FirstPass = 130,
                    SecondPass = 15,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 2, Name = "Updated Machine", Status = true }
                });

            // Act
            var result = await _controller.Update(id, updateQcPerformanceKpi);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcPerformanceKpiDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
            Assert.Equal("02", returnValue.Month);
            Assert.Equal(150, returnValue.TotalTest);
            Assert.Equal(130, returnValue.FirstPass);
        }

        /// <summary>
        /// Tests the Update method of QcPerformanceKpiController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateQcPerformanceKpi = new UpdateQcPerformanceKpiRequestDTO
            {
                Id = 999,
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                FirstPass = 85
            };

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpi>(It.IsAny<UpdateQcPerformanceKpiRequestDTO>()))
                .Returns(new QcPerformanceKpi
                {
                    Id = 999,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85
                });

            _mockRepository.Setup(repo => repo.UpdateAsync(id, It.IsAny<QcPerformanceKpi>()))
                .ReturnsAsync((QcPerformanceKpi?)null);

            // Act
            var result = await _controller.Update(id, updateQcPerformanceKpi);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests the Update method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Update_ThrowsException_Returns500Error()
        {
            // Arrange
            var id = 1;
            var updateQcPerformanceKpi = new UpdateQcPerformanceKpiRequestDTO
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                FirstPass = 85
            };

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpi>(It.IsAny<UpdateQcPerformanceKpiRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Update(id, updateQcPerformanceKpi);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Delete method of QcPerformanceKpiController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var qcPerformanceKpiToDelete = new QcPerformanceKpi
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "01",
                TotalTest = 100,
                FirstPass = 85,
                SecondPass = 10,
                ThirdPass = 5,
                Product = new Product { Id = 1, Name = "Test Product", Status = true },
                Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _mockRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(qcPerformanceKpiToDelete);

            _mockMapper.Setup(mapper => mapper.Map<QcPerformanceKpiDTO>(It.IsAny<QcPerformanceKpi>()))
                .Returns(new QcPerformanceKpiDTO
                {
                    Id = 1,
                    Year = "2024",
                    Month = "01",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 10,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcPerformanceKpiDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("2024", returnValue.Year);
            Assert.Equal(100, returnValue.TotalTest);
        }

        /// <summary>
        /// Tests the Delete method of QcPerformanceKpiController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _mockRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((QcPerformanceKpi?)null);

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
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
