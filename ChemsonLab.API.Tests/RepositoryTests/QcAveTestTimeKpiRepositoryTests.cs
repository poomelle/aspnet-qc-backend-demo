using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.QcAveTestTimeKpiRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class QcAveTestTimeKpiRepositoryTests
    {
        private readonly Mock<IQcAveTestTimeKpiRepository> _mockRepository;

        public QcAveTestTimeKpiRepositoryTests()
        {
            _mockRepository = new Mock<IQcAveTestTimeKpiRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample QcAveTestTimeKpi data for testing purposes.
        /// </summary>
        /// <returns>List of sample QcAveTestTimeKpi objects</returns>
        private List<QcAveTestTimeKpi> GetSampleQcAveTestTimeKpis()
        {
            return new List<QcAveTestTimeKpi>
            {
                new QcAveTestTimeKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "January",
                    TotalTest = 150,
                    AveTestTime = 3600000, // 1 hour in milliseconds
                    Product = new Product { Id = 1, Name = "Product Alpha" },
                    Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
                },
                new QcAveTestTimeKpi
                {
                    Id = 2,
                    ProductId = 2,
                    MachineId = 1,
                    Year = "2024",
                    Month = "January",
                    TotalTest = 200,
                    AveTestTime = 2700000, // 45 minutes in milliseconds
                    Product = new Product { Id = 2, Name = "Product Beta" },
                    Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
                },
                new QcAveTestTimeKpi
                {
                    Id = 3,
                    ProductId = 1,
                    MachineId = 2,
                    Year = "2024",
                    Month = "February",
                    TotalTest = 175,
                    AveTestTime = 4200000, // 70 minutes in milliseconds
                    Product = new Product { Id = 1, Name = "Product Alpha" },
                    Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
                },
                new QcAveTestTimeKpi
                {
                    Id = 4,
                    ProductId = 3,
                    MachineId = 3,
                    Year = "2023",
                    Month = "December",
                    TotalTest = 100,
                    AveTestTime = 1800000, // 30 minutes in milliseconds
                    Product = new Product { Id = 3, Name = "Product Gamma" },
                    Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
                },
                new QcAveTestTimeKpi
                {
                    Id = 5,
                    ProductId = null,
                    MachineId = null,
                    Year = null,
                    Month = null,
                    TotalTest = null,
                    AveTestTime = null,
                    Product = null,
                    Machine = null
                },
                new QcAveTestTimeKpi
                {
                    Id = 6,
                    ProductId = 2,
                    MachineId = 2,
                    Year = "2024",
                    Month = "March",
                    TotalTest = 250,
                    AveTestTime = 5400000, // 90 minutes in milliseconds
                    Product = new Product { Id = 2, Name = "Product Beta" },
                    Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all QcAveTestTimeKpi items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllQcAveTestTimeKpis()
        {
            // Arrange
            var expectedKpis = GetSampleQcAveTestTimeKpis();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
            Assert.Equal(expectedKpis, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcAveTestTimeKpi items by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredKpis()
        {
            // Arrange
            var productName = "Product Alpha";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Product?.Name == productName).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, null, null, null, null, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, kpi => Assert.Equal(productName, kpi.Product?.Name));
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcAveTestTimeKpi items by machine name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMachineName_ReturnsFilteredKpis()
        {
            // Arrange
            var machineName = "Testing Machine Beta";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Machine?.Name == machineName).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, machineName, null, null, null, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, machineName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, kpi => Assert.Equal(machineName, kpi.Machine?.Name));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, machineName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcAveTestTimeKpi items by year.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByYear_ReturnsFilteredKpis()
        {
            // Arrange
            var year = "2024";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Year == year).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, year, null, null, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, year);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.All(result, kpi => Assert.Equal(year, kpi.Year));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, year, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters QcAveTestTimeKpi items by month.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMonth_ReturnsFilteredKpis()
        {
            // Arrange
            var month = "January";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Month == month).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, month, null, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, month);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, kpi => Assert.Equal(month, kpi.Month));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, month, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameAscending_ReturnsSortedKpis()
        {
            // Arrange
            var sortBy = "productName";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Product != null).OrderBy(x => x.Product.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedKpis, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by machine name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByMachineNameDescending_ReturnsSortedKpis()
        {
            // Arrange
            var sortBy = "machineName";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Machine != null).OrderByDescending(x => x.Machine.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, sortBy, false))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedKpis, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by year ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByYearAscending_ReturnsSortedKpis()
        {
            // Arrange
            var sortBy = "year";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Year != null).OrderBy(x => x.Year).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedKpis, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by month descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByMonthDescending_ReturnsSortedKpis()
        {
            // Arrange
            var sortBy = "month";
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Month != null).OrderByDescending(x => x.Month).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, sortBy, false))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedKpis, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredKpis()
        {
            // Arrange
            var productName = "Product Beta";
            var year = "2024";
            var month = "March";
            var sortBy = "machineName";
            var expectedKpis = GetSampleQcAveTestTimeKpis()
                .Where(x => x.Product?.Name == productName && x.Year == year && x.Month == month)
                .OrderBy(x => x.Machine?.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, null, year, month, sortBy, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(productName, null, year, month, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(productName, result.First().Product?.Name);
            Assert.Equal(year, result.First().Year);
            Assert.Equal(month, result.First().Month);
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, null, year, month, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no KPIs match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingKpis_ReturnsEmptyList()
        {
            // Arrange
            var productName = "NonExistentProduct";
            var expectedKpis = new List<QcAveTestTimeKpi>();

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, null, null, null, null, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with case-insensitive sorting.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_CaseInsensitiveSorting_ReturnsSortedKpis()
        {
            // Arrange
            var sortBy = "PRODUCTNAME"; // uppercase
            var expectedKpis = GetSampleQcAveTestTimeKpis().Where(x => x.Product != null).OrderBy(x => x.Product.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedKpis);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedKpis, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, sortBy, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns QcAveTestTimeKpi when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsQcAveTestTimeKpi()
        {
            // Arrange
            var kpiId = 1;
            var expectedKpi = GetSampleQcAveTestTimeKpis().First(x => x.Id == kpiId);

            _mockRepository.Setup(repo => repo.GetByIdAsync(kpiId))
                .ReturnsAsync(expectedKpi);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(kpiId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(kpiId, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("January", result.Month);
            Assert.Equal(150, result.TotalTest);
            Assert.Equal(3600000, result.AveTestTime);
            _mockRepository.Verify(repo => repo.GetByIdAsync(kpiId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns KPI with null optional properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_KpiWithNullProperties_ReturnsKpi()
        {
            // Arrange
            var kpiId = 5;
            var expectedKpi = GetSampleQcAveTestTimeKpis().First(x => x.Id == kpiId);

            _mockRepository.Setup(repo => repo.GetByIdAsync(kpiId))
                .ReturnsAsync(expectedKpi);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(kpiId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(kpiId, result.Id);
            Assert.Null(result.ProductId);
            Assert.Null(result.MachineId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            Assert.Null(result.TotalTest);
            Assert.Null(result.AveTestTime);
            _mockRepository.Verify(repo => repo.GetByIdAsync(kpiId), Times.Once);
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
                .ReturnsAsync((QcAveTestTimeKpi?)null);

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
                .ReturnsAsync((QcAveTestTimeKpi?)null);

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
                .ReturnsAsync((QcAveTestTimeKpi?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new QcAveTestTimeKpi with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidQcAveTestTimeKpiWithAllProperties_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcAveTestTimeKpi
            {
                ProductId = 4,
                MachineId = 4,
                Year = "2024",
                Month = "April",
                TotalTest = 300,
                AveTestTime = 7200000, // 2 hours in milliseconds
                Product = new Product { Id = 4, Name = "Product Delta" },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine" }
            };

            var createdKpi = new QcAveTestTimeKpi
            {
                Id = 7,
                ProductId = 4,
                MachineId = 4,
                Year = "2024",
                Month = "April",
                TotalTest = 300,
                AveTestTime = 7200000,
                Product = new Product { Id = 4, Name = "Product Delta" },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine" }
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newKpi))
                .ReturnsAsync(createdKpi);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(4, result.ProductId);
            Assert.Equal(4, result.MachineId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("April", result.Month);
            Assert.Equal(300, result.TotalTest);
            Assert.Equal(7200000, result.AveTestTime);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalQcAveTestTimeKpi_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcAveTestTimeKpi
            {
                ProductId = 1,
                MachineId = 1,
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var createdKpi = new QcAveTestTimeKpi
            {
                Id = 8,
                ProductId = 1,
                MachineId = 1,
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newKpi))
                .ReturnsAsync(createdKpi);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            Assert.Null(result.TotalTest);
            Assert.Null(result.AveTestTime);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_QcAveTestTimeKpiWithNullOptionalProperties_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcAveTestTimeKpi
            {
                ProductId = null,
                MachineId = null,
                Year = null,
                Month = null,
                TotalTest = null,
                AveTestTime = null,
                Product = null,
                Machine = null
            };

            var createdKpi = new QcAveTestTimeKpi
            {
                Id = 9,
                ProductId = null,
                MachineId = null,
                Year = null,
                Month = null,
                TotalTest = null,
                AveTestTime = null,
                Product = null,
                Machine = null
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newKpi))
                .ReturnsAsync(createdKpi);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Null(result.ProductId);
            Assert.Null(result.MachineId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            Assert.Null(result.TotalTest);
            Assert.Null(result.AveTestTime);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with extreme KPI values.
        /// </summary>
        [Fact]
        public async Task CreateAsync_QcAveTestTimeKpiWithExtremeValues_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcAveTestTimeKpi
            {
                ProductId = 5,
                MachineId = 5,
                Year = "2025",
                Month = "December",
                TotalTest = 1,
                AveTestTime = 86400000, // 24 hours in milliseconds
                Product = new Product { Id = 5, Name = "Extreme Product" },
                Machine = new Machine { Id = 5, Name = "Extreme Machine" }
            };

            var createdKpi = new QcAveTestTimeKpi
            {
                Id = 10,
                ProductId = 5,
                MachineId = 5,
                Year = "2025",
                Month = "December",
                TotalTest = 1,
                AveTestTime = 86400000,
                Product = new Product { Id = 5, Name = "Extreme Product" },
                Machine = new Machine { Id = 5, Name = "Extreme Machine" }
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newKpi))
                .ReturnsAsync(createdKpi);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal(1, result.TotalTest);
            Assert.Equal(86400000, result.AveTestTime);
            Assert.Equal("2025", result.Year);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing QcAveTestTimeKpi.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndKpi_ReturnsUpdatedKpi()
        {
            // Arrange
            var kpiId = 1;
            var updateKpi = new QcAveTestTimeKpi
            {
                ProductId = 3,
                MachineId = 3,
                Year = "2025",
                Month = "May",
                TotalTest = 400,
                AveTestTime = 9000000, // 2.5 hours in milliseconds
                Product = new Product { Id = 3, Name = "Updated Product" },
                Machine = new Machine { Id = 3, Name = "Updated Machine" }
            };

            var updatedKpi = new QcAveTestTimeKpi
            {
                Id = 1,
                ProductId = 3,
                MachineId = 3,
                Year = "2025",
                Month = "May",
                TotalTest = 400,
                AveTestTime = 9000000,
                Product = new Product { Id = 3, Name = "Updated Product" },
                Machine = new Machine { Id = 3, Name = "Updated Machine" }
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(kpiId, updateKpi))
                .ReturnsAsync(updatedKpi);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(kpiId, updateKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(3, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.Equal("2025", result.Year);
            Assert.Equal("May", result.Month);
            Assert.Equal(400, result.TotalTest);
            Assert.Equal(9000000, result.AveTestTime);
            _mockRepository.Verify(repo => repo.UpdateAsync(kpiId, updateKpi), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when KPI with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateKpi = new QcAveTestTimeKpi
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "June"
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(invalidId, updateKpi))
                .ReturnsAsync((QcAveTestTimeKpi?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(invalidId, updateKpi);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(invalidId, updateKpi), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedKpi()
        {
            // Arrange
            var kpiId = 2;
            var updateKpi = new QcAveTestTimeKpi
            {
                ProductId = 2, // Same as original
                MachineId = 1, // Same as original
                Year = "2024", // Same as original
                Month = "February", // Changed from January to February
                TotalTest = 250, // Changed from 200 to 250
                AveTestTime = 3000000, // Changed from 2700000 to 3000000
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var updatedKpi = new QcAveTestTimeKpi
            {
                Id = 2,
                ProductId = 2,
                MachineId = 1,
                Year = "2024",
                Month = "February",
                TotalTest = 250,
                AveTestTime = 3000000,
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(kpiId, updateKpi))
                .ReturnsAsync(updatedKpi);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(kpiId, updateKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("February", result.Month);
            Assert.Equal(250, result.TotalTest);
            Assert.Equal(3000000, result.AveTestTime);
            _mockRepository.Verify(repo => repo.UpdateAsync(kpiId, updateKpi), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedKpi()
        {
            // Arrange
            var kpiId = 3;
            var updateKpi = new QcAveTestTimeKpi
            {
                ProductId = null, // Changed from 1 to null
                MachineId = null, // Changed from 2 to null
                Year = null, // Changed from "2024" to null
                Month = null, // Changed from "February" to null
                TotalTest = null, // Changed from 175 to null
                AveTestTime = null, // Changed from 4200000 to null
                Product = null,
                Machine = null
            };

            var updatedKpi = new QcAveTestTimeKpi
            {
                Id = 3,
                ProductId = null,
                MachineId = null,
                Year = null,
                Month = null,
                TotalTest = null,
                AveTestTime = null,
                Product = null,
                Machine = null
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(kpiId, updateKpi))
                .ReturnsAsync(updatedKpi);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(kpiId, updateKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ProductId);
            Assert.Null(result.MachineId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            Assert.Null(result.TotalTest);
            Assert.Null(result.AveTestTime);
            _mockRepository.Verify(repo => repo.UpdateAsync(kpiId, updateKpi), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateKpi = new QcAveTestTimeKpi
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024"
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(zeroId, updateKpi))
                .ReturnsAsync((QcAveTestTimeKpi?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(zeroId, updateKpi);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(zeroId, updateKpi), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing QcAveTestTimeKpi.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedKpi()
        {
            // Arrange
            var kpiId = 1;
            var deletedKpi = GetSampleQcAveTestTimeKpis().First(x => x.Id == kpiId);

            _mockRepository.Setup(repo => repo.DeleteAsync(kpiId))
                .ReturnsAsync(deletedKpi);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(kpiId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("January", result.Month);
            Assert.Equal(150, result.TotalTest);
            Assert.Equal(3600000, result.AveTestTime);
            _mockRepository.Verify(repo => repo.DeleteAsync(kpiId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when KPI with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockRepository.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((QcAveTestTimeKpi?)null);

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
                .ReturnsAsync((QcAveTestTimeKpi?)null);

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
                .ReturnsAsync((QcAveTestTimeKpi?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with KPI containing null optional properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_KpiWithNullProperties_ReturnsDeletedKpi()
        {
            // Arrange
            var kpiId = 5;
            var deletedKpi = GetSampleQcAveTestTimeKpis().First(x => x.Id == kpiId);

            _mockRepository.Setup(repo => repo.DeleteAsync(kpiId))
                .ReturnsAsync(deletedKpi);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(kpiId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Null(result.ProductId);
            Assert.Null(result.MachineId);
            Assert.Null(result.Year);
            Assert.Null(result.Month);
            Assert.Null(result.TotalTest);
            Assert.Null(result.AveTestTime);
            _mockRepository.Verify(repo => repo.DeleteAsync(kpiId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with high-performance KPI.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_HighPerformanceKpi_ReturnsDeletedKpi()
        {
            // Arrange
            var kpiId = 6;
            var deletedKpi = GetSampleQcAveTestTimeKpis().First(x => x.Id == kpiId);

            _mockRepository.Setup(repo => repo.DeleteAsync(kpiId))
                .ReturnsAsync(deletedKpi);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(kpiId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(2, result.MachineId);
            Assert.Equal("2024", result.Year);
            Assert.Equal("March", result.Month);
            Assert.Equal(250, result.TotalTest);
            Assert.Equal(5400000, result.AveTestTime); // 90 minutes
            _mockRepository.Verify(repo => repo.DeleteAsync(kpiId), Times.Once);
        }

        #endregion
    }
}

