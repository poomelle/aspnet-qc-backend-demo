using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.QcLabel;
using ChemsonLab.API.Repositories.QcLabelRepository;
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
    public class QcLabelControllerTests
    {
        private readonly QcLabelController _controller;
        private readonly Mock<IQcLabelRepository> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<QcLabelController>> _mockLogger;

        public QcLabelControllerTests()
        {
            _mockService = new Mock<IQcLabelRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<QcLabelController>>();
            _controller = new QcLabelController(_mockService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        /// <summary>
        /// Tests the GetAll method of QcLabelController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var qcLabelList = new List<QcLabel>
            {
                new QcLabel
                {
                    Id = 1,
                    BatchName = "Test Batch",
                    Printed = false,
                    ProductId = 1,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                }
            };

            _mockService.Setup(repo => repo.GetAllAsync(null, null, null, null, null))
                .ReturnsAsync(qcLabelList);

            _mockMapper.Setup(mapper => mapper.Map<List<QcLabelDTO>>(It.IsAny<List<QcLabel>>()))
                .Returns(new List<QcLabelDTO>
                {
                    new QcLabelDTO
                    {
                        Id = 1,
                        BatchName = "Test Batch",
                        Printed = false,
                        Year = "2024",
                        Month = "01",
                        Product = new Product { Id = 1, Name = "Test Product", Status = true }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<QcLabelDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests the GetAll method with filters to ensure it passes parameters correctly.
        /// </summary>
        [Fact]
        public async Task GetAll_WithFilters_ReturnsOkResult()
        {
            // Arrange
            var batchName = "Test Batch";
            var productName = "Test Product";
            var printed = "true";
            var year = "2024";
            var month = "01";

            var qcLabelList = new List<QcLabel>
            {
                new QcLabel
                {
                    Id = 1,
                    BatchName = batchName,
                    Printed = true,
                    Year = year,
                    Month = month
                }
            };

            _mockService.Setup(repo => repo.GetAllAsync(batchName, productName, printed, year, month))
                .ReturnsAsync(qcLabelList);

            _mockMapper.Setup(mapper => mapper.Map<List<QcLabelDTO>>(It.IsAny<List<QcLabel>>()))
                .Returns(new List<QcLabelDTO>());

            // Act
            var result = await _controller.GetAll(batchName, productName, printed, year, month);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockService.Verify(repo => repo.GetAllAsync(batchName, productName, printed, year, month), Times.Once);
        }

        /// <summary>
        /// Tests the GetAll method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetAll_ThrowsException_Returns500Error()
        {
            // Arrange
            _mockService.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAll(null, null, null, null, null);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the GetById method of QcLabelController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var qcLabel = new QcLabel
            {
                Id = 1,
                BatchName = "Test Batch",
                Printed = false,
                ProductId = 1,
                Year = "2024",
                Month = "01",
                Product = new Product { Id = 1, Name = "Test Product", Status = true }
            };

            _mockService.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(qcLabel);

            _mockMapper.Setup(mapper => mapper.Map<QcLabelDTO>(It.IsAny<QcLabel>()))
                .Returns(new QcLabelDTO
                {
                    Id = 1,
                    BatchName = "Test Batch",
                    Printed = false,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcLabelDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Batch", returnValue.BatchName);
            Assert.False(returnValue.Printed);
        }

        /// <summary>
        /// Tests the GetById method of QcLabelController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((QcLabel?)null);

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
            _mockService.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Create method of QcLabelController to ensure it returns CreatedAtAction for valid creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidQcLabel_ReturnsCreatedAtAction()
        {
            // Arrange
            var newQcLabel = new AddQcLabelRequestDTO
            {
                BatchName = "New Batch",
                ProductId = 1,
                Printed = false
            };

            _mockMapper.Setup(mapper => mapper.Map<QcLabel>(It.IsAny<AddQcLabelRequestDTO>()))
                .Returns(new QcLabel { BatchName = "New Batch", ProductId = 1, Printed = false });

            _mockService.Setup(repo => repo.CreateAsync(It.IsAny<QcLabel>()))
                .ReturnsAsync(new QcLabel
                {
                    Id = 1,
                    BatchName = "New Batch",
                    ProductId = 1,
                    Printed = false,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            _mockMapper.Setup(mapper => mapper.Map<QcLabelDTO>(It.IsAny<QcLabel>()))
                .Returns(new QcLabelDTO
                {
                    Id = 1,
                    BatchName = "New Batch",
                    Printed = false,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Create(newQcLabel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<QcLabelDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("New Batch", returnValue.BatchName);
            Assert.Equal(nameof(QcLabelController.GetById), createdResult.ActionName);
        }

        /// <summary>
        /// Tests the Create method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Create_ThrowsException_Returns500Error()
        {
            // Arrange
            var newQcLabel = new AddQcLabelRequestDTO
            {
                BatchName = "New Batch",
                ProductId = 1,
                Printed = false
            };

            _mockMapper.Setup(mapper => mapper.Map<QcLabel>(It.IsAny<AddQcLabelRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Create(newQcLabel);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Update method of QcLabelController to ensure it returns Ok for valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidQcLabel_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateQcLabel = new UpdateQcLabelRequestDTO
            {
                BatchName = "Updated Batch",
                ProductId = 2,
                Printed = true
            };

            _mockMapper.Setup(mapper => mapper.Map<QcLabel>(It.IsAny<UpdateQcLabelRequestDTO>()))
                .Returns(new QcLabel { BatchName = "Updated Batch", ProductId = 2, Printed = true });

            _mockService.Setup(repo => repo.UpdateAsync(1, It.IsAny<QcLabel>()))
                .ReturnsAsync(new QcLabel
                {
                    Id = 1,
                    BatchName = "Updated Batch",
                    ProductId = 2,
                    Printed = true,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 2, Name = "Updated Product", Status = true }
                });

            _mockMapper.Setup(mapper => mapper.Map<QcLabelDTO>(It.IsAny<QcLabel>()))
                .Returns(new QcLabelDTO
                {
                    Id = 1,
                    BatchName = "Updated Batch",
                    Printed = true,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 2, Name = "Updated Product", Status = true }
                });

            // Act
            var result = await _controller.Update(id, updateQcLabel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcLabelDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Updated Batch", returnValue.BatchName);
            Assert.True(returnValue.Printed);
        }

        /// <summary>
        /// Tests the Update method of QcLabelController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateQcLabel = new UpdateQcLabelRequestDTO
            {
                BatchName = "Updated Batch",
                ProductId = 1,
                Printed = true
            };

            _mockMapper.Setup(mapper => mapper.Map<QcLabel>(It.IsAny<UpdateQcLabelRequestDTO>()))
                .Returns(new QcLabel { BatchName = "Updated Batch", ProductId = 1, Printed = true });

            _mockService.Setup(repo => repo.UpdateAsync(id, It.IsAny<QcLabel>()))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _controller.Update(id, updateQcLabel);

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
            var updateQcLabel = new UpdateQcLabelRequestDTO
            {
                BatchName = "Updated Batch",
                ProductId = 1,
                Printed = true
            };

            _mockMapper.Setup(mapper => mapper.Map<QcLabel>(It.IsAny<UpdateQcLabelRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Update(id, updateQcLabel);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Delete method of QcLabelController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var qcLabelToDelete = new QcLabel
            {
                Id = 1,
                BatchName = "Test Batch",
                ProductId = 1,
                Printed = false,
                Year = "2024",
                Month = "01",
                Product = new Product { Id = 1, Name = "Test Product", Status = true }
            };

            _mockService.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(qcLabelToDelete);

            _mockMapper.Setup(mapper => mapper.Map<QcLabelDTO>(It.IsAny<QcLabel>()))
                .Returns(new QcLabelDTO
                {
                    Id = 1,
                    BatchName = "Test Batch",
                    Printed = false,
                    Year = "2024",
                    Month = "01",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QcLabelDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Tests the Delete method of QcLabelController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _mockService.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((QcLabel?)null);

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
            _mockService.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
