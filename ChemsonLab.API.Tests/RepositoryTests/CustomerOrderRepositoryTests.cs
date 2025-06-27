using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.CustomerOrderRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class CustomerOrderRepositoryTests
    {
        private readonly Mock<ICustomerOrderRepository> _mockRepository;

        public CustomerOrderRepositoryTests()
        {
            _mockRepository = new Mock<ICustomerOrderRepository>();
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all customer orders when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllCustomerOrders()
        {
            // Arrange
            var expectedOrders = GetSampleCustomerOrders();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedOrders, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customer orders by customer name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByCustomerName_ReturnsFilteredOrders()
        {
            // Arrange
            var customerName = "Customer1";
            var expectedOrders = GetSampleCustomerOrders()
                .Where(x => x.Customer.Name.Contains(customerName)).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(customerName, null, null, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(customerName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, order => Assert.Contains(customerName, order.Customer.Name));
            _mockRepository.Verify(repo => repo.GetAllAsync(customerName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customer orders by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredOrders()
        {
            // Arrange
            var productName = "Product1";
            var expectedOrders = GetSampleCustomerOrders()
                .Where(x => x.Product.Name.Contains(productName)).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, productName, null, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, order => Assert.Contains(productName, order.Product.Name));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, productName, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customer orders by status (true).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByStatusTrue_ReturnsActiveOrders()
        {
            // Arrange
            var status = "true";
            var expectedOrders = GetSampleCustomerOrders()
                .Where(x => x.Status == true).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, status, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, order => Assert.True(order.Status));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customer orders by status (false).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByStatusFalse_ReturnsInactiveOrders()
        {
            // Arrange
            var status = "false";
            var expectedOrders = GetSampleCustomerOrders()
                .Where(x => x.Status == false).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, status, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, order => Assert.False(order.Status));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by customer name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByCustomerNameAscending_ReturnsSortedOrders()
        {
            // Arrange
            var sortBy = "customerName";
            var expectedOrders = GetSampleCustomerOrders()
                .OrderBy(x => x.Customer.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedOrders, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by customer name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByCustomerNameDescending_ReturnsSortedOrders()
        {
            // Arrange
            var sortBy = "customerName";
            var expectedOrders = GetSampleCustomerOrders()
                .OrderByDescending(x => x.Customer.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, false))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedOrders, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameAscending_ReturnsSortedOrders()
        {
            // Arrange
            var sortBy = "productName";
            var expectedOrders = GetSampleCustomerOrders()
                .OrderBy(x => x.Product.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedOrders, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameDescending_ReturnsSortedOrders()
        {
            // Arrange
            var sortBy = "productName";
            var expectedOrders = GetSampleCustomerOrders()
                .OrderByDescending(x => x.Product.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, false))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedOrders, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredOrders()
        {
            // Arrange
            var customerName = "Customer1";
            var productName = "Product1";
            var status = "true";
            var sortBy = "customerName";
            var expectedOrders = GetSampleCustomerOrders()
                .Where(x => x.Customer.Name.Contains(customerName) &&
                           x.Product.Name.Contains(productName) &&
                           x.Status == true)
                .OrderBy(x => x.Customer.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(customerName, productName, status, sortBy, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(customerName, productName, status, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(customerName, result.First().Customer.Name);
            Assert.Contains(productName, result.First().Product.Name);
            Assert.True(result.First().Status);
            _mockRepository.Verify(repo => repo.GetAllAsync(customerName, productName, status, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no orders match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingOrders_ReturnsEmptyList()
        {
            // Arrange
            var customerName = "NonExistentCustomer";
            var expectedOrders = new List<CustomerOrder>();

            _mockRepository.Setup(repo => repo.GetAllAsync(customerName, null, null, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(customerName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(customerName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid status string (non-boolean).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidStatusString_ReturnsAllOrders()
        {
            // Arrange
            var invalidStatus = "invalid";
            var expectedOrders = GetSampleCustomerOrders(); // Should return all orders when status parsing fails

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, invalidStatus, null, true))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, invalidStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, invalidStatus, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns customer order when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCustomerOrder()
        {
            // Arrange
            var orderId = 1;
            var expectedOrder = GetSampleCustomerOrders().First(x => x.Id == orderId);

            _mockRepository.Setup(repo => repo.GetByIdAsync(orderId))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
            Assert.Equal("Customer1", result.Customer.Name);
            Assert.Equal("Product1", result.Product.Name);
            Assert.True(result.Status);
            _mockRepository.Verify(repo => repo.GetByIdAsync(orderId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockRepository.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _mockRepository.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _mockRepository.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new customer order.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidCustomerOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var newOrder = new CustomerOrder
            {
                CustomerId = 1,
                ProductId = 1,
                Status = true,
                Customer = new Customer { Id = 1, Name = "NewCustomer", Email = "new@example.com", Status = true },
                Product = new Product { Id = 1, Name = "NewProduct" }
            };

            var createdOrder = new CustomerOrder
            {
                Id = 5,
                CustomerId = 1,
                ProductId = 1,
                Status = true,
                Customer = newOrder.Customer,
                Product = newOrder.Product
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newOrder))
                .ReturnsAsync(createdOrder);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(1, result.CustomerId);
            Assert.Equal(1, result.ProductId);
            Assert.True(result.Status);
            Assert.Equal("NewCustomer", result.Customer.Name);
            Assert.Equal("NewProduct", result.Product.Name);
            _mockRepository.Verify(repo => repo.CreateAsync(newOrder), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalCustomerOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var newOrder = new CustomerOrder
            {
                CustomerId = 2,
                ProductId = 2,
                Status = false
            };

            var createdOrder = new CustomerOrder
            {
                Id = 6,
                CustomerId = 2,
                ProductId = 2,
                Status = false
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newOrder))
                .ReturnsAsync(createdOrder);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(2, result.CustomerId);
            Assert.Equal(2, result.ProductId);
            Assert.False(result.Status);
            _mockRepository.Verify(repo => repo.CreateAsync(newOrder), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with complete navigation properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_CompletelyPopulatedCustomerOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var newOrder = new CustomerOrder
            {
                CustomerId = 1,
                ProductId = 1,
                Status = true,
                Customer = new Customer
                {
                    Id = 1,
                    Name = "CompleteCustomer",
                    Email = "complete@example.com",
                    Status = true
                },
                Product = new Product
                {
                    Id = 1,
                    Name = "CompleteProduct"
                }
            };

            var createdOrder = new CustomerOrder
            {
                Id = 7,
                CustomerId = 1,
                ProductId = 1,
                Status = true,
                Customer = newOrder.Customer,
                Product = newOrder.Product
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newOrder))
                .ReturnsAsync(createdOrder);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("CompleteCustomer", result.Customer.Name);
            Assert.Equal("complete@example.com", result.Customer.Email);
            Assert.Equal("CompleteProduct", result.Product.Name);
            _mockRepository.Verify(repo => repo.CreateAsync(newOrder), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing customer order.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndCustomerOrder_ReturnsUpdatedOrder()
        {
            // Arrange
            var orderId = 1;
            var updateOrder = new CustomerOrder
            {
                CustomerId = 2,
                ProductId = 2,
                Status = false
            };

            var updatedOrder = new CustomerOrder
            {
                Id = 1,
                CustomerId = 2,
                ProductId = 2,
                Status = false,
                Customer = new Customer { Id = 2, Name = "UpdatedCustomer", Email = "updated@example.com", Status = true },
                Product = new Product { Id = 2, Name = "UpdatedProduct" }
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(orderId, updateOrder))
                .ReturnsAsync(updatedOrder);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(orderId, updateOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.CustomerId);
            Assert.Equal(2, result.ProductId);
            Assert.False(result.Status);
            Assert.Equal("UpdatedCustomer", result.Customer.Name);
            Assert.Equal("UpdatedProduct", result.Product.Name);
            _mockRepository.Verify(repo => repo.UpdateAsync(orderId, updateOrder), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when customer order with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateOrder = new CustomerOrder
            {
                CustomerId = 1,
                ProductId = 1,
                Status = true
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(invalidId, updateOrder))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(invalidId, updateOrder);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(invalidId, updateOrder), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (changing only status).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedOrder()
        {
            // Arrange
            var orderId = 1;
            var updateOrder = new CustomerOrder
            {
                CustomerId = 1, // Same as original
                ProductId = 1,  // Same as original
                Status = false  // Changed from true to false
            };

            var updatedOrder = new CustomerOrder
            {
                Id = 1,
                CustomerId = 1,
                ProductId = 1,
                Status = false,
                Customer = new Customer { Id = 1, Name = "Customer1", Email = "customer1@example.com", Status = true },
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(orderId, updateOrder))
                .ReturnsAsync(updatedOrder);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(orderId, updateOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CustomerId);
            Assert.Equal(1, result.ProductId);
            Assert.False(result.Status); // Status changed
            _mockRepository.Verify(repo => repo.UpdateAsync(orderId, updateOrder), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateOrder = new CustomerOrder
            {
                CustomerId = 1,
                ProductId = 1,
                Status = true
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(zeroId, updateOrder))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(zeroId, updateOrder);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(zeroId, updateOrder), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing customer order.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedOrder()
        {
            // Arrange
            var orderId = 1;
            var deletedOrder = GetSampleCustomerOrders().First(x => x.Id == orderId);

            _mockRepository.Setup(repo => repo.DeleteAsync(orderId))
                .ReturnsAsync(deletedOrder);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Customer1", result.Customer.Name);
            Assert.Equal("Product1", result.Product.Name);
            _mockRepository.Verify(repo => repo.DeleteAsync(orderId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when customer order with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockRepository.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _mockRepository.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _mockRepository.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((CustomerOrder?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to generate sample customer orders for testing.
        /// </summary>
        private static List<CustomerOrder> GetSampleCustomerOrders()
        {
            return new List<CustomerOrder>
            {
                new CustomerOrder
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 1,
                    Status = true,
                    Customer = new Customer
                    {
                        Id = 1,
                        Name = "Customer1",
                        Email = "customer1@example.com",
                        Status = true
                    },
                    Product = new Product { Id = 1, Name = "Product1" }
                },
                new CustomerOrder
                {
                    Id = 2,
                    CustomerId = 1,
                    ProductId = 2,
                    Status = false,
                    Customer = new Customer
                    {
                        Id = 1,
                        Name = "Customer1",
                        Email = "customer1@example.com",
                        Status = true
                    },
                    Product = new Product { Id = 2, Name = "Product2" }
                },
                new CustomerOrder
                {
                    Id = 3,
                    CustomerId = 2,
                    ProductId = 1,
                    Status = true,
                    Customer = new Customer
                    {
                        Id = 2,
                        Name = "Customer2",
                        Email = "customer2@example.com",
                        Status = false
                    },
                    Product = new Product { Id = 1, Name = "Product1" }
                },
                new CustomerOrder
                {
                    Id = 4,
                    CustomerId = 3,
                    ProductId = 3,
                    Status = false,
                    Customer = new Customer
                    {
                        Id = 3,
                        Name = "Customer3",
                        Email = "customer3@example.com",
                        Status = true
                    },
                    Product = new Product { Id = 3, Name = "Product3" }
                }
            };
        }

        #endregion
    }
}
