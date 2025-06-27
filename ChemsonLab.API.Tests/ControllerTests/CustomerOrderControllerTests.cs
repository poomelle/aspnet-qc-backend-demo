using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Customer;
using ChemsonLab.API.Models.DTO.CustomerOrder;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Repositories.CustomerOrderRepository;
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
    public class CustomerOrderControllerTests
    {
        private readonly Mock<ICustomerOrderRepository> _customerOrderRepositoryMock;
        private readonly Mock<ILogger<CustomerOrderController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CustomerOrderController _controller;

        public CustomerOrderControllerTests()
        {
            _customerOrderRepositoryMock = new Mock<ICustomerOrderRepository>();
            _loggerMock = new Mock<ILogger<CustomerOrderController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new CustomerOrderController(_customerOrderRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// Tests the GetAll method of CustomerOrderController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var customerOrderList = new List<CustomerOrder>
            {
                new CustomerOrder
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 1,
                    Status = true,
                    Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                }
            };

            _customerOrderRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, true))
                .ReturnsAsync(customerOrderList);

            _mapperMock.Setup(mapper => mapper.Map<List<CustomerOrderDTO>>(It.IsAny<List<CustomerOrder>>()))
                .Returns(new List<CustomerOrderDTO>
                {
                    new CustomerOrderDTO
                    {
                        Id = 1,
                        Status = true,
                        Customer = new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                        Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CustomerOrderDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of CustomerOrderController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var customerOrder = new CustomerOrder
            {
                Id = 1,
                CustomerId = 1,
                ProductId = 1,
                Status = true,
                Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                Product = new Product { Id = 1, Name = "Test Product", Status = true }
            };

            _customerOrderRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(customerOrder);

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrderDTO>(It.IsAny<CustomerOrder>()))
                .Returns(new CustomerOrderDTO
                {
                    Id = 1,
                    Status = true,
                    Customer = new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerOrderDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Test the GetById method of CustomerOrderController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _customerOrderRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of CustomerOrderController to ensure it returns CreatedAtAction for a valid customer order creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidCustomerOrder_ReturnsCreatedAtAction()
        {
            // Arrange
            var newCustomerOrder = new AddCustomerOrderRequestDTO
            {
                CustomerId = 1,
                ProductId = 1,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrder>(It.IsAny<AddCustomerOrderRequestDTO>()))
                .Returns(new CustomerOrder { Id = 1, CustomerId = 1, ProductId = 1, Status = true });

            _customerOrderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<CustomerOrder>()))
                .ReturnsAsync(new CustomerOrder
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 1,
                    Status = true,
                    Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new Product { Id = 1, Name = "Test Product", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrderDTO>(It.IsAny<CustomerOrder>()))
                .Returns(new CustomerOrderDTO
                {
                    Id = 1,
                    Status = true,
                    Customer = new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Create(newCustomerOrder);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CustomerOrderDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Test the Create method of CustomerOrderController to ensure it returns error for an invalid customer order creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidCustomerOrder_ReturnsError()
        {
            // Arrange
            var newCustomerOrder = new AddCustomerOrderRequestDTO
            {
                CustomerId = 0, // Invalid CustomerId
                ProductId = 1,
                Status = true
            };

            _controller.ModelState.AddModelError("CustomerId", "The CustomerId field must be greater than 0.");

            // Act
            var result = await _controller.Create(newCustomerOrder);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of CustomerOrderController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidCustomerOrder_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateCustomerOrder = new UpdateCustomerOrderRequestDTO
            {
                CustomerId = 1,
                ProductId = 2,
                Status = false
            };

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrder>(It.IsAny<UpdateCustomerOrderRequestDTO>()))
                .Returns(new CustomerOrder { Id = 1, CustomerId = 1, ProductId = 2, Status = false });

            _customerOrderRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<CustomerOrder>()))
                .ReturnsAsync(new CustomerOrder
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 2,
                    Status = false,
                    Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new Product { Id = 2, Name = "Updated Product", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrderDTO>(It.IsAny<CustomerOrder>()))
                .Returns(new CustomerOrderDTO
                {
                    Id = 1,
                    Status = false,
                    Customer = new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new ProductDTO { Id = 2, Name = "Updated Product", Status = true }
                });

            // Act
            var result = await _controller.Update(id, updateCustomerOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerOrderDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.False(returnValue.Status);
            Assert.Equal("Updated Product", returnValue.Product.Name);
        }

        /// <summary>
        /// Test the Update method of CustomerOrderController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateCustomerOrder = new UpdateCustomerOrderRequestDTO
            {
                CustomerId = 1,
                ProductId = 1,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrder>(It.IsAny<UpdateCustomerOrderRequestDTO>()))
                .Returns(new CustomerOrder { Id = 999, CustomerId = 1, ProductId = 1, Status = true });

            _customerOrderRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<CustomerOrder>()))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _controller.Update(id, updateCustomerOrder);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of CustomerOrderController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidCustomerOrder_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateCustomerOrder = new UpdateCustomerOrderRequestDTO
            {
                CustomerId = 0, // Invalid CustomerId
                ProductId = 1,
                Status = true
            };

            _controller.ModelState.AddModelError("CustomerId", "The CustomerId field must be greater than 0.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<CustomerOrder>(It.IsAny<UpdateCustomerOrderRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateCustomerOrder);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of CustomerOrderController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var customerOrderToDelete = new CustomerOrder
            {
                Id = 1,
                CustomerId = 1,
                ProductId = 1,
                Status = true,
                Customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                Product = new Product { Id = 1, Name = "Test Product", Status = true }
            };

            _customerOrderRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(customerOrderToDelete);

            _mapperMock.Setup(mapper => mapper.Map<CustomerOrderDTO>(It.IsAny<CustomerOrder>()))
                .Returns(new CustomerOrderDTO
                {
                    Id = 1,
                    Status = true,
                    Customer = new CustomerDTO { Id = 1, Name = "Test Customer", Email = "test@example.com", Status = true },
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerOrderDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Test the Delete method of CustomerOrderController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _customerOrderRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
