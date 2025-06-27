using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.CustomerRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class CustomerRepositoryTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        public CustomerRepositoryTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
        }


        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all customers when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllCustomers()
        {
            // Arrange
            var expectedCustomers = GetSampleCustomers();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedCustomers, result);
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customers by name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByName_ReturnsFilteredCustomers()
        {
            // Arrange
            var name = "John";
            var expectedCustomers = GetSampleCustomers()
                .Where(x => x.Name.Contains(name)).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, customer => Assert.Contains(name, customer.Name));
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customers by status (true).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByStatusTrue_ReturnsActiveCustomers()
        {
            // Arrange
            var status = "true";
            var expectedCustomers = GetSampleCustomers()
                .Where(x => x.Status == true).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, status, null, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, customer => Assert.True(customer.Status));
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters customers by status (false).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByStatusFalse_ReturnsInactiveCustomers()
        {
            // Arrange
            var status = "false";
            var expectedCustomers = GetSampleCustomers()
                .Where(x => x.Status == false).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, status, null, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.All(result, customer => Assert.False(customer.Status));
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameAscending_ReturnsSortedCustomers()
        {
            // Arrange
            var sortBy = "Name";
            var expectedCustomers = GetSampleCustomers()
                .OrderBy(x => x.Name).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedCustomers, result);
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameDescending_ReturnsSortedCustomers()
        {
            // Arrange
            var sortBy = "Name";
            var expectedCustomers = GetSampleCustomers()
                .OrderByDescending(x => x.Name).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, false))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedCustomers, result);
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with case-insensitive sorting by name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameCaseInsensitive_ReturnsSortedCustomers()
        {
            // Arrange
            var sortBy = "name"; // lowercase to test case insensitivity
            var expectedCustomers = GetSampleCustomers()
                .OrderBy(x => x.Name).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedCustomers, result);
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredCustomers()
        {
            // Arrange
            var name = "John";
            var status = "true";
            var sortBy = "Name";
            var expectedCustomers = GetSampleCustomers()
                .Where(x => x.Name.Contains(name) && x.Status == true)
                .OrderBy(x => x.Name).ToList();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(name, status, sortBy, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(name, status, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, customer =>
            {
                Assert.Contains(name, customer.Name);
                Assert.True(customer.Status);
            });
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(name, status, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no customers match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingCustomers_ReturnsEmptyList()
        {
            // Arrange
            var name = "NonExistentCustomer";
            var expectedCustomers = new List<Customer>();

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid status string (non-boolean).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidStatusString_ReturnsAllCustomers()
        {
            // Arrange
            var invalidStatus = "invalid";
            var expectedCustomers = GetSampleCustomers(); // Should return all customers when status parsing fails

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync(null, invalidStatus, null, true))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepositoryMock.Object.GetAllAsync(null, invalidStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            _customerRepositoryMock.Verify(repo => repo.GetAllAsync(null, invalidStatus, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns customer when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCustomer()
        {
            // Arrange
            var customerId = 1;
            var expectedCustomer = GetSampleCustomers().First(x => x.Id == customerId);

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.GetByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerId, result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john.doe@example.com", result.Email);
            Assert.True(result.Status);
            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new customer.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidCustomer_ReturnsCreatedCustomer()
        {
            // Arrange
            var newCustomer = new Customer
            {
                Name = "New Customer",
                Email = "new.customer@example.com",
                Status = true
            };

            var createdCustomer = new Customer
            {
                Id = 5,
                Name = "New Customer",
                Email = "new.customer@example.com",
                Status = true
            };

            _customerRepositoryMock.Setup(repo => repo.CreateAsync(newCustomer))
                .ReturnsAsync(createdCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.CreateAsync(newCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("New Customer", result.Name);
            Assert.Equal("new.customer@example.com", result.Email);
            Assert.True(result.Status);
            _customerRepositoryMock.Verify(repo => repo.CreateAsync(newCustomer), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with inactive customer.
        /// </summary>
        [Fact]
        public async Task CreateAsync_InactiveCustomer_ReturnsCreatedCustomer()
        {
            // Arrange
            var newCustomer = new Customer
            {
                Name = "Inactive Customer",
                Email = "inactive.customer@example.com",
                Status = false
            };

            var createdCustomer = new Customer
            {
                Id = 6,
                Name = "Inactive Customer",
                Email = "inactive.customer@example.com",
                Status = false
            };

            _customerRepositoryMock.Setup(repo => repo.CreateAsync(newCustomer))
                .ReturnsAsync(createdCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.CreateAsync(newCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("Inactive Customer", result.Name);
            Assert.Equal("inactive.customer@example.com", result.Email);
            Assert.False(result.Status);
            _customerRepositoryMock.Verify(repo => repo.CreateAsync(newCustomer), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with customer having special characters in name.
        /// </summary>
        [Fact]
        public async Task CreateAsync_CustomerWithSpecialCharacters_ReturnsCreatedCustomer()
        {
            // Arrange
            var newCustomer = new Customer
            {
                Name = "José María O'Connor & Co.",
                Email = "jose.maria@example.com",
                Status = true
            };

            var createdCustomer = new Customer
            {
                Id = 7,
                Name = "José María O'Connor & Co.",
                Email = "jose.maria@example.com",
                Status = true
            };

            _customerRepositoryMock.Setup(repo => repo.CreateAsync(newCustomer))
                .ReturnsAsync(createdCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.CreateAsync(newCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("José María O'Connor & Co.", result.Name);
            Assert.Equal("jose.maria@example.com", result.Email);
            _customerRepositoryMock.Verify(repo => repo.CreateAsync(newCustomer), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing customer.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndCustomer_ReturnsUpdatedCustomer()
        {
            // Arrange
            var customerId = 1;
            var updateCustomer = new Customer
            {
                Name = "Updated John Doe",
                Email = "updated.john.doe@example.com",
                Status = false
            };

            var updatedCustomer = new Customer
            {
                Id = 1,
                Name = "Updated John Doe",
                Email = "updated.john.doe@example.com",
                Status = false
            };

            _customerRepositoryMock.Setup(repo => repo.UpdateAsync(customerId, updateCustomer))
                .ReturnsAsync(updatedCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.UpdateAsync(customerId, updateCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated John Doe", result.Name);
            Assert.Equal("updated.john.doe@example.com", result.Email);
            Assert.False(result.Status);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(customerId, updateCustomer), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when customer with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateCustomer = new Customer
            {
                Name = "Updated Customer",
                Email = "updated@example.com",
                Status = true
            };

            _customerRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateCustomer))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.UpdateAsync(invalidId, updateCustomer);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateCustomer), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only name changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedCustomer()
        {
            // Arrange
            var customerId = 1;
            var updateCustomer = new Customer
            {
                Name = "John D. Smith", // Changed
                Email = "john.doe@example.com", // Same as original
                Status = true // Same as original
            };

            var updatedCustomer = new Customer
            {
                Id = 1,
                Name = "John D. Smith",
                Email = "john.doe@example.com",
                Status = true
            };

            _customerRepositoryMock.Setup(repo => repo.UpdateAsync(customerId, updateCustomer))
                .ReturnsAsync(updatedCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.UpdateAsync(customerId, updateCustomer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John D. Smith", result.Name);
            Assert.Equal("john.doe@example.com", result.Email);
            Assert.True(result.Status);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(customerId, updateCustomer), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateCustomer = new Customer
            {
                Name = "Updated Customer",
                Email = "updated@example.com",
                Status = true
            };

            _customerRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateCustomer))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.UpdateAsync(zeroId, updateCustomer);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateCustomer), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing customer.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedCustomer()
        {
            // Arrange
            var customerId = 1;
            var deletedCustomer = GetSampleCustomers().First(x => x.Id == customerId);

            _customerRepositoryMock.Setup(repo => repo.DeleteAsync(customerId))
                .ReturnsAsync(deletedCustomer);

            // Act
            var result = await _customerRepositoryMock.Object.DeleteAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john.doe@example.com", result.Email);
            _customerRepositoryMock.Verify(repo => repo.DeleteAsync(customerId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when customer with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _customerRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _customerRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _customerRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _customerRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to generate sample customers for testing.
        /// </summary>
        private static List<Customer> GetSampleCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Status = true
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Status = true
                },
                new Customer
                {
                    Id = 3,
                    Name = "John Johnson",
                    Email = "john.johnson@example.com",
                    Status = true
                },
                new Customer
                {
                    Id = 4,
                    Name = "Alice Brown",
                    Email = "alice.brown@example.com",
                    Status = false
                }
            };
        }

        #endregion
    }
}
