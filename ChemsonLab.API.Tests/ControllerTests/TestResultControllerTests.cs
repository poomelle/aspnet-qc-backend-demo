using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Machine;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Models.DTO.TestResult;
using ChemsonLab.API.Repositories.TestResultRepository;
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
    public class TestResultControllerTests
    {
        private readonly Mock<ITestResultRepository> _testResultRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TestResultController>> _loggerMock;
        private readonly TestResultController _controller;

        public TestResultControllerTests()
        {
            _testResultRepositoryMock = new Mock<ITestResultRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TestResultController>>();

            _controller = new TestResultController(
                _testResultRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of TestResultController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var testResultList = new List<TestResult>
            {
                new TestResult
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    TestDate = DateTime.Now,
                    OperatorName = "Test Operator",
                    TestType = "Quality Test",
                    Status = true,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                }
            };

            _testResultRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(testResultList);

            _mapperMock.Setup(mapper => mapper.Map<List<TestResultDTO>>(It.IsAny<List<TestResult>>()))
                .Returns(new List<TestResultDTO>
                {
                    new TestResultDTO
                    {
                        Id = 1,
                        TestDate = DateTime.Now.ToString(),
                        OperatorName = "Test Operator",
                        TestType = "Quality Test",
                        Status = true,
                        Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                        Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                    }
                });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TestResultDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests the GetAll method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetAll_ThrowsException_Returns500Error()
        {
            // Arrange
            _testResultRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the GetById method of TestResultController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var testResult = new TestResult
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                TestDate = DateTime.Now,
                OperatorName = "Test Operator",
                TestType = "Quality Test",
                Speed = 100.5,
                TestTime = 60.0,
                Status = true,
                Product = new Product { Id = 1, Name = "Test Product", Status = true },
                Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(testResult);

            _mapperMock.Setup(mapper => mapper.Map<TestResultDTO>(It.IsAny<TestResult>()))
                .Returns(new TestResultDTO
                {
                    Id = 1,
                    TestDate = DateTime.Now.ToString(),
                    OperatorName = "Test Operator",
                    TestType = "Quality Test",
                    Speed = 100.5,
                    TestTime = 60.0,
                    Status = true,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TestResultDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Operator", returnValue.OperatorName);
            Assert.Equal("Quality Test", returnValue.TestType);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Tests the GetById method of TestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((TestResult?)null);

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
            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Create method of TestResultController to ensure it returns CreatedAtAction for valid creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidTestResult_ReturnsCreatedAtAction()
        {
            // Arrange
            var newTestResult = new AddTestResultRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = DateTime.Now.ToString(),
                OperatorName = "Test Operator",
                TestType = "Quality Test",
                Speed = 100.5,
                TestTime = 60.0,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResult>(It.IsAny<AddTestResultRequestDTO>()))
                .Returns(new TestResult
                {
                    ProductId = 1,
                    MachineId = 1,
                    TestDate = DateTime.Now,
                    OperatorName = "Test Operator",
                    TestType = "Quality Test",
                    Speed = 100.5,
                    TestTime = 60.0,
                    Status = true
                });

            _testResultRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TestResult>()))
                .ReturnsAsync(new TestResult
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    TestDate = DateTime.Now,
                    OperatorName = "Test Operator",
                    TestType = "Quality Test",
                    Speed = 100.5,
                    TestTime = 60.0,
                    Status = true,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<TestResultDTO>(It.IsAny<TestResult>()))
                .Returns(new TestResultDTO
                {
                    Id = 1,
                    TestDate = DateTime.Now.ToString(),
                    OperatorName = "Test Operator",
                    TestType = "Quality Test",
                    Speed = 100.5,
                    TestTime = 60.0,
                    Status = true,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.Create(newTestResult);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TestResultDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Operator", returnValue.OperatorName);
            Assert.Equal("Quality Test", returnValue.TestType);
            Assert.Equal(nameof(TestResultController.GetById), createdResult.ActionName);
        }

        /// <summary>
        /// Tests the Create method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Create_ThrowsException_Returns500Error()
        {
            // Arrange
            var newTestResult = new AddTestResultRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = DateTime.Now.ToString(),
                OperatorName = "Test Operator",
                TestType = "Quality Test"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResult>(It.IsAny<AddTestResultRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Create(newTestResult);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Update method of TestResultController to ensure it returns Ok for valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidTestResult_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateTestResult = new UpdateTestResultRequestDTO
            {
                ProductId = 1,
                MachineId = 2,
                TestDate = DateTime.Now.ToString(),
                OperatorName = "Updated Operator",
                TestType = "Updated Test",
                Speed = 150.0,
                TestTime = 90.0,
                Status = false
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResult>(It.IsAny<UpdateTestResultRequestDTO>()))
                .Returns(new TestResult
                {
                    ProductId = 1,
                    MachineId = 2,
                    TestDate = DateTime.Now,
                    OperatorName = "Updated Operator",
                    TestType = "Updated Test",
                    Speed = 150.0,
                    TestTime = 90.0,
                    Status = false
                });

            _testResultRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<TestResult>()))
                .ReturnsAsync(new TestResult
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 2,
                    TestDate = DateTime.Now,
                    OperatorName = "Updated Operator",
                    TestType = "Updated Test",
                    Speed = 150.0,
                    TestTime = 90.0,
                    Status = false,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 2, Name = "Updated Machine", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<TestResultDTO>(It.IsAny<TestResult>()))
                .Returns(new TestResultDTO
                {
                    Id = 1,
                    TestDate = DateTime.Now.ToString(),
                    OperatorName = "Updated Operator",
                    TestType = "Updated Test",
                    Speed = 150.0,
                    TestTime = 90.0,
                    Status = false,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 2, Name = "Updated Machine", Status = true }
                });

            // Act
            var result = await _controller.Update(id, updateTestResult);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TestResultDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Updated Operator", returnValue.OperatorName);
            Assert.Equal("Updated Test", returnValue.TestType);
            Assert.False(returnValue.Status);
        }

        /// <summary>
        /// Tests the Update method of TestResultController to ensure it returns NotFound when mapped domain is null.
        /// </summary>
        [Fact]
        public async Task Update_NullMappedDomain_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            var updateTestResult = new UpdateTestResultRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = DateTime.Now.ToString(),
                OperatorName = "Test Operator"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResult>(It.IsAny<UpdateTestResultRequestDTO>()))
                .Returns((TestResult?)null);

            // Act
            var result = await _controller.Update(id, updateTestResult);

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
            var updateTestResult = new UpdateTestResultRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = DateTime.Now.ToString(),
                OperatorName = "Test Operator"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResult>(It.IsAny<UpdateTestResultRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Update(id, updateTestResult);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Delete method of TestResultController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var testResultToDelete = new TestResult
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                TestDate = DateTime.Now,
                OperatorName = "Test Operator",
                TestType = "Quality Test",
                Status = true,
                Product = new Product { Id = 1, Name = "Test Product", Status = true },
                Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(testResultToDelete);

            _mapperMock.Setup(mapper => mapper.Map<TestResultDTO>(It.IsAny<TestResult>()))
                .Returns(new TestResultDTO
                {
                    Id = 1,
                    TestDate = DateTime.Now.ToString(),
                    OperatorName = "Test Operator",
                    TestType = "Quality Test",
                    Status = true,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TestResultDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Operator", returnValue.OperatorName);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Tests the Delete method of TestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((TestResult?)null);

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
            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
