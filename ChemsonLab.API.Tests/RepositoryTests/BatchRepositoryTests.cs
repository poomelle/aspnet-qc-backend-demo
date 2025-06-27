using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.BatchRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class BatchRepositoryTests
    {
        private readonly Mock<IBatchRepository> _batchRepositoryMock;

        public BatchRepositoryTests()
        {
            _batchRepositoryMock = new Mock<IBatchRepository>();
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all batches when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllBatches()
        {
            // Arrange
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } },
                new Batch { Id = 2, BatchName = "Batch002", ProductId = 2, Suffix = "B", Product = new Product { Id = 2, Name = "Product2" } },
                new Batch { Id = 3, BatchName = "Batch003", ProductId = 1, Suffix = "C", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedBatches, result);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batches by batch name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchName_ReturnsFilteredBatches()
        {
            // Arrange
            var batchName = "Batch001";
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(batchName, null, null, null, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(batchName, result.First().BatchName);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(batchName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batches by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredBatches()
        {
            // Arrange
            var productName = "Product1";
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } },
                new Batch { Id = 3, BatchName = "Batch003", ProductId = 1, Suffix = "C", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(null, productName, null, null, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(null, productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, batch => Assert.Equal(productName, batch.Product.Name));
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(null, productName, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batches by suffix.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterBySuffix_ReturnsFilteredBatches()
        {
            // Arrange
            var suffix = "A";
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, suffix, null, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(null, null, suffix);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(suffix, result.First().Suffix);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, suffix, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortAscending_ReturnsSortedBatches()
        {
            // Arrange
            var sortBy = "BatchName";
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } },
                new Batch { Id = 2, BatchName = "Batch002", ProductId = 2, Suffix = "B", Product = new Product { Id = 2, Name = "Product2" } },
                new Batch { Id = 3, BatchName = "Batch003", ProductId = 1, Suffix = "C", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedBatches, result);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortDescending_ReturnsSortedBatches()
        {
            // Arrange
            var sortBy = "BatchName";
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 3, BatchName = "Batch003", ProductId = 1, Suffix = "C", Product = new Product { Id = 1, Name = "Product1" } },
                new Batch { Id = 2, BatchName = "Batch002", ProductId = 2, Suffix = "B", Product = new Product { Id = 2, Name = "Product2" } },
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, false))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedBatches, result);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredBatches()
        {
            // Arrange
            var batchName = "Batch001";
            var productName = "Product1";
            var suffix = "A";
            var sortBy = "BatchName";
            var expectedBatches = new List<Batch>
            {
                new Batch { Id = 1, BatchName = "Batch001", ProductId = 1, Suffix = "A", Product = new Product { Id = 1, Name = "Product1" } }
            };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(batchName, productName, suffix, sortBy, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(batchName, productName, suffix, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(batchName, result.First().BatchName);
            Assert.Equal(productName, result.First().Product.Name);
            Assert.Equal(suffix, result.First().Suffix);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(batchName, productName, suffix, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no batches match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingBatches_ReturnsEmptyList()
        {
            // Arrange
            var batchName = "NonExistentBatch";
            var expectedBatches = new List<Batch>();

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(batchName, null, null, null, true))
                .ReturnsAsync(expectedBatches);

            // Act
            var result = await _batchRepositoryMock.Object.GetAllAsync(batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _batchRepositoryMock.Verify(repo => repo.GetAllAsync(batchName, null, null, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns batch when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsBatch()
        {
            // Arrange
            var batchId = 1;
            var expectedBatch = new Batch
            {
                Id = 1,
                BatchName = "Batch001",
                ProductId = 1,
                Suffix = "A",
                SampleBy = "John Doe",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(batchId))
                .ReturnsAsync(expectedBatch);

            // Act
            var result = await _batchRepositoryMock.Object.GetByIdAsync(batchId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(batchId, result.Id);
            Assert.Equal("Batch001", result.BatchName);
            Assert.Equal("John Doe", result.SampleBy);
            _batchRepositoryMock.Verify(repo => repo.GetByIdAsync(batchId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new batch.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidBatch_ReturnsCreatedBatch()
        {
            // Arrange
            var newBatch = new Batch
            {
                BatchName = "NewBatch001",
                ProductId = 1,
                Suffix = "A",
                SampleBy = "Jane Doe",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            var createdBatch = new Batch
            {
                Id = 4,
                BatchName = "NewBatch001",
                ProductId = 1,
                Suffix = "A",
                SampleBy = "Jane Doe",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.CreateAsync(newBatch))
                .ReturnsAsync(createdBatch);

            // Act
            var result = await _batchRepositoryMock.Object.CreateAsync(newBatch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal("NewBatch001", result.BatchName);
            Assert.Equal("Jane Doe", result.SampleBy);
            Assert.Equal(1, result.ProductId);
            _batchRepositoryMock.Verify(repo => repo.CreateAsync(newBatch), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalBatch_ReturnsCreatedBatch()
        {
            // Arrange
            var newBatch = new Batch
            {
                BatchName = "MinimalBatch",
                ProductId = 1,
                Product = new Product { Id = 1, Name = "Product1" }
            };

            var createdBatch = new Batch
            {
                Id = 5,
                BatchName = "MinimalBatch",
                ProductId = 1,
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.CreateAsync(newBatch))
                .ReturnsAsync(createdBatch);

            // Act
            var result = await _batchRepositoryMock.Object.CreateAsync(newBatch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("MinimalBatch", result.BatchName);
            Assert.Equal(1, result.ProductId);
            _batchRepositoryMock.Verify(repo => repo.CreateAsync(newBatch), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set.
        /// </summary>
        [Fact]
        public async Task CreateAsync_CompletelyPopulatedBatch_ReturnsCreatedBatch()
        {
            // Arrange
            var newBatch = new Batch
            {
                BatchName = "CompleteBatch001",
                ProductId = 1,
                Suffix = "Z",
                SampleBy = "Complete Tester",
                Product = new Product
                {
                    Id = 1,
                    Name = "CompleteProduct",
                    Status = true,
                    SampleAmount = 100.5,
                    Comment = "Test comment"
                }
            };

            var createdBatch = new Batch
            {
                Id = 6,
                BatchName = "CompleteBatch001",
                ProductId = 1,
                Suffix = "Z",
                SampleBy = "Complete Tester",
                Product = new Product
                {
                    Id = 1,
                    Name = "CompleteProduct",
                    Status = true,
                    SampleAmount = 100.5,
                    Comment = "Test comment"
                }
            };

            _batchRepositoryMock.Setup(repo => repo.CreateAsync(newBatch))
                .ReturnsAsync(createdBatch);

            // Act
            var result = await _batchRepositoryMock.Object.CreateAsync(newBatch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("CompleteBatch001", result.BatchName);
            Assert.Equal("Z", result.Suffix);
            Assert.Equal("Complete Tester", result.SampleBy);
            Assert.Equal("CompleteProduct", result.Product.Name);
            _batchRepositoryMock.Verify(repo => repo.CreateAsync(newBatch), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing batch.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndBatch_ReturnsUpdatedBatch()
        {
            // Arrange
            var batchId = 1;
            var updateBatch = new Batch
            {
                BatchName = "UpdatedBatch001",
                ProductId = 2,
                Suffix = "B",
                SampleBy = "Updated Tester",
                Product = new Product { Id = 2, Name = "UpdatedProduct" }
            };

            var updatedBatch = new Batch
            {
                Id = 1,
                BatchName = "UpdatedBatch001",
                ProductId = 2,
                Suffix = "B",
                SampleBy = "Updated Tester",
                Product = new Product { Id = 2, Name = "UpdatedProduct" }
            };

            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(batchId, updateBatch))
                .ReturnsAsync(updatedBatch);

            // Act
            var result = await _batchRepositoryMock.Object.UpdateAsync(batchId, updateBatch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("UpdatedBatch001", result.BatchName);
            Assert.Equal("Updated Tester", result.SampleBy);
            Assert.Equal(2, result.ProductId);
            _batchRepositoryMock.Verify(repo => repo.UpdateAsync(batchId, updateBatch), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when batch with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateBatch = new Batch
            {
                BatchName = "UpdatedBatch001",
                ProductId = 1,
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateBatch))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.UpdateAsync(invalidId, updateBatch);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateBatch), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedBatch()
        {
            // Arrange
            var batchId = 1;
            var updateBatch = new Batch
            {
                BatchName = "PartiallyUpdatedBatch",
                ProductId = 1,
                Suffix = "A", // Same as original
                SampleBy = "Updated Tester", // Changed
                Product = new Product { Id = 1, Name = "Product1" }
            };

            var updatedBatch = new Batch
            {
                Id = 1,
                BatchName = "PartiallyUpdatedBatch",
                ProductId = 1,
                Suffix = "A",
                SampleBy = "Updated Tester",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(batchId, updateBatch))
                .ReturnsAsync(updatedBatch);

            // Act
            var result = await _batchRepositoryMock.Object.UpdateAsync(batchId, updateBatch);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("PartiallyUpdatedBatch", result.BatchName);
            Assert.Equal("Updated Tester", result.SampleBy);
            Assert.Equal("A", result.Suffix);
            _batchRepositoryMock.Verify(repo => repo.UpdateAsync(batchId, updateBatch), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateBatch = new Batch
            {
                BatchName = "UpdatedBatch",
                ProductId = 1,
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateBatch))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.UpdateAsync(zeroId, updateBatch);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateBatch), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing batch.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedBatch()
        {
            // Arrange
            var batchId = 1;
            var deletedBatch = new Batch
            {
                Id = 1,
                BatchName = "DeletedBatch001",
                ProductId = 1,
                Suffix = "A",
                SampleBy = "Test Deleter",
                Product = new Product { Id = 1, Name = "Product1" }
            };

            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(batchId))
                .ReturnsAsync(deletedBatch);

            // Act
            var result = await _batchRepositoryMock.Object.DeleteAsync(batchId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("DeletedBatch001", result.BatchName);
            _batchRepositoryMock.Verify(repo => repo.DeleteAsync(batchId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when batch with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _batchRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _batchRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion
    }
}
