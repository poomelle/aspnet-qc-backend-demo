using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.TestResultReportRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class TestResultReportRepositoryTests
    {
        private readonly Mock<ITestResultReportRepository> _testResultReportRepositoryMock;

        public TestResultReportRepositoryTests()
        {
            _testResultReportRepositoryMock = new Mock<ITestResultReportRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample TestResultReport data for testing purposes.
        /// </summary>
        /// <returns>List of sample TestResultReport objects</returns>
        private List<TestResultReport> GetSampleTestResultReports()
        {
            return new List<TestResultReport>
            {
                new TestResultReport
                {
                    Id = 1,
                    ReportId = 1,
                    BatchTestResultId = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 2.5,
                    FusionDiff = 1.8,
                    Result = true,
                    Comment = "Test passed within acceptable limits",
                    AveTestTime = 3600000, // 1 hour in milliseconds
                    FileLocation = "/reports/test_001.pdf",
                    Report = new Report
                    {
                        Id = 1,
                        CreateBy = "john.doe@company.com",
                        CreateDate = new DateTime(2024, 1, 15, 10, 30, 0),
                        Status = true
                    },
                    BatchTestResult = new BatchTestResult
                    {
                        Id = 1,
                        BatchId = 1,
                        TestResultId = 1,
                        Batch = new Batch
                        {
                            Id = 1,
                            BatchName = "BATCH001",
                            ProductId = 1,
                            Product = new Product { Id = 1, Name = "Product Alpha" }
                        },
                        TestResult = new TestResult
                        {
                            Id = 1,
                            TestDate = new DateTime(2024, 1, 15),
                            Product = new Product { Id = 1, Name = "Product Alpha" }
                        }
                    }
                },
                new TestResultReport
                {
                    Id = 2,
                    ReportId = 1,
                    BatchTestResultId = 2,
                    StandardReference = "STD-002",
                    TorqueDiff = 5.2,
                    FusionDiff = 3.1,
                    Result = false,
                    Comment = "Test failed - exceeds tolerance limits",
                    AveTestTime = 2700000, // 45 minutes in milliseconds
                    FileLocation = "/reports/test_002.pdf",
                    Report = new Report
                    {
                        Id = 1,
                        CreateBy = "john.doe@company.com",
                        CreateDate = new DateTime(2024, 1, 15, 10, 30, 0),
                        Status = true
                    },
                    BatchTestResult = new BatchTestResult
                    {
                        Id = 2,
                        BatchId = 2,
                        TestResultId = 2,
                        Batch = new Batch
                        {
                            Id = 2,
                            BatchName = "BATCH002",
                            ProductId = 2,
                            Product = new Product { Id = 2, Name = "Product Beta" }
                        },
                        TestResult = new TestResult
                        {
                            Id = 2,
                            TestDate = new DateTime(2024, 1, 16),
                            Product = new Product { Id = 2, Name = "Product Beta" }
                        }
                    }
                },
                new TestResultReport
                {
                    Id = 3,
                    ReportId = 2,
                    BatchTestResultId = 3,
                    StandardReference = "STD-003",
                    TorqueDiff = 1.2,
                    FusionDiff = 0.8,
                    Result = true,
                    Comment = "Excellent test results",
                    AveTestTime = 4200000, // 70 minutes in milliseconds
                    FileLocation = "/reports/test_003.pdf",
                    Report = new Report
                    {
                        Id = 2,
                        CreateBy = "jane.smith@company.com",
                        CreateDate = new DateTime(2024, 2, 10, 14, 0, 0),
                        Status = true
                    },
                    BatchTestResult = new BatchTestResult
                    {
                        Id = 3,
                        BatchId = 3,
                        TestResultId = 3,
                        Batch = new Batch
                        {
                            Id = 3,
                            BatchName = "BATCH001", // Same batch name as first for testing exact match
                            ProductId = 1,
                            Product = new Product { Id = 1, Name = "Product Alpha" }
                        },
                        TestResult = new TestResult
                        {
                            Id = 3,
                            TestDate = new DateTime(2024, 2, 10),
                            Product = new Product { Id = 1, Name = "Product Alpha" }
                        }
                    }
                },
                new TestResultReport
                {
                    Id = 4,
                    ReportId = null,
                    BatchTestResultId = null,
                    StandardReference = null,
                    TorqueDiff = null,
                    FusionDiff = null,
                    Result = null,
                    Comment = null,
                    AveTestTime = null,
                    FileLocation = null,
                    Report = null,
                    BatchTestResult = null
                },
                new TestResultReport
                {
                    Id = 5,
                    ReportId = 3,
                    BatchTestResultId = 4,
                    StandardReference = "STD-004",
                    TorqueDiff = 3.8,
                    FusionDiff = 2.3,
                    Result = false,
                    Comment = "Marginal failure - requires retest",
                    AveTestTime = 5400000, // 90 minutes in milliseconds
                    FileLocation = "/reports/test_004.pdf",
                    Report = new Report
                    {
                        Id = 3,
                        CreateBy = "mike.johnson@company.com",
                        CreateDate = new DateTime(2024, 3, 5, 9, 15, 0),
                        Status = false
                    },
                    BatchTestResult = new BatchTestResult
                    {
                        Id = 4,
                        BatchId = 4,
                        TestResultId = 4,
                        Batch = new Batch
                        {
                            Id = 4,
                            BatchName = "BATCH003",
                            ProductId = 3,
                            Product = new Product { Id = 3, Name = "Product Gamma" }
                        },
                        TestResult = new TestResult
                        {
                            Id = 4,
                            TestDate = new DateTime(2024, 3, 5),
                            Product = new Product { Id = 3, Name = "Product Gamma" }
                        }
                    }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all TestResultReport items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllTestResultReports()
        {
            // Arrange
            var expectedReports = GetSampleTestResultReports();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedReports, result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by createBy.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByCreateBy_ReturnsFilteredReports()
        {
            // Arrange
            var createBy = "john.doe";
            var expectedReports = GetSampleTestResultReports().Where(x => x.Report?.CreateBy?.Contains(createBy) == true).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(createBy);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Contains(createBy, report.Report?.CreateBy));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by createDate.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByCreateDate_ReturnsFilteredReports()
        {
            // Arrange
            var createDate = "2024-01-15";
            var expectedReports = GetSampleTestResultReports().Where(x => x.Report?.CreateDate.Date == DateTime.Parse(createDate).Date).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, createDate, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, createDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Equal(DateTime.Parse(createDate).Date, report.Report?.CreateDate.Date));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, createDate, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by productName.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredReports()
        {
            // Arrange
            var productName = "Product Alpha";
            var expectedReports = GetSampleTestResultReports().Where(x => x.BatchTestResult?.TestResult?.Product?.Name?.Contains(productName) == true).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, productName, null, null, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Contains(productName, report.BatchTestResult?.TestResult?.Product?.Name));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, productName, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by batchName (contains).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchName_ReturnsFilteredReports()
        {
            // Arrange
            var batchName = "BATCH00";
            var expectedReports = GetSampleTestResultReports().Where(x => x.BatchTestResult?.Batch?.BatchName?.Contains(batchName) == true).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, batchName, null, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.All(result, report => Assert.Contains(batchName, report.BatchTestResult?.Batch?.BatchName));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, batchName, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by result (true).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByResultTrue_ReturnsPassedReports()
        {
            // Arrange
            var result = "true";
            var expectedReports = GetSampleTestResultReports().Where(x => x.Result == true).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, result, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var resultList = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, result);

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(2, resultList.Count);
            Assert.All(resultList, report => Assert.True(report.Result));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, result, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by result (false).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByResultFalse_ReturnsFailedReports()
        {
            // Arrange
            var result = "false";
            var expectedReports = GetSampleTestResultReports().Where(x => x.Result == false).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, result, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var resultList = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, result);

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(2, resultList.Count);
            Assert.All(resultList, report => Assert.False(report.Result));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, result, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by batchTestResultId.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchTestResultId_ReturnsFilteredReports()
        {
            // Arrange
            var batchTestResultId = "1";
            var expectedReports = GetSampleTestResultReports().Where(x => x.BatchTestResultId == int.Parse(batchTestResultId)).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, batchTestResultId, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, null, batchTestResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(int.Parse(batchTestResultId), result.First().BatchTestResultId);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, batchTestResultId, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters TestResultReport items by exactBatchName.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByExactBatchName_ReturnsFilteredReports()
        {
            // Arrange
            var exactBatchName = "BATCH001";
            var expectedReports = GetSampleTestResultReports().Where(x => string.Equals(x.BatchTestResult?.Batch?.BatchName, exactBatchName, StringComparison.OrdinalIgnoreCase)).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, exactBatchName, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, exactBatchName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Equal(exactBatchName, report.BatchTestResult?.Batch?.BatchName, StringComparer.OrdinalIgnoreCase));
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, exactBatchName, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by createDate ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByCreateDateAscending_ReturnsSortedReports()
        {
            // Arrange
            var sortBy = "createDate";
            var expectedReports = GetSampleTestResultReports().Where(x => x.Report != null).OrderBy(x => x.Report.CreateDate).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedReports, result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by productName descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameDescending_ReturnsSortedReports()
        {
            // Arrange
            var sortBy = "productName";
            var expectedReports = GetSampleTestResultReports().Where(x => x.BatchTestResult?.TestResult?.Product != null).OrderByDescending(x => x.BatchTestResult.TestResult.Product.Name).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, false))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedReports, result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by batchName ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByBatchNameAscending_ReturnsSortedReports()
        {
            // Arrange
            var sortBy = "batchName";
            var expectedReports = GetSampleTestResultReports().Where(x => x.BatchTestResult?.Batch != null).OrderBy(x => x.BatchTestResult.Batch.BatchName).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, null, null, null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedReports, result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredReports()
        {
            // Arrange
            var createBy = "john.doe";
            var productName = "Product Alpha";
            var result = "true";
            var sortBy = "createDate";
            var expectedReports = GetSampleTestResultReports()
                .Where(x => x.Report?.CreateBy?.Contains(createBy) == true &&
                           x.BatchTestResult?.TestResult?.Product?.Name?.Contains(productName) == true &&
                           x.Result == true)
                .OrderBy(x => x.Report?.CreateDate).ToList();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, null, productName, null, result, null, null, sortBy, true))
                .ReturnsAsync(expectedReports);

            // Act
            var resultList = await _testResultReportRepositoryMock.Object.GetAllAsync(createBy, null, productName, null, result, null, null, sortBy, true);

            // Assert
            Assert.NotNull(resultList);
            Assert.Single(resultList);
            Assert.Contains(createBy, resultList.First().Report?.CreateBy);
            Assert.Contains(productName, resultList.First().BatchTestResult?.TestResult?.Product?.Name);
            Assert.True(resultList.First().Result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, null, productName, null, result, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no TestResultReport items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingReports_ReturnsEmptyList()
        {
            // Arrange
            var createBy = "nonexistent.user";
            var expectedReports = new List<TestResultReport>();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(createBy);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid filters returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidFilters_ReturnsEmptyList()
        {
            // Arrange
            var invalidDate = "invalid-date";
            var invalidBatchTestResultId = "invalid-id";
            var invalidResult = "invalid-result";
            var expectedReports = new List<TestResultReport>();

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, invalidDate, null, null, invalidResult, invalidBatchTestResultId, null, null, true))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetAllAsync(null, invalidDate, null, null, invalidResult, invalidBatchTestResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(null, invalidDate, null, null, invalidResult, invalidBatchTestResultId, null, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns TestResultReport when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsTestResultReport()
        {
            // Arrange
            var reportId = 1;
            var expectedReport = GetSampleTestResultReports().First(x => x.Id == reportId);

            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(reportId))
                .ReturnsAsync(expectedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetByIdAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportId, result.Id);
            Assert.Equal(1, result.ReportId);
            Assert.Equal(1, result.BatchTestResultId);
            Assert.Equal("STD-001", result.StandardReference);
            Assert.Equal(2.5, result.TorqueDiff);
            Assert.Equal(1.8, result.FusionDiff);
            Assert.True(result.Result);
            Assert.Equal("Test passed within acceptable limits", result.Comment);
            Assert.Equal(3600000, result.AveTestTime);
            Assert.Equal("/reports/test_001.pdf", result.FileLocation);
            _testResultReportRepositoryMock.Verify(repo => repo.GetByIdAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns TestResultReport with null properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ReportWithNullProperties_ReturnsReport()
        {
            // Arrange
            var reportId = 4;
            var expectedReport = GetSampleTestResultReports().First(x => x.Id == reportId);

            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(reportId))
                .ReturnsAsync(expectedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetByIdAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportId, result.Id);
            Assert.Null(result.ReportId);
            Assert.Null(result.BatchTestResultId);
            Assert.Null(result.StandardReference);
            Assert.Null(result.TorqueDiff);
            Assert.Null(result.FusionDiff);
            Assert.Null(result.Result);
            Assert.Null(result.Comment);
            Assert.Null(result.AveTestTime);
            Assert.Null(result.FileLocation);
            _testResultReportRepositoryMock.Verify(repo => repo.GetByIdAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new TestResultReport with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidTestResultReportWithAllProperties_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new TestResultReport
            {
                ReportId = 4,
                BatchTestResultId = 5,
                StandardReference = "STD-005",
                TorqueDiff = 4.2,
                FusionDiff = 2.7,
                Result = true,
                Comment = "New test report with comprehensive data",
                AveTestTime = 7200000, // 2 hours in milliseconds
                FileLocation = "/reports/test_new.pdf",
                Report = new Report { Id = 4, CreateBy = "new.user@company.com" },
                BatchTestResult = new BatchTestResult { Id = 5, BatchId = 5 }
            };

            var createdReport = new TestResultReport
            {
                Id = 6,
                ReportId = 4,
                BatchTestResultId = 5,
                StandardReference = "STD-005",
                TorqueDiff = 4.2,
                FusionDiff = 2.7,
                Result = true,
                Comment = "New test report with comprehensive data",
                AveTestTime = 7200000,
                FileLocation = "/reports/test_new.pdf",
                Report = new Report { Id = 4, CreateBy = "new.user@company.com" },
                BatchTestResult = new BatchTestResult { Id = 5, BatchId = 5 }
            };

            _testResultReportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(4, result.ReportId);
            Assert.Equal(5, result.BatchTestResultId);
            Assert.Equal("STD-005", result.StandardReference);
            Assert.Equal(4.2, result.TorqueDiff);
            Assert.Equal(2.7, result.FusionDiff);
            Assert.True(result.Result);
            Assert.Equal("New test report with comprehensive data", result.Comment);
            Assert.Equal(7200000, result.AveTestTime);
            Assert.Equal("/reports/test_new.pdf", result.FileLocation);
            _testResultReportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalTestResultReport_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new TestResultReport
            {
                ReportId = 1,
                BatchTestResultId = 1
            };

            var createdReport = new TestResultReport
            {
                Id = 7,
                ReportId = 1,
                BatchTestResultId = 1
            };

            _testResultReportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(1, result.ReportId);
            Assert.Equal(1, result.BatchTestResultId);
            Assert.Null(result.StandardReference);
            Assert.Null(result.TorqueDiff);
            Assert.Null(result.FusionDiff);
            Assert.Null(result.Result);
            Assert.Null(result.Comment);
            Assert.Null(result.AveTestTime);
            Assert.Null(result.FileLocation);
            _testResultReportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_TestResultReportWithNullOptionalProperties_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new TestResultReport
            {
                ReportId = null,
                BatchTestResultId = null,
                StandardReference = null,
                TorqueDiff = null,
                FusionDiff = null,
                Result = null,
                Comment = null,
                AveTestTime = null,
                FileLocation = null,
                Report = null,
                BatchTestResult = null
            };

            var createdReport = new TestResultReport
            {
                Id = 8,
                ReportId = null,
                BatchTestResultId = null,
                StandardReference = null,
                TorqueDiff = null,
                FusionDiff = null,
                Result = null,
                Comment = null,
                AveTestTime = null,
                FileLocation = null,
                Report = null,
                BatchTestResult = null
            };

            _testResultReportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Null(result.ReportId);
            Assert.Null(result.BatchTestResultId);
            Assert.Null(result.StandardReference);
            Assert.Null(result.TorqueDiff);
            Assert.Null(result.FusionDiff);
            Assert.Null(result.Result);
            Assert.Null(result.Comment);
            Assert.Null(result.AveTestTime);
            Assert.Null(result.FileLocation);
            _testResultReportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with failed test result.
        /// </summary>
        [Fact]
        public async Task CreateAsync_FailedTestResultReport_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new TestResultReport
            {
                ReportId = 2,
                BatchTestResultId = 3,
                StandardReference = "STD-FAIL",
                TorqueDiff = 10.5,
                FusionDiff = 8.2,
                Result = false,
                Comment = "Test failed - significant deviations detected",
                AveTestTime = 1800000, // 30 minutes in milliseconds
                FileLocation = "/reports/test_failed.pdf"
            };

            var createdReport = new TestResultReport
            {
                Id = 9,
                ReportId = 2,
                BatchTestResultId = 3,
                StandardReference = "STD-FAIL",
                TorqueDiff = 10.5,
                FusionDiff = 8.2,
                Result = false,
                Comment = "Test failed - significant deviations detected",
                AveTestTime = 1800000,
                FileLocation = "/reports/test_failed.pdf"
            };

            _testResultReportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.False(result.Result);
            Assert.Equal(10.5, result.TorqueDiff);
            Assert.Equal(8.2, result.FusionDiff);
            Assert.Contains("failed", result.Comment);
            _testResultReportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing TestResultReport.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndReport_ReturnsUpdatedReport()
        {
            // Arrange
            var reportId = 1;
            var updateReport = new TestResultReport
            {
                ReportId = 5,
                BatchTestResultId = 6,
                StandardReference = "STD-UPDATED",
                TorqueDiff = 7.8,
                FusionDiff = 4.9,
                Result = false,
                Comment = "Updated test report with new analysis",
                AveTestTime = 9000000, // 2.5 hours in milliseconds
                FileLocation = "/reports/test_updated.pdf"
            };

            var updatedReport = new TestResultReport
            {
                Id = 1,
                ReportId = 5,
                BatchTestResultId = 6,
                StandardReference = "STD-UPDATED",
                TorqueDiff = 7.8,
                FusionDiff = 4.9,
                Result = false,
                Comment = "Updated test report with new analysis",
                AveTestTime = 9000000,
                FileLocation = "/reports/test_updated.pdf"
            };

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(reportId, updateReport))
                .ReturnsAsync(updatedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.UpdateAsync(reportId, updateReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(5, result.ReportId);
            Assert.Equal(6, result.BatchTestResultId);
            Assert.Equal("STD-UPDATED", result.StandardReference);
            Assert.Equal(7.8, result.TorqueDiff);
            Assert.Equal(4.9, result.FusionDiff);
            Assert.False(result.Result);
            Assert.Equal("Updated test report with new analysis", result.Comment);
            _testResultReportRepositoryMock.Verify(repo => repo.UpdateAsync(reportId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when TestResultReport with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateReport = new TestResultReport
            {
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-UPDATE"
            };

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateReport))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.UpdateAsync(invalidId, updateReport);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedReport()
        {
            // Arrange
            var reportId = 2;
            var updateReport = new TestResultReport
            {
                ReportId = 1, // Same as original
                BatchTestResultId = 2, // Same as original
                StandardReference = "STD-002-UPDATED", // Changed
                TorqueDiff = 5.2, // Same as original
                FusionDiff = 3.1, // Same as original
                Result = true, // Changed from false to true
                Comment = "Test result updated after review" // Changed
            };

            var updatedReport = new TestResultReport
            {
                Id = 2,
                ReportId = 1,
                BatchTestResultId = 2,
                StandardReference = "STD-002-UPDATED",
                TorqueDiff = 5.2,
                FusionDiff = 3.1,
                Result = true,
                Comment = "Test result updated after review"
            };

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(reportId, updateReport))
                .ReturnsAsync(updatedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.UpdateAsync(reportId, updateReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("STD-002-UPDATED", result.StandardReference);
            Assert.True(result.Result);
            Assert.Equal("Test result updated after review", result.Comment);
            _testResultReportRepositoryMock.Verify(repo => repo.UpdateAsync(reportId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedReport()
        {
            // Arrange
            var reportId = 3;
            var updateReport = new TestResultReport
            {
                ReportId = null, // Changed from 2 to null
                BatchTestResultId = null, // Changed from 3 to null
                StandardReference = null, // Changed from "STD-003" to null
                TorqueDiff = null, // Changed from 1.2 to null
                FusionDiff = null, // Changed from 0.8 to null
                Result = null // Changed from true to null
            };

            var updatedReport = new TestResultReport
            {
                Id = 3,
                ReportId = null,
                BatchTestResultId = null,
                StandardReference = null,
                TorqueDiff = null,
                FusionDiff = null,
                Result = null
            };

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(reportId, updateReport))
                .ReturnsAsync(updatedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.UpdateAsync(reportId, updateReport);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ReportId);
            Assert.Null(result.BatchTestResultId);
            Assert.Null(result.StandardReference);
            Assert.Null(result.TorqueDiff);
            Assert.Null(result.FusionDiff);
            Assert.Null(result.Result);
            _testResultReportRepositoryMock.Verify(repo => repo.UpdateAsync(reportId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateReport = new TestResultReport
            {
                ReportId = 1,
                BatchTestResultId = 1
            };

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateReport))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.UpdateAsync(zeroId, updateReport);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateReport), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing TestResultReport.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedReport()
        {
            // Arrange
            var reportId = 1;
            var deletedReport = GetSampleTestResultReports().First(x => x.Id == reportId);

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(reportId))
                .ReturnsAsync(deletedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.DeleteAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.ReportId);
            Assert.Equal(1, result.BatchTestResultId);
            Assert.Equal("STD-001", result.StandardReference);
            Assert.Equal(2.5, result.TorqueDiff);
            Assert.Equal(1.8, result.FusionDiff);
            Assert.True(result.Result);
            _testResultReportRepositoryMock.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when TestResultReport with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _testResultReportRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _testResultReportRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with TestResultReport containing null properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ReportWithNullProperties_ReturnsDeletedReport()
        {
            // Arrange
            var reportId = 4;
            var deletedReport = GetSampleTestResultReports().First(x => x.Id == reportId);

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(reportId))
                .ReturnsAsync(deletedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.DeleteAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Null(result.ReportId);
            Assert.Null(result.BatchTestResultId);
            Assert.Null(result.StandardReference);
            Assert.Null(result.TorqueDiff);
            Assert.Null(result.FusionDiff);
            Assert.Null(result.Result);
            _testResultReportRepositoryMock.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with failed test result report.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_FailedTestResultReport_ReturnsDeletedReport()
        {
            // Arrange
            var reportId = 5;
            var deletedReport = GetSampleTestResultReports().First(x => x.Id == reportId);

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(reportId))
                .ReturnsAsync(deletedReport);

            // Act
            var result = await _testResultReportRepositoryMock.Object.DeleteAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(3, result.ReportId);
            Assert.Equal(4, result.BatchTestResultId);
            Assert.Equal("STD-004", result.StandardReference);
            Assert.Equal(3.8, result.TorqueDiff);
            Assert.Equal(2.3, result.FusionDiff);
            Assert.False(result.Result);
            Assert.Equal("Marginal failure - requires retest", result.Comment);
            _testResultReportRepositoryMock.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        #endregion
    }
}

