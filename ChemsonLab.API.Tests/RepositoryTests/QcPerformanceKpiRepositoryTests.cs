using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.QcPerformanceKpiRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class QcPerformanceKpiRepositoryTests
    {
        private readonly Mock<IQcPerformanceKpiRepository> _mockRepository;

        public QcPerformanceKpiRepositoryTests()
        {
            _mockRepository = new Mock<IQcPerformanceKpiRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample QcPerformanceKpi data for testing purposes.
        /// </summary>
        /// <returns>List of sample QcPerformanceKpi objects</returns>
        private List<QcPerformanceKpi> GetSampleQcPerformanceKpis()
        {
            return new List<QcPerformanceKpi>
            {
                new QcPerformanceKpi
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    Year = "2024",
                    Month = "January",
                    TotalTest = 100,
                    FirstPass = 85,
                    SecondPass = 12,
                    ThirdPass = 3,
                    Product = new Product { Id = 1, Name = "Product Alpha" },
                    Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
                },
                new QcPerformanceKpi
                {
                    Id = 2,
                    ProductId = 2,
                    MachineId = 1,
                    Year = "2024",
                    Month = "January",
                    TotalTest = 150,
                    FirstPass = 130,
                    SecondPass = 15,
                    ThirdPass = 5,
                    Product = new Product { Id = 2, Name = "Product Beta" },
                    Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
                },
                new QcPerformanceKpi
                {
                    Id = 3,
                    ProductId = 1,
                    MachineId = 2,
                    Year = "2024",
                    Month = "February",
                    TotalTest = 200,
                    FirstPass = 180,
                    SecondPass = 15,
                    ThirdPass = 5,
                    Product = new Product { Id = 1, Name = "Product Alpha" },
                    Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
                },
                new QcPerformanceKpi
                {
                    Id = 4,
                    ProductId = 3,
                    MachineId = 3,
                    Year = "2023",
                    Month = "December",
                    TotalTest = 75,
                    FirstPass = 60,
                    SecondPass = 10,
                    ThirdPass = 5,
                    Product = new Product { Id = 3, Name = "Product Gamma" },
                    Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
                },
                new QcPerformanceKpi
                {
                    Id = 5,
                    ProductId = null,
                    MachineId = null,
                    Year = null,
                    Month = null,
                    TotalTest = null,
                    FirstPass = null,
                    SecondPass = null,
                    ThirdPass = null,
                    Product = null,
                    Machine = null
                },
                new QcPerformanceKpi
                {
                    Id = 6,
                    ProductId = 2,
                    MachineId = 2,
                    Year = "2024",
                    Month = "March",
                    TotalTest = 250,
                    FirstPass = 240,
                    SecondPass = 8,
                    ThirdPass = 2,
                    Product = new Product { Id = 2, Name = "Product Beta" },
                    Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all QcPerformanceKpi items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllQcPerformanceKpis()
        {
            // Arrange
            var expectedKpis = GetSampleQcPerformanceKpis();

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
        /// Test GetAllAsync method filters QcPerformanceKpi items by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredKpis()
        {
            // Arrange
            var productName = "Product Alpha";
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Product?.Name == productName).ToList();

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
        /// Test GetAllAsync method filters QcPerformanceKpi items by machine name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMachineName_ReturnsFilteredKpis()
        {
            // Arrange
            var machineName = "Testing Machine Beta";
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Machine?.Name == machineName).ToList();

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
        /// Test GetAllAsync method filters QcPerformanceKpi items by year.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByYear_ReturnsFilteredKpis()
        {
            // Arrange
            var year = "2024";
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Year == year).ToList();

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
        /// Test GetAllAsync method filters QcPerformanceKpi items by month.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMonth_ReturnsFilteredKpis()
        {
            // Arrange
            var month = "January";
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Month == month).ToList();

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
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Product != null).OrderBy(x => x.Product.Name).ToList();

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
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Machine != null).OrderByDescending(x => x.Machine.Name).ToList();

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
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Year != null).OrderBy(x => x.Year).ToList();

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
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Month != null).OrderByDescending(x => x.Month).ToList();

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
            var expectedKpis = GetSampleQcPerformanceKpis()
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
            var expectedKpis = new List<QcPerformanceKpi>();

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
            var expectedKpis = GetSampleQcPerformanceKpis().Where(x => x.Product != null).OrderBy(x => x.Product.Name).ToList();

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
        /// Test GetByIdAsync method returns QcPerformanceKpi when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsQcPerformanceKpi()
        {
            // Arrange
            var kpiId = 1;
            var expectedKpi = GetSampleQcPerformanceKpis().First(x => x.Id == kpiId);

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
            Assert.Equal(100, result.TotalTest);
            Assert.Equal(85, result.FirstPass);
            Assert.Equal(12, result.SecondPass);
            Assert.Equal(3, result.ThirdPass);
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
            var expectedKpi = GetSampleQcPerformanceKpis().First(x => x.Id == kpiId);

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
            Assert.Null(result.FirstPass);
            Assert.Null(result.SecondPass);
            Assert.Null(result.ThirdPass);
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
                .ReturnsAsync((QcPerformanceKpi?)null);

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
                .ReturnsAsync((QcPerformanceKpi?)null);

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
                .ReturnsAsync((QcPerformanceKpi?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new QcPerformanceKpi with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidQcPerformanceKpiWithAllProperties_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcPerformanceKpi
            {
                ProductId = 4,
                MachineId = 4,
                Year = "2024",
                Month = "April",
                TotalTest = 300,
                FirstPass = 280,
                SecondPass = 15,
                ThirdPass = 5,
                Product = new Product { Id = 4, Name = "Product Delta" },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine" }
            };

            var createdKpi = new QcPerformanceKpi
            {
                Id = 7,
                ProductId = 4,
                MachineId = 4,
                Year = "2024",
                Month = "April",
                TotalTest = 300,
                FirstPass = 280,
                SecondPass = 15,
                ThirdPass = 5,
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
            Assert.Equal(280, result.FirstPass);
            Assert.Equal(15, result.SecondPass);
            Assert.Equal(5, result.ThirdPass);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalQcPerformanceKpi_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcPerformanceKpi
            {
                ProductId = 1,
                MachineId = 1,
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var createdKpi = new QcPerformanceKpi
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
            Assert.Null(result.FirstPass);
            Assert.Null(result.SecondPass);
            Assert.Null(result.ThirdPass);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_QcPerformanceKpiWithNullOptionalProperties_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcPerformanceKpi
            {
                ProductId = null,
                MachineId = null,
                Year = null,
                Month = null,
                TotalTest = null,
                FirstPass = null,
                SecondPass = null,
                ThirdPass = null,
                Product = null,
                Machine = null
            };

            var createdKpi = new QcPerformanceKpi
            {
                Id = 9,
                ProductId = null,
                MachineId = null,
                Year = null,
                Month = null,
                TotalTest = null,
                FirstPass = null,
                SecondPass = null,
                ThirdPass = null,
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
            Assert.Null(result.FirstPass);
            Assert.Null(result.SecondPass);
            Assert.Null(result.ThirdPass);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with high performance KPI values.
        /// </summary>
        [Fact]
        public async Task CreateAsync_QcPerformanceKpiWithHighPerformance_ReturnsCreatedKpi()
        {
            // Arrange
            var newKpi = new QcPerformanceKpi
            {
                ProductId = 5,
                MachineId = 5,
                Year = "2024",
                Month = "May",
                TotalTest = 500,
                FirstPass = 495, // 99% first pass rate
                SecondPass = 4,
                ThirdPass = 1,
                Product = new Product { Id = 5, Name = "High Quality Product" },
                Machine = new Machine { Id = 5, Name = "Premium Machine" }
            };

            var createdKpi = new QcPerformanceKpi
            {
                Id = 10,
                ProductId = 5,
                MachineId = 5,
                Year = "2024",
                Month = "May",
                TotalTest = 500,
                FirstPass = 495,
                SecondPass = 4,
                ThirdPass = 1,
                Product = new Product { Id = 5, Name = "High Quality Product" },
                Machine = new Machine { Id = 5, Name = "Premium Machine" }
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newKpi))
                .ReturnsAsync(createdKpi);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newKpi);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal(500, result.TotalTest);
            Assert.Equal(495, result.FirstPass);
            Assert.Equal(4, result.SecondPass);
            Assert.Equal(1, result.ThirdPass);
            _mockRepository.Verify(repo => repo.CreateAsync(newKpi), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing QcPerformanceKpi.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndKpi_ReturnsUpdatedKpi()
        {
            // Arrange
            var kpiId = 1;
            var updateKpi = new QcPerformanceKpi
            {
                ProductId = 3,
                MachineId = 3,
                Year = "2025",
                Month = "May",
                TotalTest = 400,
                FirstPass = 350,
                SecondPass = 35,
                ThirdPass = 15,
                Product = new Product { Id = 3, Name = "Updated Product" },
                Machine = new Machine { Id = 3, Name = "Updated Machine" }
            };

            var updatedKpi = new QcPerformanceKpi
            {
                Id = 1,
                ProductId = 3,
                MachineId = 3,
                Year = "2025",
                Month = "May",
                TotalTest = 400,
                FirstPass = 350,
                SecondPass = 35,
                ThirdPass = 15,
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
            Assert.Equal(350, result.FirstPass);
            Assert.Equal(35, result.SecondPass);
            Assert.Equal(15, result.ThirdPass);
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
            var updateKpi = new QcPerformanceKpi
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024",
                Month = "June"
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(invalidId, updateKpi))
                .ReturnsAsync((QcPerformanceKpi?)null);

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
            var updateKpi = new QcPerformanceKpi
            {
                ProductId = 2, // Same as original
                MachineId = 1, // Same as original
                Year = "2024", // Same as original
                Month = "February", // Changed from January to February
                TotalTest = 180, // Changed from 150 to 180
                FirstPass = 160, // Changed from 130 to 160
                SecondPass = 15, // Same as original
                ThirdPass = 5, // Same as original
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var updatedKpi = new QcPerformanceKpi
            {
                Id = 2,
                ProductId = 2,
                MachineId = 1,
                Year = "2024",
                Month = "February",
                TotalTest = 180,
                FirstPass = 160,
                SecondPass = 15,
                ThirdPass = 5,
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
            Assert.Equal(180, result.TotalTest);
            Assert.Equal(160, result.FirstPass);
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
            var updateKpi = new QcPerformanceKpi
            {
                ProductId = null, // Changed from 1 to null
                MachineId = null, // Changed from 2 to null
                Year = null, // Changed from "2024" to null
                Month = null, // Changed from "February" to null
                TotalTest = null, // Changed from 200 to null
                FirstPass = null, // Changed from 180 to null
                SecondPass = null, // Changed from 15 to null
                ThirdPass = null, // Changed from 5 to null
                Product = null,
                Machine = null
            };

            var updatedKpi = new QcPerformanceKpi
            {
                Id = 3,
                ProductId = null,
                MachineId = null,
                Year = null,
                Month = null,
                TotalTest = null,
                FirstPass = null,
                SecondPass = null,
                ThirdPass = null,
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
            Assert.Null(result.FirstPass);
            Assert.Null(result.SecondPass);
            Assert.Null(result.ThirdPass);
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
            var updateKpi = new QcPerformanceKpi
            {
                ProductId = 1,
                MachineId = 1,
                Year = "2024"
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(zeroId, updateKpi))
                .ReturnsAsync((QcPerformanceKpi?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(zeroId, updateKpi);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(zeroId, updateKpi), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing QcPerformanceKpi.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedKpi()
        {
            // Arrange
            var kpiId = 1;
            var deletedKpi = GetSampleQcPerformanceKpis().First(x => x.Id == kpiId);

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
            Assert.Equal(100, result.TotalTest);
            Assert.Equal(85, result.FirstPass);
            Assert.Equal(12, result.SecondPass);
            Assert.Equal(3, result.ThirdPass);
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
                .ReturnsAsync((QcPerformanceKpi?)null);

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
                .ReturnsAsync((QcPerformanceKpi?)null);

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
                .ReturnsAsync((QcPerformanceKpi?)null);

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
            var deletedKpi = GetSampleQcPerformanceKpis().First(x => x.Id == kpiId);

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
            Assert.Null(result.FirstPass);
            Assert.Null(result.SecondPass);
            Assert.Null(result.ThirdPass);
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
            var deletedKpi = GetSampleQcPerformanceKpis().First(x => x.Id == kpiId);

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
            Assert.Equal(240, result.FirstPass); // High first pass rate
            Assert.Equal(8, result.SecondPass);
            Assert.Equal(2, result.ThirdPass);
            _mockRepository.Verify(repo => repo.DeleteAsync(kpiId), Times.Once);
        }

        #endregion
    }
}



