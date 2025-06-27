using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.DailyQcRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class DailyQcRepositoryTests
    {
        private readonly Mock<IDailyQcRepository> _dailyQcRepositoryMock;

        public DailyQcRepositoryTests()
        {
            _dailyQcRepositoryMock = new Mock<IDailyQcRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample DailyQc data for testing purposes.
        /// </summary>
        /// <returns>List of sample DailyQc objects</returns>
        private List<DailyQc> GetSampleDailyQcs()
        {
            return new List<DailyQc>
            {
                new DailyQc
                {
                    Id = 1,
                    IncomingDate = new DateTime(2024, 1, 15),
                    ProductId = 1,
                    Priority = 1,
                    Comment = "High priority test",
                    Batches = "B001,B002",
                    StdReqd = "Standard A",
                    Extras = "Extra tests required",
                    MixesReqd = 3,
                    Mixed = 3,
                    TestStatus = "Completed",
                    LastLabel = "LBL001",
                    LastBatch = "B002",
                    TestedDate = new DateTime(2024, 1, 16),
                    Year = "2024",
                    Month = "January",
                    Product = new Product { Id = 1, Name = "Product A" }
                },
                new DailyQc
                {
                    Id = 2,
                    IncomingDate = new DateTime(2024, 2, 10),
                    ProductId = 2,
                    Priority = 2,
                    Comment = "Medium priority test",
                    Batches = "B003,B004",
                    StdReqd = "Standard B",
                    Extras = null,
                    MixesReqd = 2,
                    Mixed = 1,
                    TestStatus = "In Progress",
                    LastLabel = "LBL002",
                    LastBatch = "B004",
                    TestedDate = null,
                    Year = "2024",
                    Month = "February",
                    Product = new Product { Id = 2, Name = "Product B" }
                },
                new DailyQc
                {
                    Id = 3,
                    IncomingDate = new DateTime(2024, 3, 5),
                    ProductId = 1,
                    Priority = 3,
                    Comment = "Low priority test",
                    Batches = "B005",
                    StdReqd = "Standard A",
                    Extras = "Quick test",
                    MixesReqd = 1,
                    Mixed = 0,
                    TestStatus = "Pending",
                    LastLabel = "LBL003",
                    LastBatch = "B005",
                    TestedDate = null,
                    Year = "2024",
                    Month = "March",
                    Product = new Product { Id = 1, Name = "Product A" }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all DailyQc items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllDailyQcs()
        {
            // Arrange
            var expectedDailyQcs = GetSampleDailyQcs();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedDailyQcs, result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by ID.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterById_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var id = "1";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.Id == int.Parse(id)).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(id, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(int.Parse(id), result.First().Id);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(id, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var productName = "Product A";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.Product.Name == productName).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, productName, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, dailyQc => Assert.Equal(productName, dailyQc.Product.Name));
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, productName, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by incoming date.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByIncomingDate_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var incomingDate = "2024-01-15";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.IncomingDate.Date == DateTime.Parse(incomingDate).Date).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, incomingDate, null, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, incomingDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(DateTime.Parse(incomingDate).Date, result.First().IncomingDate.Date);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, incomingDate, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by tested date.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestedDate_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var testedDate = "2024-01-16";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.TestedDate?.Date == DateTime.Parse(testedDate).Date).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, testedDate, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, testedDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(DateTime.Parse(testedDate).Date, result.First().TestedDate?.Date);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, testedDate, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by test status.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestStatus_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var testStatus = "Completed";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.TestStatus == testStatus).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, testStatus, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, null, testStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(testStatus, result.First().TestStatus);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, testStatus, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by year.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByYear_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var year = "2024";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.Year == year).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, year, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, null, null, year);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, dailyQc => Assert.Equal(year, dailyQc.Year));
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, year, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters DailyQc items by month.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMonth_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var month = "January";
            var expectedDailyQcs = GetSampleDailyQcs().Where(x => x.Month == month).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, month, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, month);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(month, result.First().Month);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, month, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameAscending_ReturnsSortedDailyQcs()
        {
            // Arrange
            var sortBy = "productName";
            var expectedDailyQcs = GetSampleDailyQcs().OrderBy(x => x.Product.Name).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedDailyQcs, result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by incoming date descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByIncomingDateDescending_ReturnsSortedDailyQcs()
        {
            // Arrange
            var sortBy = "incomingDate";
            var expectedDailyQcs = GetSampleDailyQcs().OrderByDescending(x => x.IncomingDate).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, false))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedDailyQcs, result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by test status ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByTestStatusAscending_ReturnsSortedDailyQcs()
        {
            // Arrange
            var sortBy = "testStatus";
            var expectedDailyQcs = GetSampleDailyQcs().OrderBy(x => x.TestStatus).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedDailyQcs, result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredDailyQcs()
        {
            // Arrange
            var productName = "Product A";
            var testStatus = "Completed";
            var year = "2024";
            var sortBy = "incomingDate";
            var expectedDailyQcs = GetSampleDailyQcs()
                .Where(x => x.Product.Name == productName && x.TestStatus == testStatus && x.Year == year)
                .OrderBy(x => x.IncomingDate).ToList();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, productName, null, null, testStatus, year, null, sortBy, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, productName, null, null, testStatus, year, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(productName, result.First().Product.Name);
            Assert.Equal(testStatus, result.First().TestStatus);
            Assert.Equal(year, result.First().Year);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, productName, null, null, testStatus, year, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no DailyQc items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingDailyQcs_ReturnsEmptyList()
        {
            // Arrange
            var productName = "NonExistentProduct";
            var expectedDailyQcs = new List<DailyQc>();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(null, productName, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(null, productName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(null, productName, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid ID filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidIdFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidId = "invalid";
            var expectedDailyQcs = new List<DailyQc>();

            _dailyQcRepositoryMock.Setup(repo => repo.GetAllAsync(invalidId, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedDailyQcs);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetAllAsync(invalidId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetAllAsync(invalidId, null, null, null, null, null, null, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns DailyQc when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsDailyQc()
        {
            // Arrange
            var dailyQcId = 1;
            var expectedDailyQc = GetSampleDailyQcs().First(x => x.Id == dailyQcId);

            _dailyQcRepositoryMock.Setup(repo => repo.GetByIdAsync(dailyQcId))
                .ReturnsAsync(expectedDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetByIdAsync(dailyQcId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyQcId, result.Id);
            Assert.Equal("Product A", result.Product.Name);
            Assert.Equal("Completed", result.TestStatus);
            _dailyQcRepositoryMock.Verify(repo => repo.GetByIdAsync(dailyQcId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _dailyQcRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _dailyQcRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _dailyQcRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new DailyQc.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidDailyQc_ReturnsCreatedDailyQc()
        {
            // Arrange
            var newDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 4, 10),
                ProductId = 1,
                Priority = 1,
                Comment = "New test item",
                Batches = "B006,B007",
                StdReqd = "Standard C",
                Extras = "Additional tests",
                MixesReqd = 2,
                Mixed = 0,
                TestStatus = "Pending",
                LastLabel = "LBL004",
                LastBatch = "B007",
                TestedDate = null,
                Year = "2024",
                Month = "April",
                Product = new Product { Id = 1, Name = "Product A" }
            };

            var createdDailyQc = new DailyQc
            {
                Id = 4,
                IncomingDate = new DateTime(2024, 4, 10),
                ProductId = 1,
                Priority = 1,
                Comment = "New test item",
                Batches = "B006,B007",
                StdReqd = "Standard C",
                Extras = "Additional tests",
                MixesReqd = 2,
                Mixed = 0,
                TestStatus = "Pending",
                LastLabel = "LBL004",
                LastBatch = "B007",
                TestedDate = null,
                Year = "2024",
                Month = "April",
                Product = new Product { Id = 1, Name = "Product A" }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.CreateAsync(newDailyQc))
                .ReturnsAsync(createdDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.CreateAsync(newDailyQc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal("New test item", result.Comment);
            Assert.Equal("Pending", result.TestStatus);
            Assert.Equal(1, result.ProductId);
            _dailyQcRepositoryMock.Verify(repo => repo.CreateAsync(newDailyQc), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalDailyQc_ReturnsCreatedDailyQc()
        {
            // Arrange
            var newDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 5, 1),
                ProductId = 2,
                Product = new Product { Id = 2, Name = "Product B" }
            };

            var createdDailyQc = new DailyQc
            {
                Id = 5,
                IncomingDate = new DateTime(2024, 5, 1),
                ProductId = 2,
                Product = new Product { Id = 2, Name = "Product B" }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.CreateAsync(newDailyQc))
                .ReturnsAsync(createdDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.CreateAsync(newDailyQc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(new DateTime(2024, 5, 1), result.IncomingDate);
            _dailyQcRepositoryMock.Verify(repo => repo.CreateAsync(newDailyQc), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all properties populated.
        /// </summary>
        [Fact]
        public async Task CreateAsync_CompletelyPopulatedDailyQc_ReturnsCreatedDailyQc()
        {
            // Arrange
            var newDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 6, 15),
                ProductId = 1,
                Priority = 1,
                Comment = "Complete test with all properties",
                Batches = "B010,B011,B012",
                StdReqd = "All standards required",
                Extras = "Comprehensive testing",
                MixesReqd = 5,
                Mixed = 3,
                TestStatus = "In Progress",
                LastLabel = "LBL010",
                LastBatch = "B012",
                TestedDate = new DateTime(2024, 6, 16),
                Year = "2024",
                Month = "June",
                Product = new Product
                {
                    Id = 1,
                    Name = "Premium Product",
                    Status = true,
                    SampleAmount = 250.75,
                    Comment = "High quality product"
                }
            };

            var createdDailyQc = new DailyQc
            {
                Id = 6,
                IncomingDate = new DateTime(2024, 6, 15),
                ProductId = 1,
                Priority = 1,
                Comment = "Complete test with all properties",
                Batches = "B010,B011,B012",
                StdReqd = "All standards required",
                Extras = "Comprehensive testing",
                MixesReqd = 5,
                Mixed = 3,
                TestStatus = "In Progress",
                LastLabel = "LBL010",
                LastBatch = "B012",
                TestedDate = new DateTime(2024, 6, 16),
                Year = "2024",
                Month = "June",
                Product = new Product
                {
                    Id = 1,
                    Name = "Premium Product",
                    Status = true,
                    SampleAmount = 250.75,
                    Comment = "High quality product"
                }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.CreateAsync(newDailyQc))
                .ReturnsAsync(createdDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.CreateAsync(newDailyQc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("Complete test with all properties", result.Comment);
            Assert.Equal(5, result.MixesReqd);
            Assert.Equal(3, result.Mixed);
            Assert.Equal("Premium Product", result.Product.Name);
            _dailyQcRepositoryMock.Verify(repo => repo.CreateAsync(newDailyQc), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing DailyQc.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndDailyQc_ReturnsUpdatedDailyQc()
        {
            // Arrange
            var dailyQcId = 1;
            var updateDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 7, 20),
                ProductId = 2,
                Priority = 2,
                Comment = "Updated test item",
                Batches = "B020,B021",
                StdReqd = "Updated Standard",
                Extras = "Updated extras",
                MixesReqd = 4,
                Mixed = 2,
                TestStatus = "Updated Status",
                LastLabel = "LBL020",
                LastBatch = "B021",
                TestedDate = new DateTime(2024, 7, 21),
                Year = "2024",
                Month = "July",
                Product = new Product { Id = 2, Name = "Updated Product" }
            };

            var updatedDailyQc = new DailyQc
            {
                Id = 1,
                IncomingDate = new DateTime(2024, 7, 20),
                ProductId = 2,
                Priority = 2,
                Comment = "Updated test item",
                Batches = "B020,B021",
                StdReqd = "Updated Standard",
                Extras = "Updated extras",
                MixesReqd = 4,
                Mixed = 2,
                TestStatus = "Updated Status",
                LastLabel = "LBL020",
                LastBatch = "B021",
                TestedDate = new DateTime(2024, 7, 21),
                Year = "2024",
                Month = "July",
                Product = new Product { Id = 2, Name = "Updated Product" }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.UpdateAsync(dailyQcId, updateDailyQc))
                .ReturnsAsync(updatedDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.UpdateAsync(dailyQcId, updateDailyQc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated test item", result.Comment);
            Assert.Equal("Updated Status", result.TestStatus);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(4, result.MixesReqd);
            _dailyQcRepositoryMock.Verify(repo => repo.UpdateAsync(dailyQcId, updateDailyQc), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when DailyQc with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 8, 1),
                ProductId = 1,
                TestStatus = "Updated",
                Product = new Product { Id = 1, Name = "Product A" }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateDailyQc))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.UpdateAsync(invalidId, updateDailyQc);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateDailyQc), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedDailyQc()
        {
            // Arrange
            var dailyQcId = 2;
            var updateDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 2, 10), // Same as original
                ProductId = 2, // Same as original
                Priority = 1, // Changed from 2 to 1
                Comment = "Partially updated comment", // Changed
                TestStatus = "Completed", // Changed from "In Progress"
                Mixed = 2, // Changed from 1 to 2
                Product = new Product { Id = 2, Name = "Product B" }
            };

            var updatedDailyQc = new DailyQc
            {
                Id = 2,
                IncomingDate = new DateTime(2024, 2, 10),
                ProductId = 2,
                Priority = 1,
                Comment = "Partially updated comment",
                TestStatus = "Completed",
                Mixed = 2,
                Product = new Product { Id = 2, Name = "Product B" }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.UpdateAsync(dailyQcId, updateDailyQc))
                .ReturnsAsync(updatedDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.UpdateAsync(dailyQcId, updateDailyQc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Partially updated comment", result.Comment);
            Assert.Equal("Completed", result.TestStatus);
            Assert.Equal(1, result.Priority);
            Assert.Equal(2, result.Mixed);
            _dailyQcRepositoryMock.Verify(repo => repo.UpdateAsync(dailyQcId, updateDailyQc), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateDailyQc = new DailyQc
            {
                IncomingDate = new DateTime(2024, 9, 1),
                ProductId = 1,
                TestStatus = "Updated",
                Product = new Product { Id = 1, Name = "Product A" }
            };

            _dailyQcRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateDailyQc))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.UpdateAsync(zeroId, updateDailyQc);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateDailyQc), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing DailyQc.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedDailyQc()
        {
            // Arrange
            var dailyQcId = 1;
            var deletedDailyQc = GetSampleDailyQcs().First(x => x.Id == dailyQcId);

            _dailyQcRepositoryMock.Setup(repo => repo.DeleteAsync(dailyQcId))
                .ReturnsAsync(deletedDailyQc);

            // Act
            var result = await _dailyQcRepositoryMock.Object.DeleteAsync(dailyQcId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("High priority test", result.Comment);
            Assert.Equal("Completed", result.TestStatus);
            _dailyQcRepositoryMock.Verify(repo => repo.DeleteAsync(dailyQcId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when DailyQc with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _dailyQcRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _dailyQcRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _dailyQcRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((DailyQc?)null);

            // Act
            var result = await _dailyQcRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _dailyQcRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion
    }
}
