using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.DailyQc;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Repositories.DailyQcRepository;
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
    public class DailyQcControllerTests
    {
        private readonly Mock<IDailyQcRepository> _dailyQcRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<DailyQcController>> _loggerMock;
        private readonly DailyQcController _controller;

        public DailyQcControllerTests()
        {
            _dailyQcRepositoryMock = new Mock<IDailyQcRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<DailyQcController>>();
            _controller = new DailyQcController(_dailyQcRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// Tests the GetAll method of DailyQcController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var dailyQcList = new List<DailyQc>
            {
                new DailyQc
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    ProductId = 1,
                    Priority = 1,
                    Comment = "Test Comment",
                    TestStatus = "Pending",
                    Year = "2024",
                    Month = "January",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(dailyQcList);

            _mapperMock.Setup(mapper => mapper.Map<List<DailyQcDTO>>(It.IsAny<List<DailyQc>>()))
                .Returns(new List<DailyQcDTO>
                {
                    new DailyQcDTO
                    {
                        Id = 1,
                        IncomingDate = DateTime.Now,
                        Priority = 1,
                        Comment = "Test Comment",
                        TestStatus = "Pending",
                        Year = "2024",
                        Month = "January",
                        Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<DailyQcDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of DailyQcController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var dailyQc = new DailyQc
            {
                Id = 1,
                IncomingDate = DateTime.Now,
                ProductId = 1,
                Priority = 1,
                Comment = "Test Comment",
                TestStatus = "Pending",
                Year = "2024",
                Month = "January",
                Product = new Product { Id = 1, Name = "Test Product", Status = true }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(dailyQc);

            _mapperMock.Setup(mapper => mapper.Map<DailyQcDTO>(It.IsAny<DailyQc>()))
                .Returns(new DailyQcDTO
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    Priority = 1,
                    Comment = "Test Comment",
                    TestStatus = "Pending",
                    Year = "2024",
                    Month = "January",
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DailyQcDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Comment", returnValue.Comment);
            Assert.Equal("Pending", returnValue.TestStatus);
        }

        /// <summary>
        /// Test the GetById method of DailyQcController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _dailyQcRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of DailyQcController to ensure it returns CreatedAtAction for a valid DailyQc creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidDailyQc_ReturnsCreatedAtAction()
        {
            // Arrange
            var newDailyQc = new AddDailyQcRequestDTO
            {
                IncomingDate = DateTime.Now,
                ProductId = 1,
                Priority = 1,
                Comment = "New Comment",
                TestStatus = "Pending",
                Year = "2024",
                Month = "January"
            };

            _mapperMock.Setup(mapper => mapper.Map<DailyQc>(It.IsAny<AddDailyQcRequestDTO>()))
                .Returns(new DailyQc
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    ProductId = 1,
                    Priority = 1,
                    Comment = "New Comment",
                    TestStatus = "Pending",
                    Year = "2024",
                    Month = "January"
                });

            _dailyQcRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<DailyQc>()))
                .ReturnsAsync(new DailyQc
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    ProductId = 1,
                    Priority = 1,
                    Comment = "New Comment",
                    TestStatus = "Pending",
                    Year = "2024",
                    Month = "January",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<DailyQcDTO>(It.IsAny<DailyQc>()))
                .Returns(new DailyQcDTO
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    Priority = 1,
                    Comment = "New Comment",
                    TestStatus = "Pending",
                    Year = "2024",
                    Month = "January",
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Create(newDailyQc);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<DailyQcDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("New Comment", returnValue.Comment);
            Assert.Equal("Pending", returnValue.TestStatus);
        }

        /// <summary>
        /// Test the Create method of DailyQcController to ensure it returns error for an invalid DailyQc creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidDailyQc_ReturnsError()
        {
            // Arrange
            var newDailyQc = new AddDailyQcRequestDTO
            {
                // Missing required fields
                ProductId = 0
            };

            _controller.ModelState.AddModelError("ProductId", "The ProductId field is required.");

            // Act
            var result = await _controller.Create(newDailyQc);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of DailyQcController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidDailyQc_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateDailyQc = new UpdateDailyQcRequestDTO
            {
                IncomingDate = DateTime.Now,
                ProductId = 1,
                Priority = 2,
                Comment = "Updated Comment",
                TestStatus = "Completed",
                Year = "2024",
                Month = "February"
            };

            _mapperMock.Setup(mapper => mapper.Map<DailyQc>(It.IsAny<UpdateDailyQcRequestDTO>()))
                .Returns(new DailyQc
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    ProductId = 1,
                    Priority = 2,
                    Comment = "Updated Comment",
                    TestStatus = "Completed",
                    Year = "2024",
                    Month = "February"
                });

            _dailyQcRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<DailyQc>()))
                .ReturnsAsync(new DailyQc
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    ProductId = 1,
                    Priority = 2,
                    Comment = "Updated Comment",
                    TestStatus = "Completed",
                    Year = "2024",
                    Month = "February",
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<DailyQcDTO>(It.IsAny<DailyQc>()))
                .Returns(new DailyQcDTO
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    Priority = 2,
                    Comment = "Updated Comment",
                    TestStatus = "Completed",
                    Year = "2024",
                    Month = "February",
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Update(id, updateDailyQc);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DailyQcDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Updated Comment", returnValue.Comment);
            Assert.Equal("Completed", returnValue.TestStatus);
            Assert.Equal(2, returnValue.Priority);
        }

        /// <summary>
        /// Test the Update method of DailyQcController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateDailyQc = new UpdateDailyQcRequestDTO
            {
                IncomingDate = DateTime.Now,
                ProductId = 1,
                Priority = 1,
                Comment = "Updated Comment",
                TestStatus = "Pending"
            };

            _mapperMock.Setup(mapper => mapper.Map<DailyQc>(It.IsAny<UpdateDailyQcRequestDTO>()))
                .Returns(new DailyQc { Id = 999, ProductId = 1, Comment = "Updated Comment" });

            _dailyQcRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<DailyQc>()))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _controller.Update(id, updateDailyQc);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of DailyQcController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidDailyQc_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateDailyQc = new UpdateDailyQcRequestDTO
            {
                ProductId = 0 // Invalid ProductId
            };

            _controller.ModelState.AddModelError("ProductId", "The ProductId field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<DailyQc>(It.IsAny<UpdateDailyQcRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateDailyQc);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of DailyQcController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var dailyQcToDelete = new DailyQc
            {
                Id = 1,
                IncomingDate = DateTime.Now,
                ProductId = 1,
                Priority = 1,
                Comment = "Test Comment",
                TestStatus = "Pending",
                Year = "2024",
                Month = "January",
                Product = new Product { Id = 1, Name = "Test Product", Status = true }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(dailyQcToDelete);

            _mapperMock.Setup(mapper => mapper.Map<DailyQcDTO>(It.IsAny<DailyQc>()))
                .Returns(new DailyQcDTO
                {
                    Id = 1,
                    IncomingDate = DateTime.Now,
                    Priority = 1,
                    Comment = "Test Comment",
                    TestStatus = "Pending",
                    Year = "2024",
                    Month = "January",
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DailyQcDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Comment", returnValue.Comment);
            Assert.Equal("Pending", returnValue.TestStatus);
        }

        /// <summary>
        /// Test the Delete method of DailyQcController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _dailyQcRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
