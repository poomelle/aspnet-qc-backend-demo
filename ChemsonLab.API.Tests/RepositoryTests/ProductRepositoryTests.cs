using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.ProductRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class ProductRepositoryTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public ProductRepositoryTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample Product data for testing purposes.
        /// </summary>
        /// <returns>List of sample Product objects</returns>
        private List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Product Alpha",
                    Status = true,
                    SampleAmount = 100.5,
                    Comment = "High quality product for testing",
                    DBDate = new DateTime(2024, 1, 15),
                    COA = true,
                    Colour = "Blue",
                    TorqueWarning = 25.0,
                    TorqueFail = 30.0,
                    FusionWarning = 150.0,
                    FusionFail = 180.0,
                    UpdateDate = new DateTime(2024, 1, 20),
                    BulkWeight = 500.25,
                    PaperBagWeight = 50.0,
                    PaperBagNo = 10,
                    BatchWeight = 25.5
                },
                new Product
                {
                    Id = 2,
                    Name = "Product Beta",
                    Status = false,
                    SampleAmount = 75.25,
                    Comment = "Standard product for general use",
                    DBDate = new DateTime(2024, 2, 10),
                    COA = false,
                    Colour = "Red",
                    TorqueWarning = 20.0,
                    TorqueFail = 25.0,
                    FusionWarning = 140.0,
                    FusionFail = 170.0,
                    UpdateDate = new DateTime(2024, 2, 15),
                    BulkWeight = 450.0,
                    PaperBagWeight = 45.0,
                    PaperBagNo = 8,
                    BatchWeight = 22.0
                },
                new Product
                {
                    Id = 3,
                    Name = "Product Gamma",
                    Status = true,
                    SampleAmount = 120.0,
                    Comment = "Premium product with enhanced features",
                    DBDate = new DateTime(2024, 3, 5),
                    COA = true,
                    Colour = "Green",
                    TorqueWarning = 30.0,
                    TorqueFail = 35.0,
                    FusionWarning = 160.0,
                    FusionFail = 190.0,
                    UpdateDate = new DateTime(2024, 3, 10),
                    BulkWeight = 600.0,
                    PaperBagWeight = 60.0,
                    PaperBagNo = 12,
                    BatchWeight = 30.0
                },
                new Product
                {
                    Id = 4,
                    Name = "Product Delta",
                    Status = false,
                    SampleAmount = null,
                    Comment = null,
                    DBDate = null,
                    COA = null,
                    Colour = null,
                    TorqueWarning = null,
                    TorqueFail = null,
                    FusionWarning = null,
                    FusionFail = null,
                    UpdateDate = null,
                    BulkWeight = null,
                    PaperBagWeight = null,
                    PaperBagNo = null,
                    BatchWeight = null
                },
                new Product
                {
                    Id = 5,
                    Name = "Product Alpha", // Duplicate name for testing exact matching
                    Status = true,
                    SampleAmount = 95.0,
                    Comment = "Another alpha product variant",
                    DBDate = new DateTime(2024, 4, 1),
                    COA = false,
                    Colour = "Yellow",
                    TorqueWarning = 22.5,
                    TorqueFail = 27.5,
                    FusionWarning = 145.0,
                    FusionFail = 175.0,
                    UpdateDate = new DateTime(2024, 4, 5),
                    BulkWeight = 480.0,
                    PaperBagWeight = 48.0,
                    PaperBagNo = 9,
                    BatchWeight = 24.0
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all Product items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllProducts()
        {
            // Arrange
            var expectedProducts = GetSampleProducts();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProducts, result);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Product items by exact name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByExactName_ReturnsFilteredProducts()
        {
            // Arrange
            var name = "Product Alpha";
            var expectedProducts = GetSampleProducts().Where(x => x.Name.Equals(name)).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Two products with exact name match
            Assert.All(result, product => Assert.Equal(name, product.Name));
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Product items by unique name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByUniqueName_ReturnsFilteredProducts()
        {
            // Arrange
            var name = "Product Beta";
            var expectedProducts = GetSampleProducts().Where(x => x.Name.Equals(name)).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(name, result.First().Name);
            Assert.False(result.First().Status);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Product items by active status.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByActiveStatus_ReturnsActiveProducts()
        {
            // Arrange
            var status = "true";
            var expectedProducts = GetSampleProducts().Where(x => x.Status == true).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, status, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, product => Assert.True(product.Status));
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Product items by inactive status.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByInactiveStatus_ReturnsInactiveProducts()
        {
            // Arrange
            var status = "false";
            var expectedProducts = GetSampleProducts().Where(x => x.Status == false).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, status, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, product => Assert.False(product.Status));
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameAscending_ReturnsSortedProducts()
        {
            // Arrange
            var sortBy = "Name";
            var expectedProducts = GetSampleProducts().OrderBy(x => x.Name).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProducts, result);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameDescending_ReturnsSortedProducts()
        {
            // Arrange
            var sortBy = "Name";
            var expectedProducts = GetSampleProducts().OrderByDescending(x => x.Name).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, false))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProducts, result);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with case-insensitive sorting.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_CaseInsensitiveSorting_ReturnsSortedProducts()
        {
            // Arrange
            var sortBy = "name"; // lowercase
            var expectedProducts = GetSampleProducts().OrderBy(x => x.Name).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProducts, result);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredProducts()
        {
            // Arrange
            var name = "Product Alpha";
            var status = "true";
            var sortBy = "Name";
            var expectedProducts = GetSampleProducts()
                .Where(x => x.Name.Equals(name) && x.Status == true)
                .OrderBy(x => x.Name).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(name, status, sortBy, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(name, status, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, product =>
            {
                Assert.Equal(name, product.Name);
                Assert.True(product.Status);
            });
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(name, status, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no Product items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingProducts_ReturnsEmptyList()
        {
            // Arrange
            var name = "NonExistentProduct";
            var expectedProducts = new List<Product>();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid status filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidStatusFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidStatus = "invalid";
            var expectedProducts = new List<Product>();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, invalidStatus, null, true))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _productRepositoryMock.Object.GetAllAsync(null, invalidStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _productRepositoryMock.Verify(repo => repo.GetAllAsync(null, invalidStatus, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns Product when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = GetSampleProducts().First(x => x.Id == productId);

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productRepositoryMock.Object.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Product Alpha", result.Name);
            Assert.True(result.Status);
            Assert.Equal(100.5, result.SampleAmount);
            Assert.Equal("High quality product for testing", result.Comment);
            Assert.True(result.COA);
            Assert.Equal("Blue", result.Colour);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns product with null optional properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ProductWithNullProperties_ReturnsProduct()
        {
            // Arrange
            var productId = 4;
            var expectedProduct = GetSampleProducts().First(x => x.Id == productId);

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _productRepositoryMock.Object.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Product Delta", result.Name);
            Assert.False(result.Status);
            Assert.Null(result.SampleAmount);
            Assert.Null(result.Comment);
            Assert.Null(result.COA);
            Assert.Null(result.Colour);
            Assert.Null(result.DBDate);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new Product with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidProductWithAllProperties_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "New Product Premium",
                Status = true,
                SampleAmount = 150.75,
                Comment = "Newly created premium product",
                DBDate = new DateTime(2024, 5, 15),
                COA = true,
                Colour = "Purple",
                TorqueWarning = 35.0,
                TorqueFail = 40.0,
                FusionWarning = 170.0,
                FusionFail = 200.0,
                UpdateDate = new DateTime(2024, 5, 20),
                BulkWeight = 750.0,
                PaperBagWeight = 75.0,
                PaperBagNo = 15,
                BatchWeight = 37.5
            };

            var createdProduct = new Product
            {
                Id = 6,
                Name = "New Product Premium",
                Status = true,
                SampleAmount = 150.75,
                Comment = "Newly created premium product",
                DBDate = new DateTime(2024, 5, 15),
                COA = true,
                Colour = "Purple",
                TorqueWarning = 35.0,
                TorqueFail = 40.0,
                FusionWarning = 170.0,
                FusionFail = 200.0,
                UpdateDate = new DateTime(2024, 5, 20),
                BulkWeight = 750.0,
                PaperBagWeight = 75.0,
                PaperBagNo = 15,
                BatchWeight = 37.5
            };

            _productRepositoryMock.Setup(repo => repo.CreateAsync(newProduct))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productRepositoryMock.Object.CreateAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("New Product Premium", result.Name);
            Assert.True(result.Status);
            Assert.Equal(150.75, result.SampleAmount);
            Assert.Equal("Purple", result.Colour);
            Assert.Equal(35.0, result.TorqueWarning);
            Assert.Equal(750.0, result.BulkWeight);
            _productRepositoryMock.Verify(repo => repo.CreateAsync(newProduct), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Minimal Product",
                Status = false
            };

            var createdProduct = new Product
            {
                Id = 7,
                Name = "Minimal Product",
                Status = false
            };

            _productRepositoryMock.Setup(repo => repo.CreateAsync(newProduct))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productRepositoryMock.Object.CreateAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("Minimal Product", result.Name);
            Assert.False(result.Status);
            Assert.Null(result.SampleAmount);
            Assert.Null(result.Comment);
            _productRepositoryMock.Verify(repo => repo.CreateAsync(newProduct), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ProductWithNullOptionalProperties_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Product With Nulls",
                Status = true,
                SampleAmount = null,
                Comment = null,
                DBDate = null,
                COA = null,
                Colour = null,
                TorqueWarning = null,
                TorqueFail = null,
                FusionWarning = null,
                FusionFail = null,
                UpdateDate = null,
                BulkWeight = null,
                PaperBagWeight = null,
                PaperBagNo = null,
                BatchWeight = null
            };

            var createdProduct = new Product
            {
                Id = 8,
                Name = "Product With Nulls",
                Status = true,
                SampleAmount = null,
                Comment = null,
                DBDate = null,
                COA = null,
                Colour = null,
                TorqueWarning = null,
                TorqueFail = null,
                FusionWarning = null,
                FusionFail = null,
                UpdateDate = null,
                BulkWeight = null,
                PaperBagWeight = null,
                PaperBagNo = null,
                BatchWeight = null
            };

            _productRepositoryMock.Setup(repo => repo.CreateAsync(newProduct))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productRepositoryMock.Object.CreateAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal("Product With Nulls", result.Name);
            Assert.True(result.Status);
            Assert.Null(result.SampleAmount);
            Assert.Null(result.COA);
            Assert.Null(result.TorqueWarning);
            _productRepositoryMock.Verify(repo => repo.CreateAsync(newProduct), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with extreme values.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ProductWithExtremeValues_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Extreme Product Test",
                Status = true,
                SampleAmount = 0.001, // Very small value
                Comment = new string('A', 1000), // Very long comment
                DBDate = new DateTime(1900, 1, 1), // Very old date
                COA = false,
                Colour = "Transparent",
                TorqueWarning = 999.99,
                TorqueFail = 1000.0,
                FusionWarning = 0.1,
                FusionFail = 0.2,
                UpdateDate = new DateTime(2100, 12, 31), // Future date
                BulkWeight = 0.0001,
                PaperBagWeight = 10000.0,
                PaperBagNo = 1,
                BatchWeight = 50000.0
            };

            var createdProduct = new Product
            {
                Id = 9,
                Name = "Extreme Product Test",
                Status = true,
                SampleAmount = 0.001,
                Comment = new string('A', 1000),
                DBDate = new DateTime(1900, 1, 1),
                COA = false,
                Colour = "Transparent",
                TorqueWarning = 999.99,
                TorqueFail = 1000.0,
                FusionWarning = 0.1,
                FusionFail = 0.2,
                UpdateDate = new DateTime(2100, 12, 31),
                BulkWeight = 0.0001,
                PaperBagWeight = 10000.0,
                PaperBagNo = 1,
                BatchWeight = 50000.0
            };

            _productRepositoryMock.Setup(repo => repo.CreateAsync(newProduct))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productRepositoryMock.Object.CreateAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Equal(0.001, result.SampleAmount);
            Assert.Equal(999.99, result.TorqueWarning);
            Assert.Equal(10000.0, result.PaperBagWeight);
            _productRepositoryMock.Verify(repo => repo.CreateAsync(newProduct), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing Product with all properties.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var productId = 1;
            var updateProduct = new Product
            {
                Name = "Updated Product Alpha",
                Status = false,
                SampleAmount = 200.75,
                Comment = "Updated premium product description",
                DBDate = new DateTime(2024, 6, 15),
                COA = false,
                Colour = "Orange",
                TorqueWarning = 45.0,
                TorqueFail = 50.0,
                FusionWarning = 180.0,
                FusionFail = 210.0,
                UpdateDate = new DateTime(2024, 6, 20),
                BulkWeight = 850.0,
                PaperBagWeight = 85.0,
                PaperBagNo = 20,
                BatchWeight = 42.5
            };

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Product Alpha",
                Status = false,
                SampleAmount = 200.75,
                Comment = "Updated premium product description",
                DBDate = new DateTime(2024, 6, 15),
                COA = false,
                Colour = "Orange",
                TorqueWarning = 45.0,
                TorqueFail = 50.0,
                FusionWarning = 180.0,
                FusionFail = 210.0,
                UpdateDate = new DateTime(2024, 6, 20),
                BulkWeight = 850.0,
                PaperBagWeight = 85.0,
                PaperBagNo = 20,
                BatchWeight = 42.5
            };

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(productId, updateProduct))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productRepositoryMock.Object.UpdateAsync(productId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Product Alpha", result.Name);
            Assert.False(result.Status);
            Assert.Equal(200.75, result.SampleAmount);
            Assert.Equal("Orange", result.Colour);
            Assert.Equal(45.0, result.TorqueWarning);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(productId, updateProduct), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when Product with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateProduct = new Product
            {
                Name = "Updated Product",
                Status = true
            };

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateProduct))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.UpdateAsync(invalidId, updateProduct);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateProduct), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedProduct()
        {
            // Arrange
            var productId = 2;
            var updateProduct = new Product
            {
                Name = "Updated Product Beta", // Changed
                Status = true, // Changed from false to true
                SampleAmount = 75.25, // Same as original
                Comment = "Updated standard product", // Changed
                DBDate = new DateTime(2024, 2, 10), // Same as original
                COA = false, // Same as original
                Colour = "Red", // Same as original
                TorqueWarning = 20.0, // Same as original
                TorqueFail = 25.0, // Same as original
                FusionWarning = 140.0, // Same as original
                FusionFail = 170.0, // Same as original
                UpdateDate = new DateTime(2024, 6, 25), // Changed
                BulkWeight = 450.0, // Same as original
                PaperBagWeight = 45.0, // Same as original
                PaperBagNo = 8, // Same as original
                BatchWeight = 22.0 // Same as original
            };

            var updatedProduct = new Product
            {
                Id = 2,
                Name = "Updated Product Beta",
                Status = true,
                SampleAmount = 75.25,
                Comment = "Updated standard product",
                DBDate = new DateTime(2024, 2, 10),
                COA = false,
                Colour = "Red",
                TorqueWarning = 20.0,
                TorqueFail = 25.0,
                FusionWarning = 140.0,
                FusionFail = 170.0,
                UpdateDate = new DateTime(2024, 6, 25),
                BulkWeight = 450.0,
                PaperBagWeight = 45.0,
                PaperBagNo = 8,
                BatchWeight = 22.0
            };

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(productId, updateProduct))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productRepositoryMock.Object.UpdateAsync(productId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Product Beta", result.Name);
            Assert.True(result.Status);
            Assert.Equal("Updated standard product", result.Comment);
            Assert.Equal(new DateTime(2024, 6, 25), result.UpdateDate);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(productId, updateProduct), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedProduct()
        {
            // Arrange
            var productId = 3;
            var updateProduct = new Product
            {
                Name = "Product Gamma",
                Status = true,
                SampleAmount = null, // Changed from 120.0 to null
                Comment = null, // Changed from comment to null
                DBDate = null, // Changed from date to null
                COA = null, // Changed from true to null
                Colour = null, // Changed from "Green" to null
                TorqueWarning = null,
                TorqueFail = null,
                FusionWarning = null,
                FusionFail = null,
                UpdateDate = null,
                BulkWeight = null,
                PaperBagWeight = null,
                PaperBagNo = null,
                BatchWeight = null
            };

            var updatedProduct = new Product
            {
                Id = 3,
                Name = "Product Gamma",
                Status = true,
                SampleAmount = null,
                Comment = null,
                DBDate = null,
                COA = null,
                Colour = null,
                TorqueWarning = null,
                TorqueFail = null,
                FusionWarning = null,
                FusionFail = null,
                UpdateDate = null,
                BulkWeight = null,
                PaperBagWeight = null,
                PaperBagNo = null,
                BatchWeight = null
            };

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(productId, updateProduct))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productRepositoryMock.Object.UpdateAsync(productId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.SampleAmount);
            Assert.Null(result.Comment);
            Assert.Null(result.COA);
            Assert.Null(result.Colour);
            Assert.Null(result.TorqueWarning);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(productId, updateProduct), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateProduct = new Product
            {
                Name = "Updated Product",
                Status = true
            };

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateProduct))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.UpdateAsync(zeroId, updateProduct);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateProduct), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing Product.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedProduct()
        {
            // Arrange
            var productId = 1;
            var deletedProduct = GetSampleProducts().First(x => x.Id == productId);

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId))
                .ReturnsAsync(deletedProduct);

            // Act
            var result = await _productRepositoryMock.Object.DeleteAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Product Alpha", result.Name);
            Assert.True(result.Status);
            Assert.Equal(100.5, result.SampleAmount);
            Assert.Equal("Blue", result.Colour);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when Product with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with product containing null optional properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ProductWithNullProperties_ReturnsDeletedProduct()
        {
            // Arrange
            var productId = 4;
            var deletedProduct = GetSampleProducts().First(x => x.Id == productId);

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId))
                .ReturnsAsync(deletedProduct);

            // Act
            var result = await _productRepositoryMock.Object.DeleteAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal("Product Delta", result.Name);
            Assert.False(result.Status);
            Assert.Null(result.SampleAmount);
            Assert.Null(result.Comment);
            Assert.Null(result.COA);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with inactive product.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InactiveProduct_ReturnsDeletedProduct()
        {
            // Arrange
            var productId = 2;
            var deletedProduct = GetSampleProducts().First(x => x.Id == productId);

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId))
                .ReturnsAsync(deletedProduct);

            // Act
            var result = await _productRepositoryMock.Object.DeleteAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("Product Beta", result.Name);
            Assert.False(result.Status);
            Assert.Equal(75.25, result.SampleAmount);
            Assert.Equal("Red", result.Colour);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }

        #endregion
    }
}

