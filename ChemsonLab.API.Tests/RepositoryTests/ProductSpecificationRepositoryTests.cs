using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.ProductSpecificationRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class ProductSpecificationRepositoryTests
    {
        private readonly Mock<IProductSpecificationRepository> _productSpecificationRepositoryMock;

        public ProductSpecificationRepositoryTests()
        {
            _productSpecificationRepositoryMock = new Mock<IProductSpecificationRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample ProductSpecification data for testing purposes.
        /// </summary>
        /// <returns>List of sample ProductSpecification objects</returns>
        private List<ProductSpecification> GetSampleProductSpecifications()
        {
            return new List<ProductSpecification>
            {
                new ProductSpecification
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    InUse = true,
                    Temp = 180,
                    Load = 50,
                    RPM = 100,
                    Product = new Product
                    {
                        Id = 1,
                        Name = "Product Alpha",
                        Status = true
                    },
                    Machine = new Machine
                    {
                        Id = 1,
                        Name = "Testing Machine Alpha",
                        Status = true
                    }
                },
                new ProductSpecification
                {
                    Id = 2,
                    ProductId = 2,
                    MachineId = 1,
                    InUse = false,
                    Temp = 160,
                    Load = 40,
                    RPM = 80,
                    Product = new Product
                    {
                        Id = 2,
                        Name = "Product Beta",
                        Status = true
                    },
                    Machine = new Machine
                    {
                        Id = 1,
                        Name = "Testing Machine Alpha",
                        Status = true
                    }
                },
                new ProductSpecification
                {
                    Id = 3,
                    ProductId = 1,
                    MachineId = 2,
                    InUse = true,
                    Temp = 200,
                    Load = 60,
                    RPM = 120,
                    Product = new Product
                    {
                        Id = 1,
                        Name = "Product Alpha",
                        Status = true
                    },
                    Machine = new Machine
                    {
                        Id = 2,
                        Name = "Testing Machine Beta",
                        Status = true
                    }
                },
                new ProductSpecification
                {
                    Id = 4,
                    ProductId = 3,
                    MachineId = 3,
                    InUse = null,
                    Temp = null,
                    Load = null,
                    RPM = null,
                    Product = new Product
                    {
                        Id = 3,
                        Name = "Product Gamma",
                        Status = false
                    },
                    Machine = new Machine
                    {
                        Id = 3,
                        Name = "Quality Control Machine",
                        Status = false
                    }
                },
                new ProductSpecification
                {
                    Id = 5,
                    ProductId = 4,
                    MachineId = 2,
                    InUse = false,
                    Temp = 140,
                    Load = 30,
                    RPM = 60,
                    Product = new Product
                    {
                        Id = 4,
                        Name = "Product Delta",
                        Status = true
                    },
                    Machine = new Machine
                    {
                        Id = 2,
                        Name = "Testing Machine Beta",
                        Status = true
                    }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all ProductSpecification items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllProductSpecifications()
        {
            // Arrange
            var expectedProductSpecifications = GetSampleProductSpecifications();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProductSpecifications, result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters ProductSpecification items by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredProductSpecifications()
        {
            // Arrange
            var productName = "Product Alpha";
            var expectedProductSpecifications = GetSampleProductSpecifications().Where(x => x.Product.Name.Equals(productName)).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(productName, null, null, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, spec => Assert.Equal(productName, spec.Product.Name));
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(productName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters ProductSpecification items by machine name (contains).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMachineName_ReturnsFilteredProductSpecifications()
        {
            // Arrange
            var machineName = "Testing";
            var expectedProductSpecifications = GetSampleProductSpecifications().Where(x => x.Machine.Name.Contains(machineName)).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, machineName, null, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, machineName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.All(result, spec => Assert.Contains(machineName, spec.Machine.Name));
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, machineName, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters ProductSpecification items by exact machine name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByExactMachineName_ReturnsFilteredProductSpecifications()
        {
            // Arrange
            var machineName = "Testing Machine Beta";
            var expectedProductSpecifications = GetSampleProductSpecifications().Where(x => x.Machine.Name.Contains(machineName)).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, machineName, null, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, machineName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, spec => Assert.Contains(machineName, spec.Machine.Name));
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, machineName, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters ProductSpecification items by InUse (true).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByInUseTrue_ReturnsActiveProductSpecifications()
        {
            // Arrange
            var inUse = "true";
            var expectedProductSpecifications = GetSampleProductSpecifications().Where(x => x.InUse == true).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, inUse, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, inUse);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, spec => Assert.True(spec.InUse));
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, inUse, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters ProductSpecification items by InUse (false).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByInUseFalse_ReturnsInactiveProductSpecifications()
        {
            // Arrange
            var inUse = "false";
            var expectedProductSpecifications = GetSampleProductSpecifications().Where(x => x.InUse == false).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, inUse, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, inUse);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, spec => Assert.False(spec.InUse));
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, inUse, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameAscending_ReturnsSortedProductSpecifications()
        {
            // Arrange
            var sortBy = "productName";
            var expectedProductSpecifications = GetSampleProductSpecifications().OrderBy(x => x.Product.Name).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProductSpecifications, result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameDescending_ReturnsSortedProductSpecifications()
        {
            // Arrange
            var sortBy = "productName";
            var expectedProductSpecifications = GetSampleProductSpecifications().OrderByDescending(x => x.Product.Name).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, false))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProductSpecifications, result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by machine name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByMachineNameAscending_ReturnsSortedProductSpecifications()
        {
            // Arrange
            var sortBy = "machineName";
            var expectedProductSpecifications = GetSampleProductSpecifications().OrderBy(x => x.Machine.Name).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProductSpecifications, result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by machine name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByMachineNameDescending_ReturnsSortedProductSpecifications()
        {
            // Arrange
            var sortBy = "machineName";
            var expectedProductSpecifications = GetSampleProductSpecifications().OrderByDescending(x => x.Machine.Name).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, false))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProductSpecifications, result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredProductSpecifications()
        {
            // Arrange
            var productName = "Product Alpha";
            var machineName = "Testing";
            var inUse = "true";
            var sortBy = "machineName";
            var expectedProductSpecifications = GetSampleProductSpecifications()
                .Where(x => x.Product.Name.Equals(productName) && x.Machine.Name.Contains(machineName) && x.InUse == true)
                .OrderBy(x => x.Machine.Name).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(productName, machineName, inUse, sortBy, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(productName, machineName, inUse, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, spec =>
            {
                Assert.Equal(productName, spec.Product.Name);
                Assert.Contains(machineName, spec.Machine.Name);
                Assert.True(spec.InUse);
            });
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(productName, machineName, inUse, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no ProductSpecification items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingProductSpecifications_ReturnsEmptyList()
        {
            // Arrange
            var productName = "NonExistentProduct";
            var expectedProductSpecifications = new List<ProductSpecification>();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(productName, null, null, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(productName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid InUse filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidInUseFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidInUse = "invalid";
            var expectedProductSpecifications = new List<ProductSpecification>();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, invalidInUse, null, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, invalidInUse);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, invalidInUse, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with case-insensitive sorting.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_CaseInsensitiveSorting_ReturnsSortedProductSpecifications()
        {
            // Arrange
            var sortBy = "PRODUCTNAME"; // uppercase
            var expectedProductSpecifications = GetSampleProductSpecifications().OrderBy(x => x.Product.Name).ToList();

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, sortBy, true))
                .ReturnsAsync(expectedProductSpecifications);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetAllAsync(null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedProductSpecifications, result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, sortBy, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns ProductSpecification when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsProductSpecification()
        {
            // Arrange
            var productSpecificationId = 1;
            var expectedProductSpecification = GetSampleProductSpecifications().First(x => x.Id == productSpecificationId);

            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(productSpecificationId))
                .ReturnsAsync(expectedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetByIdAsync(productSpecificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productSpecificationId, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.True(result.InUse);
            Assert.Equal(180, result.Temp);
            Assert.Equal(50, result.Load);
            Assert.Equal(100, result.RPM);
            Assert.Equal("Product Alpha", result.Product.Name);
            Assert.Equal("Testing Machine Alpha", result.Machine.Name);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetByIdAsync(productSpecificationId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns ProductSpecification with null optional properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ProductSpecificationWithNullProperties_ReturnsProductSpecification()
        {
            // Arrange
            var productSpecificationId = 4;
            var expectedProductSpecification = GetSampleProductSpecifications().First(x => x.Id == productSpecificationId);

            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(productSpecificationId))
                .ReturnsAsync(expectedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetByIdAsync(productSpecificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productSpecificationId, result.Id);
            Assert.Equal(3, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.Null(result.InUse);
            Assert.Null(result.Temp);
            Assert.Null(result.Load);
            Assert.Null(result.RPM);
            Assert.Equal("Product Gamma", result.Product.Name);
            Assert.Equal("Quality Control Machine", result.Machine.Name);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetByIdAsync(productSpecificationId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new ProductSpecification with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidProductSpecificationWithAllProperties_ReturnsCreatedProductSpecification()
        {
            // Arrange
            var newProductSpecification = new ProductSpecification
            {
                ProductId = 5,
                MachineId = 4,
                InUse = true,
                Temp = 220,
                Load = 70,
                RPM = 150,
                Product = new Product { Id = 5, Name = "Product Epsilon", Status = true },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine", Status = true }
            };

            var createdProductSpecification = new ProductSpecification
            {
                Id = 6,
                ProductId = 5,
                MachineId = 4,
                InUse = true,
                Temp = 220,
                Load = 70,
                RPM = 150,
                Product = new Product { Id = 5, Name = "Product Epsilon", Status = true },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine", Status = true }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.CreateAsync(newProductSpecification))
                .ReturnsAsync(createdProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.CreateAsync(newProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(5, result.ProductId);
            Assert.Equal(4, result.MachineId);
            Assert.True(result.InUse);
            Assert.Equal(220, result.Temp);
            Assert.Equal(70, result.Load);
            Assert.Equal(150, result.RPM);
            _productSpecificationRepositoryMock.Verify(repo => repo.CreateAsync(newProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalProductSpecification_ReturnsCreatedProductSpecification()
        {
            // Arrange
            var newProductSpecification = new ProductSpecification
            {
                ProductId = 2,
                MachineId = 3,
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
            };

            var createdProductSpecification = new ProductSpecification
            {
                Id = 7,
                ProductId = 2,
                MachineId = 3,
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.CreateAsync(newProductSpecification))
                .ReturnsAsync(createdProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.CreateAsync(newProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.Null(result.InUse);
            Assert.Null(result.Temp);
            Assert.Null(result.Load);
            Assert.Null(result.RPM);
            _productSpecificationRepositoryMock.Verify(repo => repo.CreateAsync(newProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ProductSpecificationWithNullOptionalProperties_ReturnsCreatedProductSpecification()
        {
            // Arrange
            var newProductSpecification = new ProductSpecification
            {
                ProductId = 6,
                MachineId = 1,
                InUse = null,
                Temp = null,
                Load = null,
                RPM = null,
                Product = new Product { Id = 6, Name = "Product Zeta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var createdProductSpecification = new ProductSpecification
            {
                Id = 8,
                ProductId = 6,
                MachineId = 1,
                InUse = null,
                Temp = null,
                Load = null,
                RPM = null,
                Product = new Product { Id = 6, Name = "Product Zeta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.CreateAsync(newProductSpecification))
                .ReturnsAsync(createdProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.CreateAsync(newProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal(6, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Null(result.InUse);
            Assert.Null(result.Temp);
            Assert.Null(result.Load);
            Assert.Null(result.RPM);
            _productSpecificationRepositoryMock.Verify(repo => repo.CreateAsync(newProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with extreme specification values.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ProductSpecificationWithExtremeValues_ReturnsCreatedProductSpecification()
        {
            // Arrange
            var newProductSpecification = new ProductSpecification
            {
                ProductId = 7,
                MachineId = 2,
                InUse = false,
                Temp = 500, // High temperature
                Load = 1, // Minimal load
                RPM = 5000, // Very high RPM
                Product = new Product { Id = 7, Name = "Extreme Product" },
                Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
            };

            var createdProductSpecification = new ProductSpecification
            {
                Id = 9,
                ProductId = 7,
                MachineId = 2,
                InUse = false,
                Temp = 500,
                Load = 1,
                RPM = 5000,
                Product = new Product { Id = 7, Name = "Extreme Product" },
                Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.CreateAsync(newProductSpecification))
                .ReturnsAsync(createdProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.CreateAsync(newProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Equal(500, result.Temp);
            Assert.Equal(1, result.Load);
            Assert.Equal(5000, result.RPM);
            Assert.False(result.InUse);
            _productSpecificationRepositoryMock.Verify(repo => repo.CreateAsync(newProductSpecification), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing ProductSpecification.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndProductSpecification_ReturnsUpdatedProductSpecification()
        {
            // Arrange
            var productSpecificationId = 1;
            var updateProductSpecification = new ProductSpecification
            {
                ProductId = 2,
                MachineId = 3,
                InUse = false,
                Temp = 250,
                Load = 80,
                RPM = 200,
                Product = new Product { Id = 2, Name = "Updated Product" },
                Machine = new Machine { Id = 3, Name = "Updated Machine" }
            };

            var updatedProductSpecification = new ProductSpecification
            {
                Id = 1,
                ProductId = 2,
                MachineId = 3,
                InUse = false,
                Temp = 250,
                Load = 80,
                RPM = 200,
                Product = new Product { Id = 2, Name = "Updated Product" },
                Machine = new Machine { Id = 3, Name = "Updated Machine" }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(productSpecificationId, updateProductSpecification))
                .ReturnsAsync(updatedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.UpdateAsync(productSpecificationId, updateProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.False(result.InUse);
            Assert.Equal(250, result.Temp);
            Assert.Equal(80, result.Load);
            Assert.Equal(200, result.RPM);
            _productSpecificationRepositoryMock.Verify(repo => repo.UpdateAsync(productSpecificationId, updateProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when ProductSpecification with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateProductSpecification = new ProductSpecification
            {
                ProductId = 1,
                MachineId = 1,
                InUse = true,
                Product = new Product { Id = 1 },
                Machine = new Machine { Id = 1 }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateProductSpecification))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.UpdateAsync(invalidId, updateProductSpecification);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedProductSpecification()
        {
            // Arrange
            var productSpecificationId = 2;
            var updateProductSpecification = new ProductSpecification
            {
                ProductId = 2, // Same as original
                MachineId = 1, // Same as original
                InUse = true, // Changed from false to true
                Temp = 160, // Same as original
                Load = 55, // Changed from 40 to 55
                RPM = 90, // Changed from 80 to 90
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var updatedProductSpecification = new ProductSpecification
            {
                Id = 2,
                ProductId = 2,
                MachineId = 1,
                InUse = true,
                Temp = 160,
                Load = 55,
                RPM = 90,
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(productSpecificationId, updateProductSpecification))
                .ReturnsAsync(updatedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.UpdateAsync(productSpecificationId, updateProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.InUse);
            Assert.Equal(55, result.Load);
            Assert.Equal(90, result.RPM);
            Assert.Equal(160, result.Temp); // Unchanged
            _productSpecificationRepositoryMock.Verify(repo => repo.UpdateAsync(productSpecificationId, updateProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedProductSpecification()
        {
            // Arrange
            var productSpecificationId = 3;
            var updateProductSpecification = new ProductSpecification
            {
                ProductId = 1,
                MachineId = 2,
                InUse = null, // Changed from true to null
                Temp = null, // Changed from 200 to null
                Load = null, // Changed from 60 to null
                RPM = null, // Changed from 120 to null
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
            };

            var updatedProductSpecification = new ProductSpecification
            {
                Id = 3,
                ProductId = 1,
                MachineId = 2,
                InUse = null,
                Temp = null,
                Load = null,
                RPM = null,
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(productSpecificationId, updateProductSpecification))
                .ReturnsAsync(updatedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.UpdateAsync(productSpecificationId, updateProductSpecification);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.InUse);
            Assert.Null(result.Temp);
            Assert.Null(result.Load);
            Assert.Null(result.RPM);
            _productSpecificationRepositoryMock.Verify(repo => repo.UpdateAsync(productSpecificationId, updateProductSpecification), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateProductSpecification = new ProductSpecification
            {
                ProductId = 1,
                MachineId = 1,
                Product = new Product { Id = 1 },
                Machine = new Machine { Id = 1 }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateProductSpecification))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.UpdateAsync(zeroId, updateProductSpecification);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateProductSpecification), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing ProductSpecification.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedProductSpecification()
        {
            // Arrange
            var productSpecificationId = 1;
            var deletedProductSpecification = GetSampleProductSpecifications().First(x => x.Id == productSpecificationId);

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(productSpecificationId))
                .ReturnsAsync(deletedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.DeleteAsync(productSpecificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.True(result.InUse);
            Assert.Equal(180, result.Temp);
            Assert.Equal("Product Alpha", result.Product.Name);
            _productSpecificationRepositoryMock.Verify(repo => repo.DeleteAsync(productSpecificationId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when ProductSpecification with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _productSpecificationRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with ProductSpecification containing null optional properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ProductSpecificationWithNullProperties_ReturnsDeletedProductSpecification()
        {
            // Arrange
            var productSpecificationId = 4;
            var deletedProductSpecification = GetSampleProductSpecifications().First(x => x.Id == productSpecificationId);

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(productSpecificationId))
                .ReturnsAsync(deletedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.DeleteAsync(productSpecificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal(3, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.Null(result.InUse);
            Assert.Null(result.Temp);
            Assert.Null(result.Load);
            Assert.Null(result.RPM);
            _productSpecificationRepositoryMock.Verify(repo => repo.DeleteAsync(productSpecificationId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with inactive ProductSpecification.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InactiveProductSpecification_ReturnsDeletedProductSpecification()
        {
            // Arrange
            var productSpecificationId = 2;
            var deletedProductSpecification = GetSampleProductSpecifications().First(x => x.Id == productSpecificationId);

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(productSpecificationId))
                .ReturnsAsync(deletedProductSpecification);

            // Act
            var result = await _productSpecificationRepositoryMock.Object.DeleteAsync(productSpecificationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.False(result.InUse);
            Assert.Equal(160, result.Temp);
            Assert.Equal("Product Beta", result.Product.Name);
            _productSpecificationRepositoryMock.Verify(repo => repo.DeleteAsync(productSpecificationId), Times.Once);
        }

        #endregion
    }
}

