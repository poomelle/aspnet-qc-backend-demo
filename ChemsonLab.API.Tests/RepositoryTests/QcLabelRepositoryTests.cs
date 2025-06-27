using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.QcLabelRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class QcLabelRepositoryTests
    {
        private readonly Mock<IQcLabelRepository> _mockQcLabelRepository;

        public QcLabelRepositoryTests()
        {
            _mockQcLabelRepository = new Mock<IQcLabelRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample QcLabel data for testing purposes.
        /// </summary>
        /// <returns>List of sample QcLabel objects</returns>
        private List<QcLabel> GetSampleQcLabels()
        {
            return new List<QcLabel>
            {
                new QcLabel
                {
                    Id = 1,
                    BatchName = "BATCH001",
                    Printed = true,
                    ProductId = 1,
                    Year = "2024",
                    Month = "January",
                    Product = new Product { Id = 1, Name = "Product Alpha" }
                },
                new QcLabel
                {
                    Id = 2,
                    BatchName = "BATCH002",
                    Printed = false,
                    ProductId = 2,
                    Year = "2024",
                    Month = "January",
                    Product = new Product { Id = 2, Name = "Product Beta" }
                },
                new QcLabel
                {
                    Id = 3,
                    BatchName = "BATCH003",
                    Printed = true,
                    ProductId = 1,
                    Year = "2024",
                    Month = "February",
                    Product = new Product { Id = 1, Name = "Product Alpha" }
                },
                new QcLabel
                {
                    Id = 4,
                    BatchName = "BATCH004",
                    Printed = false,
                    ProductId = 3,
                    Year = "2023",
                    Month = "December",
                    Product = new Product { Id = 3, Name = "Product Gamma" }
                },
                new QcLabel
                {
                    Id = 5,
                    BatchName = null,
                    Printed = null,
                    ProductId = null,
                    Year = null,
                    Month = null,
                    Product = null
                },
                new QcLabel
                {
                    Id = 6,
                    BatchName = "BATCH005",
                    Printed = true,
                    ProductId = 2,
                    Year = "2024",
                    Month = "March",
                    Product = new Product { Id = 2, Name = "Product Beta" }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all QcLabel items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllQcLabels()
        {
            // Arrange
            var expectedLabels = GetSampleQcLabels();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
            Assert.Equal(expectedLabels, result);
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcLabel items by batch name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchName_ReturnsFilteredLabels()
        {
            // Arrange
            var batchName = "BATCH001";
            var expectedLabels = GetSampleQcLabels().Where(x => x.BatchName == batchName).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(batchName, null, null, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(batchName, result.First().BatchName);
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(batchName, null, null, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcLabel items by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredLabels()
        {
            // Arrange
            var productName = "Product Alpha";
            var expectedLabels = GetSampleQcLabels().Where(x => x.Product?.Name == productName).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, productName, null, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, label => Assert.Equal(productName, label.Product?.Name));
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, productName, null, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcLabel items by printed status (true).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByPrintedTrue_ReturnsPrintedLabels()
        {
            // Arrange
            var printed = "true";
            var expectedLabels = GetSampleQcLabels().Where(x => x.Printed == true).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, null, printed, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, null, printed);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, label => Assert.True(label.Printed));
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, null, printed, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcLabel items by printed status (false).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByPrintedFalse_ReturnsUnprintedLabels()
        {
            // Arrange
            var printed = "false";
            var expectedLabels = GetSampleQcLabels().Where(x => x.Printed == false).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, null, printed, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, null, printed);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, label => Assert.False(label.Printed));
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, null, printed, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcLabel items by year.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByYear_ReturnsFilteredLabels()
        {
            // Arrange
            var year = "2024";
            var expectedLabels = GetSampleQcLabels().Where(x => x.Year == year).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, null, null, year, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, null, null, year);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.All(result, label => Assert.Equal(year, label.Year));
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, null, null, year, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcLabel items by month.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMonth_ReturnsFilteredLabels()
        {
            // Arrange
            var month = "January";
            var expectedLabels = GetSampleQcLabels().Where(x => x.Month == month).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, month))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, null, null, null, month);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, label => Assert.Equal(month, label.Month));
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, month), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredLabels()
        {
            // Arrange
            var productName = "Product Beta";
            var printed = "true";
            var year = "2024";
            var month = "March";
            var expectedLabels = GetSampleQcLabels()
                .Where(x => x.Product?.Name == productName && x.Printed == true && x.Year == year && x.Month == month)
                .ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, productName, printed, year, month))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, productName, printed, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(productName, result.First().Product?.Name);
            Assert.True(result.First().Printed);
            Assert.Equal(year, result.First().Year);
            Assert.Equal(month, result.First().Month);
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, productName, printed, year, month), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no QcLabel items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingLabels_ReturnsEmptyList()
        {
            // Arrange
            var batchName = "NONEXISTENT_BATCH";
            var expectedLabels = new List<QcLabel>();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(batchName, null, null, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(batchName, null, null, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid printed filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidPrintedFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidPrinted = "invalid";
            var expectedLabels = new List<QcLabel>();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(null, null, invalidPrinted, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(null, null, invalidPrinted);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(null, null, invalidPrinted, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters by specific batch name pattern.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterBySpecificBatchPattern_ReturnsFilteredLabels()
        {
            // Arrange
            var batchName = "BATCH003";
            var expectedLabels = GetSampleQcLabels().Where(x => x.BatchName == batchName).ToList();

            _mockQcLabelRepository.Setup(repo => repo.GetAllAsync(batchName, null, null, null, null))
                .ReturnsAsync(expectedLabels);

            // Act
            var result = await _mockQcLabelRepository.Object.GetAllAsync(batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(batchName, result.First().BatchName);
            Assert.Equal("Product Alpha", result.First().Product?.Name);
            Assert.True(result.First().Printed);
            _mockQcLabelRepository.Verify(repo => repo.GetAllAsync(batchName, null, null, null, null), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns QcLabel when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsQcLabel()
        {
            // Arrange
            var labelId = 1;
            var expectedLabel = GetSampleQcLabels().First(x => x.Id == labelId);

            _mockQcLabelRepository.Setup(repo => repo.GetByIdAsync(labelId))
                .ReturnsAsync(expectedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.GetByIdAsync(labelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(labelId, result.Id);
            Assert.Equal("BATCH001", result.BatchName);
            Assert.True(result.Printed);
            Assert.Equal(1, result.ProductId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("January", result.Month);
            _mockQcLabelRepository.Verify(repo => repo.GetByIdAsync(labelId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns QcLabel with null optional properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_LabelWithNullProperties_ReturnsLabel()
        {
            // Arrange
            var labelId = 5;
            var expectedLabel = GetSampleQcLabels().First(x => x.Id == labelId);

            _mockQcLabelRepository.Setup(repo => repo.GetByIdAsync(labelId))
                .ReturnsAsync(expectedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.GetByIdAsync(labelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(labelId, result.Id);
            Assert.Null(result.BatchName);
            Assert.Null(result.Printed);
            Assert.Null(result.ProductId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            Assert.Null(result.Product);
            _mockQcLabelRepository.Verify(repo => repo.GetByIdAsync(labelId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockQcLabelRepository.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _mockQcLabelRepository.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _mockQcLabelRepository.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new QcLabel with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidQcLabelWithAllProperties_ReturnsCreatedLabel()
        {
            // Arrange
            var newLabel = new QcLabel
            {
                BatchName = "BATCH006",
                Printed = false,
                ProductId = 4,
                Year = "2024",
                Month = "April",
                Product = new Product { Id = 4, Name = "Product Delta" }
            };

            var createdLabel = new QcLabel
            {
                Id = 7,
                BatchName = "BATCH006",
                Printed = false,
                ProductId = 4,
                Year = "2024",
                Month = "April",
                Product = new Product { Id = 4, Name = "Product Delta" }
            };

            _mockQcLabelRepository.Setup(repo => repo.CreateAsync(newLabel))
                .ReturnsAsync(createdLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.CreateAsync(newLabel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("BATCH006", result.BatchName);
            Assert.False(result.Printed);
            Assert.Equal(4, result.ProductId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("April", result.Month);
            _mockQcLabelRepository.Verify(repo => repo.CreateAsync(newLabel), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalQcLabel_ReturnsCreatedLabel()
        {
            // Arrange
            var newLabel = new QcLabel
            {
                BatchName = "MINIMAL_BATCH",
                ProductId = 1,
                Product = new Product { Id = 1, Name = "Product Alpha" }
            };

            var createdLabel = new QcLabel
            {
                Id = 8,
                BatchName = "MINIMAL_BATCH",
                ProductId = 1,
                Product = new Product { Id = 1, Name = "Product Alpha" }
            };

            _mockQcLabelRepository.Setup(repo => repo.CreateAsync(newLabel))
                .ReturnsAsync(createdLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.CreateAsync(newLabel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal("MINIMAL_BATCH", result.BatchName);
            Assert.Equal(1, result.ProductId);
            Assert.Null(result.Printed);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            _mockQcLabelRepository.Verify(repo => repo.CreateAsync(newLabel), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_QcLabelWithNullOptionalProperties_ReturnsCreatedLabel()
        {
            // Arrange
            var newLabel = new QcLabel
            {
                BatchName = null,
                Printed = null,
                ProductId = null,
                Year = null,
                Month = null,
                Product = null
            };

            var createdLabel = new QcLabel
            {
                Id = 9,
                BatchName = null,
                Printed = null,
                ProductId = null,
                Year = null,
                Month = null,
                Product = null
            };

            _mockQcLabelRepository.Setup(repo => repo.CreateAsync(newLabel))
                .ReturnsAsync(createdLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.CreateAsync(newLabel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Null(result.BatchName);
            Assert.Null(result.Printed);
            Assert.Null(result.ProductId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            _mockQcLabelRepository.Verify(repo => repo.CreateAsync(newLabel), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with printed status true.
        /// </summary>
        [Fact]
        public async Task CreateAsync_QcLabelWithPrintedTrue_ReturnsCreatedLabel()
        {
            // Arrange
            var newLabel = new QcLabel
            {
                BatchName = "PRINTED_BATCH",
                Printed = true,
                ProductId = 2,
                Year = "2024",
                Month = "May",
                Product = new Product { Id = 2, Name = "Product Beta" }
            };

            var createdLabel = new QcLabel
            {
                Id = 10,
                BatchName = "PRINTED_BATCH",
                Printed = true,
                ProductId = 2,
                Year = "2024",
                Month = "May",
                Product = new Product { Id = 2, Name = "Product Beta" }
            };

            _mockQcLabelRepository.Setup(repo => repo.CreateAsync(newLabel))
                .ReturnsAsync(createdLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.CreateAsync(newLabel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("PRINTED_BATCH", result.BatchName);
            Assert.True(result.Printed);
            Assert.Equal(2, result.ProductId);
            _mockQcLabelRepository.Verify(repo => repo.CreateAsync(newLabel), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing QcLabel.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndLabel_ReturnsUpdatedLabel()
        {
            // Arrange
            var labelId = 1;
            var updateLabel = new QcLabel
            {
                BatchName = "UPDATED_BATCH001",
                Printed = false,
                ProductId = 3,
                Year = "2025",
                Month = "June",
                Product = new Product { Id = 3, Name = "Updated Product" }
            };

            var updatedLabel = new QcLabel
            {
                Id = 1,
                BatchName = "UPDATED_BATCH001",
                Printed = false,
                ProductId = 3,
                Year = "2025",
                Month = "June",
                Product = new Product { Id = 3, Name = "Updated Product" }
            };

            _mockQcLabelRepository.Setup(repo => repo.UpdateAsync(labelId, updateLabel))
                .ReturnsAsync(updatedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.UpdateAsync(labelId, updateLabel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("UPDATED_BATCH001", result.BatchName);
            Assert.False(result.Printed);
            Assert.Equal(3, result.ProductId);
            _mockQcLabelRepository.Verify(repo => repo.UpdateAsync(labelId, updateLabel), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when QcLabel with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateLabel = new QcLabel
            {
                BatchName = "UPDATE_BATCH",
                Printed = true,
                ProductId = 1
            };

            _mockQcLabelRepository.Setup(repo => repo.UpdateAsync(invalidId, updateLabel))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.UpdateAsync(invalidId, updateLabel);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.UpdateAsync(invalidId, updateLabel), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedLabel()
        {
            // Arrange
            var labelId = 2;
            var updateLabel = new QcLabel
            {
                BatchName = "BATCH002", // Same as original
                Printed = true, // Changed from false to true
                ProductId = 2, // Same as original
                Year = "2024", // Same as original (assuming Year/Month not updated based on repository)
                Month = "January", // Same as original
                Product = new Product { Id = 2, Name = "Product Beta" }
            };

            var updatedLabel = new QcLabel
            {
                Id = 2,
                BatchName = "BATCH002",
                Printed = true,
                ProductId = 2,
                Year = "2024",
                Month = "January",
                Product = new Product { Id = 2, Name = "Product Beta" }
            };

            _mockQcLabelRepository.Setup(repo => repo.UpdateAsync(labelId, updateLabel))
                .ReturnsAsync(updatedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.UpdateAsync(labelId, updateLabel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Printed); // Changed property
            Assert.Equal("BATCH002", result.BatchName); // Unchanged property
            Assert.Equal(2, result.ProductId); // Unchanged property
            _mockQcLabelRepository.Verify(repo => repo.UpdateAsync(labelId, updateLabel), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedLabel()
        {
            // Arrange
            var labelId = 3;
            var updateLabel = new QcLabel
            {
                BatchName = null, // Changed from "BATCH003" to null
                Printed = null, // Changed from true to null
                ProductId = null, // Changed from 1 to null
                Product = null
            };

            var updatedLabel = new QcLabel
            {
                Id = 3,
                BatchName = null,
                Printed = null,
                ProductId = null,
                Product = null
            };

            _mockQcLabelRepository.Setup(repo => repo.UpdateAsync(labelId, updateLabel))
                .ReturnsAsync(updatedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.UpdateAsync(labelId, updateLabel);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.BatchName);
            Assert.Null(result.Printed);
            Assert.Null(result.ProductId);
            _mockQcLabelRepository.Verify(repo => repo.UpdateAsync(labelId, updateLabel), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateLabel = new QcLabel
            {
                BatchName = "UPDATE_BATCH",
                Printed = true,
                ProductId = 1
            };

            _mockQcLabelRepository.Setup(repo => repo.UpdateAsync(zeroId, updateLabel))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.UpdateAsync(zeroId, updateLabel);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.UpdateAsync(zeroId, updateLabel), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing QcLabel.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedLabel()
        {
            // Arrange
            var labelId = 1;
            var deletedLabel = GetSampleQcLabels().First(x => x.Id == labelId);

            _mockQcLabelRepository.Setup(repo => repo.DeleteAsync(labelId))
                .ReturnsAsync(deletedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.DeleteAsync(labelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("BATCH001", result.BatchName);
            Assert.True(result.Printed);
            Assert.Equal(1, result.ProductId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("January", result.Month);
            _mockQcLabelRepository.Verify(repo => repo.DeleteAsync(labelId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when QcLabel with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockQcLabelRepository.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _mockQcLabelRepository.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _mockQcLabelRepository.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((QcLabel?)null);

            // Act
            var result = await _mockQcLabelRepository.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockQcLabelRepository.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with QcLabel containing null optional properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_LabelWithNullProperties_ReturnsDeletedLabel()
        {
            // Arrange
            var labelId = 5;
            var deletedLabel = GetSampleQcLabels().First(x => x.Id == labelId);

            _mockQcLabelRepository.Setup(repo => repo.DeleteAsync(labelId))
                .ReturnsAsync(deletedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.DeleteAsync(labelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Null(result.BatchName);
            Assert.Null(result.Printed);
            Assert.Null(result.ProductId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            _mockQcLabelRepository.Verify(repo => repo.DeleteAsync(labelId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with printed QcLabel.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_PrintedLabel_ReturnsDeletedLabel()
        {
            // Arrange
            var labelId = 6;
            var deletedLabel = GetSampleQcLabels().First(x => x.Id == labelId);

            _mockQcLabelRepository.Setup(repo => repo.DeleteAsync(labelId))
                .ReturnsAsync(deletedLabel);

            // Act
            var result = await _mockQcLabelRepository.Object.DeleteAsync(labelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("BATCH005", result.BatchName);
            Assert.True(result.Printed);
            Assert.Equal(2, result.ProductId);
            Assert.Equal("Product Beta", result.Product?.Name);
            _mockQcLabelRepository.Verify(repo => repo.DeleteAsync(labelId), Times.Once);
        }

        #endregion
    }
}


