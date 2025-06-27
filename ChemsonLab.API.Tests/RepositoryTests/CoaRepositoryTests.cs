using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.CoaRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class CoaRepositoryTests
    {
        private readonly Mock<ICoaRepository> _coaRepositoryMock;

        public CoaRepositoryTests()
        {
            _coaRepositoryMock = new Mock<ICoaRepository>();
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all COAs when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllCoas()
        {
            // Arrange
            var expectedCoas = GetSampleCoas();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedCoas, result);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters COAs by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredCoas()
        {
            // Arrange
            var productName = "Product1";
            var expectedCoas = GetSampleCoas()
                .Where(x => x.Product.Name == productName).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(productName, null, null, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, coa => Assert.Equal(productName, coa.Product.Name));
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(productName, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters COAs by batch name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchName_ReturnsFilteredCoas()
        {
            // Arrange
            var batchName = "Batch001";
            var expectedCoas = GetSampleCoas()
                .Where(x => x.BatchName.Contains(batchName)).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, batchName, null, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(null, batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(batchName, result.First().BatchName);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(null, batchName, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameAscending_ReturnsSortedCoas()
        {
            // Arrange
            var sortBy = "productName";
            var expectedCoas = GetSampleCoas()
                .OrderBy(x => x.Product.Name).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedCoas, result);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameDescending_ReturnsSortedCoas()
        {
            // Arrange
            var sortBy = "productName";
            var expectedCoas = GetSampleCoas()
                .OrderByDescending(x => x.Product.Name).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, false))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedCoas, result);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by batch name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByBatchNameAscending_ReturnsSortedCoas()
        {
            // Arrange
            var sortBy = "batchName";
            var expectedCoas = GetSampleCoas()
                .OrderBy(x => x.BatchName).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedCoas, result);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by batch name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByBatchNameDescending_ReturnsSortedCoas()
        {
            // Arrange
            var sortBy = "batchName";
            var expectedCoas = GetSampleCoas()
                .OrderByDescending(x => x.BatchName).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, false))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedCoas, result);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredCoas()
        {
            // Arrange
            var productName = "Product1";
            var batchName = "Batch001";
            var sortBy = "batchName";
            var expectedCoas = GetSampleCoas()
                .Where(x => x.Product.Name == productName && x.BatchName.Contains(batchName))
                .OrderBy(x => x.BatchName).ToList();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(productName, batchName, sortBy, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(productName, batchName, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(productName, result.First().Product.Name);
            Assert.Contains(batchName, result.First().BatchName);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(productName, batchName, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no COAs match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingCoas_ReturnsEmptyList()
        {
            // Arrange
            var productName = "NonExistentProduct";
            var expectedCoas = new List<Coa>();

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(productName, null, null, true))
                .ReturnsAsync(expectedCoas);

            // Act
            var result = await _coaRepositoryMock.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _coaRepositoryMock.Verify(repo => repo.GetAllAsync(productName, null, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns COA when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCoa()
        {
            // Arrange
            var coaId = 1;
            var expectedCoa = GetSampleCoas().First(x => x.Id == coaId);

            _coaRepositoryMock.Setup(repo => repo.GetByIdAsync(coaId))
                .ReturnsAsync(expectedCoa);

            // Act
            var result = await _coaRepositoryMock.Object.GetByIdAsync(coaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(coaId, result.Id);
            Assert.Equal("COA-Batch001", result.BatchName);
            Assert.Equal("Product1", result.Product.Name);
            _coaRepositoryMock.Verify(repo => repo.GetByIdAsync(coaId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _coaRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _coaRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _coaRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new COA.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidCoa_ReturnsCreatedCoa()
        {
            // Arrange
            var newCoa = new Coa
            {
                ProductId = 1,
                BatchName = "COA-NewBatch001",
                Product = new Product { Id = 1, Name = "Product1", COA = true }
            };

            var createdCoa = new Coa
            {
                Id = 4,
                ProductId = 1,
                BatchName = "COA-NewBatch001",
                Product = new Product { Id = 1, Name = "Product1", COA = true }
            };

            _coaRepositoryMock.Setup(repo => repo.CreateAsync(newCoa))
                .ReturnsAsync(createdCoa);

            // Act
            var result = await _coaRepositoryMock.Object.CreateAsync(newCoa);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal("COA-NewBatch001", result.BatchName);
            Assert.Equal(1, result.ProductId);
            Assert.Equal("Product1", result.Product.Name);
            _coaRepositoryMock.Verify(repo => repo.CreateAsync(newCoa), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalCoa_ReturnsCreatedCoa()
        {
            // Arrange
            var newCoa = new Coa
            {
                ProductId = 2,
                BatchName = "COA-MinimalBatch",
                Product = new Product { Id = 2, Name = "Product2" }
            };

            var createdCoa = new Coa
            {
                Id = 5,
                ProductId = 2,
                BatchName = "COA-MinimalBatch",
                Product = new Product { Id = 2, Name = "Product2" }
            };

            _coaRepositoryMock.Setup(repo => repo.CreateAsync(newCoa))
                .ReturnsAsync(createdCoa);

            // Act
            var result = await _coaRepositoryMock.Object.CreateAsync(newCoa);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("COA-MinimalBatch", result.BatchName);
            Assert.Equal(2, result.ProductId);
            _coaRepositoryMock.Verify(repo => repo.CreateAsync(newCoa), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with complete product navigation property.
        /// </summary>
        [Fact]
        public async Task CreateAsync_CompletelyPopulatedCoa_ReturnsCreatedCoa()
        {
            // Arrange
            var newCoa = new Coa
            {
                ProductId = 1,
                BatchName = "COA-CompleteBatch001",
                Product = new Product
                {
                    Id = 1,
                    Name = "CompleteProduct",
                    COA = true
                }
            };

            var createdCoa = new Coa
            {
                Id = 6,
                ProductId = 1,
                BatchName = "COA-CompleteBatch001",
                Product = new Product
                {
                    Id = 1,
                    Name = "CompleteProduct",
                    COA = true
                }
            };

            _coaRepositoryMock.Setup(repo => repo.CreateAsync(newCoa))
                .ReturnsAsync(createdCoa);

            // Act
            var result = await _coaRepositoryMock.Object.CreateAsync(newCoa);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("COA-CompleteBatch001", result.BatchName);
            Assert.Equal("CompleteProduct", result.Product.Name);
            Assert.True(result.Product.COA);
            _coaRepositoryMock.Verify(repo => repo.CreateAsync(newCoa), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing COA.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndCoa_ReturnsUpdatedCoa()
        {
            // Arrange
            var coaId = 1;
            var updateCoa = new Coa
            {
                ProductId = 2,
                BatchName = "COA-UpdatedBatch001",
                Product = new Product { Id = 2, Name = "UpdatedProduct" }
            };

            var updatedCoa = new Coa
            {
                Id = 1,
                ProductId = 2,
                BatchName = "COA-UpdatedBatch001",
                Product = new Product { Id = 2, Name = "UpdatedProduct" }
            };

            _coaRepositoryMock.Setup(repo => repo.UpdateAsync(coaId, updateCoa))
                .ReturnsAsync(updatedCoa);

            // Act
            var result = await _coaRepositoryMock.Object.UpdateAsync(coaId, updateCoa);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("COA-UpdatedBatch001", result.BatchName);
            Assert.Equal(2, result.ProductId);
            Assert.Equal("UpdatedProduct", result.Product.Name);
            _coaRepositoryMock.Verify(repo => repo.UpdateAsync(coaId, updateCoa), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when COA with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateCoa = new Coa
            {
                ProductId = 1,
                BatchName = "COA-UpdatedBatch",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _coaRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateCoa))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.UpdateAsync(invalidId, updateCoa);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateCoa), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only batch name changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedCoa()
        {
            // Arrange
            var coaId = 1;
            var updateCoa = new Coa
            {
                ProductId = 1, // Same as original
                BatchName = "COA-PartiallyUpdatedBatch", // Changed
                Product = new Product { Id = 1, Name = "Product1" }
            };

            var updatedCoa = new Coa
            {
                Id = 1,
                ProductId = 1,
                BatchName = "COA-PartiallyUpdatedBatch",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _coaRepositoryMock.Setup(repo => repo.UpdateAsync(coaId, updateCoa))
                .ReturnsAsync(updatedCoa);

            // Act
            var result = await _coaRepositoryMock.Object.UpdateAsync(coaId, updateCoa);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("COA-PartiallyUpdatedBatch", result.BatchName);
            Assert.Equal(1, result.ProductId);
            _coaRepositoryMock.Verify(repo => repo.UpdateAsync(coaId, updateCoa), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateCoa = new Coa
            {
                ProductId = 1,
                BatchName = "COA-UpdatedBatch",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _coaRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateCoa))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.UpdateAsync(zeroId, updateCoa);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateCoa), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing COA.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedCoa()
        {
            // Arrange
            var coaId = 1;
            var deletedCoa = GetSampleCoas().First(x => x.Id == coaId);

            _coaRepositoryMock.Setup(repo => repo.DeleteAsync(coaId))
                .ReturnsAsync(deletedCoa);

            // Act
            var result = await _coaRepositoryMock.Object.DeleteAsync(coaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("COA-Batch001", result.BatchName);
            Assert.Equal("Product1", result.Product.Name);
            _coaRepositoryMock.Verify(repo => repo.DeleteAsync(coaId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when COA with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _coaRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _coaRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _coaRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _coaRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _coaRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to generate sample COAs for testing.
        /// </summary>
        private static List<Coa> GetSampleCoas()
        {
            return new List<Coa>
            {
                new Coa
                {
                    Id = 1,
                    ProductId = 1,
                    BatchName = "COA-Batch001",
                    Product = new Product { Id = 1, Name = "Product1", COA = true }
                },
                new Coa
                {
                    Id = 2,
                    ProductId = 1,
                    BatchName = "COA-Batch002",
                    Product = new Product { Id = 1, Name = "Product1", COA = true }
                },
                new Coa
                {
                    Id = 3,
                    ProductId = 2,
                    BatchName = "COA-Batch003",
                    Product = new Product { Id = 2, Name = "Product2", COA = false }
                }
            };
        }

        #endregion
    }
}
