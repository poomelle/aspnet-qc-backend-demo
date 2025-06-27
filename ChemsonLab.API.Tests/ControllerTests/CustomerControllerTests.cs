using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Customer;
using ChemsonLab.API.Repositories.CustomerRepository;
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
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly Mock<ILogger<CustomerController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _mockLogger = new Mock<ILogger<CustomerController>>();
            _mockMapper = new Mock<IMapper>();

            _controller = new CustomerController(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        /// <summary>
        /// Tests the GetAll method of CustomerController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var customerList = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(customerList);

            _mockMapper.Setup(mapper => mapper.Map<List<CustomerDTO>>(It.IsAny<List<Customer>>()))
                .Returns(new List<CustomerDTO>
                {
                    new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CustomerDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of CustomerController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true };
            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(customer);
            _mockMapper.Setup(mapper => mapper.Map<CustomerDTO>(It.IsAny<Customer>()))
                .Returns(new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal("Test Customer", returnValue.Name);
            Assert.Equal("test@example.com", returnValue.Email);
        }

        /// <summary>
        /// Test the GetById method of CustomerController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of CustomerController to ensure it returns CreatedAtAction for a valid customer creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidCustomer_ReturnsCreatedAtAction()
        {
            // Arrange
            var newCustomer = new AddCustomerRequestDTO
            {
                Name = "New Customer",
                Email = "new@example.com",
                Status = true
            };

            _mockMapper.Setup(mapper => mapper.Map<Customer>(It.IsAny<AddCustomerRequestDTO>()))
                .Returns(new Customer { Id = 1, Name = "New Customer", Email = "new@example.com", Status = true });

            _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer { Id = 1, Name = "New Customer", Email = "new@example.com", Status = true });

            _mockMapper.Setup(mapper => mapper.Map<CustomerDTO>(It.IsAny<Customer>()))
                .Returns(new CustomerDTO { Id = 1, Name = "New Customer", Email = "new@example.com", Status = true });

            // Act
            var result = await _controller.Create(newCustomer);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CustomerDTO>(createdResult.Value);
            Assert.Equal("New Customer", returnValue.Name);
            Assert.Equal("new@example.com", returnValue.Email);
        }

        /// <summary>
        /// Test the Create method of CustomerController to ensure it returns error for an invalid customer creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidCustomer_ReturnsError()
        {
            // Arrange
            var newCustomer = new AddCustomerRequestDTO { Name = "New Customer" }; // Missing required Email
            _controller.ModelState.AddModelError("Email", "The Email field is required.");

            // Act
            var result = await _controller.Create(newCustomer);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of CustomerController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateCustomer = new UpdateCustomerRequestDTO
            {
                Name = "Updated Customer",
                Email = "updated@example.com",
                Status = false
            };

            _mockMapper.Setup(mapper => mapper.Map<Customer>(It.IsAny<UpdateCustomerRequestDTO>()))
                .Returns(new Customer { Id = 1, Name = "Updated Customer", Email = "updated@example.com", Status = false });

            _mockRepository.Setup(repo => repo.UpdateAsync(1, It.IsAny<Customer>()))
                .ReturnsAsync(new Customer { Id = 1, Name = "Updated Customer", Email = "updated@example.com", Status = false });

            _mockMapper.Setup(mapper => mapper.Map<CustomerDTO>(It.IsAny<Customer>()))
                .Returns(new CustomerDTO { Id = 1, Name = "Updated Customer", Email = "updated@example.com", Status = false });

            // Act
            var result = await _controller.Update(id, updateCustomer);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal("Updated Customer", returnValue.Name);
            Assert.Equal("updated@example.com", returnValue.Email);
            Assert.False(returnValue.Status);
        }

        /// <summary>
        /// Test the Update method of CustomerController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateCustomer = new UpdateCustomerRequestDTO
            {
                Name = "Updated Customer",
                Email = "updated@example.com",
                Status = true
            };

            _mockMapper.Setup(mapper => mapper.Map<Customer>(It.IsAny<UpdateCustomerRequestDTO>()))
                .Returns(new Customer { Id = 999, Name = "Updated Customer", Email = "updated@example.com", Status = true });

            _mockRepository.Setup(repo => repo.UpdateAsync(id, It.IsAny<Customer>()))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _controller.Update(id, updateCustomer);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of CustomerController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidCustomer_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateCustomer = new UpdateCustomerRequestDTO { Name = "Updated Customer" }; // Missing required Email
            _controller.ModelState.AddModelError("Email", "The Email field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mockMapper.Setup(mapper => mapper.Map<Customer>(It.IsAny<UpdateCustomerRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateCustomer);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of CustomerController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var customerToDelete = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true };

            _mockRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(customerToDelete);

            _mockMapper.Setup(mapper => mapper.Map<CustomerDTO?>(It.IsAny<Customer>()))
                .Returns(new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Customer", returnValue.Name);
        }

        /// <summary>
        /// Test the Delete method of CustomerController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _mockRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
