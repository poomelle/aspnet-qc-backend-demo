using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.ReportRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class ReportRepositoryTests
    {
        private readonly Mock<IReportRepository> _reportRepositoryMock;

        public ReportRepositoryTests()
        {
            _reportRepositoryMock = new Mock<IReportRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample Report data for testing purposes.
        /// </summary>
        /// <returns>List of sample Report objects</returns>
        private List<Report> GetSampleReports()
        {
            return new List<Report>
            {
                new Report
                {
                    Id = 1,
                    CreateBy = "john.doe@company.com",
                    CreateDate = new DateTime(2024, 1, 15, 10, 30, 0),
                    Status = true
                },
                new Report
                {
                    Id = 2,
                    CreateBy = "jane.smith@company.com",
                    CreateDate = new DateTime(2024, 1, 15, 14, 45, 0),
                    Status = false
                },
                new Report
                {
                    Id = 3,
                    CreateBy = "mike.johnson@company.com",
                    CreateDate = new DateTime(2024, 2, 10, 9, 15, 0),
                    Status = true
                },
                new Report
                {
                    Id = 4,
                    CreateBy = "sarah.wilson@company.com",
                    CreateDate = new DateTime(2024, 2, 10, 16, 20, 0),
                    Status = false
                },
                new Report
                {
                    Id = 5,
                    CreateBy = null,
                    CreateDate = new DateTime(2024, 3, 5, 11, 0, 0),
                    Status = true
                },
                new Report
                {
                    Id = 6,
                    CreateBy = "john.doe@company.com",
                    CreateDate = new DateTime(2024, 3, 5, 13, 30, 0),
                    Status = true
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all Report items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllReports()
        {
            // Arrange
            var expectedReports = GetSampleReports();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
            Assert.Equal(expectedReports, result);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Report items by createBy (contains).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByCreateBy_ReturnsFilteredReports()
        {
            // Arrange
            var createBy = "john.doe";
            var expectedReports = GetSampleReports().Where(x => x.CreateBy?.Contains(createBy) == true).ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, null, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(createBy);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Contains(createBy, report.CreateBy));
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Report items by exact createBy match.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByExactCreateBy_ReturnsFilteredReports()
        {
            // Arrange
            var createBy = "jane.smith@company.com";
            var expectedReports = GetSampleReports().Where(x => x.CreateBy?.Contains(createBy) == true).ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, null, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(createBy);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(createBy, result.First().CreateBy);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Report items by createDate.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByCreateDate_ReturnsFilteredReports()
        {
            // Arrange
            var createDate = "2024-01-15";
            var expectedReports = GetSampleReports().Where(x => x.CreateDate.Date == DateTime.Parse(createDate).Date).ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, createDate, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(null, createDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Equal(DateTime.Parse(createDate).Date, report.CreateDate.Date));
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, createDate, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Report items by different createDate.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByDifferentCreateDate_ReturnsFilteredReports()
        {
            // Arrange
            var createDate = "2024-02-10";
            var expectedReports = GetSampleReports().Where(x => x.CreateDate.Date == DateTime.Parse(createDate).Date).ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, createDate, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(null, createDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.Equal(DateTime.Parse(createDate).Date, report.CreateDate.Date));
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, createDate, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Report items by status (true).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByActiveStatus_ReturnsActiveReports()
        {
            // Arrange
            var status = "true";
            var expectedReports = GetSampleReports().Where(x => x.Status == true).ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, status))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(null, null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.All(result, report => Assert.True(report.Status));
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, status), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Report items by status (false).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByInactiveStatus_ReturnsInactiveReports()
        {
            // Arrange
            var status = "false";
            var expectedReports = GetSampleReports().Where(x => x.Status == false).ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, status))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(null, null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, report => Assert.False(report.Status));
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, status), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredReports()
        {
            // Arrange
            var createBy = "john.doe";
            var createDate = "2024-01-15";
            var status = "true";
            var expectedReports = GetSampleReports()
                .Where(x => x.CreateBy?.Contains(createBy) == true &&
                           x.CreateDate.Date == DateTime.Parse(createDate).Date &&
                           x.Status == true)
                .ToList();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, createDate, status))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(createBy, createDate, status);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(createBy, result.First().CreateBy);
            Assert.Equal(DateTime.Parse(createDate).Date, result.First().CreateDate.Date);
            Assert.True(result.First().Status);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, createDate, status), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no Report items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingReports_ReturnsEmptyList()
        {
            // Arrange
            var createBy = "nonexistent.user@company.com";
            var expectedReports = new List<Report>();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, null, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(createBy);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid createDate filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidCreateDateFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidCreateDate = "invalid-date";
            var expectedReports = new List<Report>();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, invalidCreateDate, null))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(null, invalidCreateDate);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, invalidCreateDate, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid status filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidStatusFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidStatus = "invalid";
            var expectedReports = new List<Report>();

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, invalidStatus))
                .ReturnsAsync(expectedReports);

            // Act
            var result = await _reportRepositoryMock.Object.GetAllAsync(null, null, invalidStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, invalidStatus), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns Report when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsReport()
        {
            // Arrange
            var reportId = 1;
            var expectedReport = GetSampleReports().First(x => x.Id == reportId);

            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(reportId))
                .ReturnsAsync(expectedReport);

            // Act
            var result = await _reportRepositoryMock.Object.GetByIdAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportId, result.Id);
            Assert.Equal("john.doe@company.com", result.CreateBy);
            Assert.Equal(new DateTime(2024, 1, 15, 10, 30, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.GetByIdAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns report with null CreateBy.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ReportWithNullCreateBy_ReturnsReport()
        {
            // Arrange
            var reportId = 5;
            var expectedReport = GetSampleReports().First(x => x.Id == reportId);

            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(reportId))
                .ReturnsAsync(expectedReport);

            // Act
            var result = await _reportRepositoryMock.Object.GetByIdAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportId, result.Id);
            Assert.Null(result.CreateBy);
            Assert.Equal(new DateTime(2024, 3, 5, 11, 0, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.GetByIdAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new Report with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidReportWithAllProperties_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new Report
            {
                CreateBy = "new.user@company.com",
                CreateDate = new DateTime(2024, 4, 15, 12, 0, 0),
                Status = true
            };

            var createdReport = new Report
            {
                Id = 7,
                CreateBy = "new.user@company.com",
                CreateDate = new DateTime(2024, 4, 15, 12, 0, 0),
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _reportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("new.user@company.com", result.CreateBy);
            Assert.Equal(new DateTime(2024, 4, 15, 12, 0, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalReport_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new Report
            {
                CreateDate = new DateTime(2024, 5, 1, 9, 0, 0),
                Status = false
            };

            var createdReport = new Report
            {
                Id = 8,
                CreateDate = new DateTime(2024, 5, 1, 9, 0, 0),
                Status = false
            };

            _reportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _reportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Null(result.CreateBy);
            Assert.Equal(new DateTime(2024, 5, 1, 9, 0, 0), result.CreateDate);
            Assert.False(result.Status);
            _reportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with null CreateBy.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ReportWithNullCreateBy_ReturnsCreatedReport()
        {
            // Arrange
            var newReport = new Report
            {
                CreateBy = null,
                CreateDate = new DateTime(2024, 6, 1, 15, 30, 0),
                Status = true
            };

            var createdReport = new Report
            {
                Id = 9,
                CreateBy = null,
                CreateDate = new DateTime(2024, 6, 1, 15, 30, 0),
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _reportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Null(result.CreateBy);
            Assert.Equal(new DateTime(2024, 6, 1, 15, 30, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with current date and time.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ReportWithCurrentDateTime_ReturnsCreatedReport()
        {
            // Arrange
            var currentDateTime = DateTime.Now;
            var newReport = new Report
            {
                CreateBy = "current.user@company.com",
                CreateDate = currentDateTime,
                Status = false
            };

            var createdReport = new Report
            {
                Id = 10,
                CreateBy = "current.user@company.com",
                CreateDate = currentDateTime,
                Status = false
            };

            _reportRepositoryMock.Setup(repo => repo.CreateAsync(newReport))
                .ReturnsAsync(createdReport);

            // Act
            var result = await _reportRepositoryMock.Object.CreateAsync(newReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("current.user@company.com", result.CreateBy);
            Assert.Equal(currentDateTime, result.CreateDate);
            Assert.False(result.Status);
            _reportRepositoryMock.Verify(repo => repo.CreateAsync(newReport), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing Report.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndReport_ReturnsUpdatedReport()
        {
            // Arrange
            var reportId = 1;
            var updateReport = new Report
            {
                CreateBy = "updated.user@company.com",
                CreateDate = new DateTime(2024, 7, 15, 14, 0, 0),
                Status = false
            };

            var updatedReport = new Report
            {
                Id = 1,
                CreateBy = "updated.user@company.com",
                CreateDate = new DateTime(2024, 7, 15, 14, 0, 0),
                Status = false
            };

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(reportId, updateReport))
                .ReturnsAsync(updatedReport);

            // Act
            var result = await _reportRepositoryMock.Object.UpdateAsync(reportId, updateReport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("updated.user@company.com", result.CreateBy);
            Assert.Equal(new DateTime(2024, 7, 15, 14, 0, 0), result.CreateDate);
            Assert.False(result.Status);
            _reportRepositoryMock.Verify(repo => repo.UpdateAsync(reportId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when Report with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateReport = new Report
            {
                CreateBy = "updated.user@company.com",
                CreateDate = new DateTime(2024, 8, 1, 10, 0, 0),
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateReport))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.UpdateAsync(invalidId, updateReport);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedReport()
        {
            // Arrange
            var reportId = 2;
            var updateReport = new Report
            {
                CreateBy = "jane.smith@company.com", // Same as original
                CreateDate = new DateTime(2024, 1, 15, 14, 45, 0), // Same as original
                Status = true // Changed from false to true
            };

            var updatedReport = new Report
            {
                Id = 2,
                CreateBy = "jane.smith@company.com",
                CreateDate = new DateTime(2024, 1, 15, 14, 45, 0),
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(reportId, updateReport))
                .ReturnsAsync(updatedReport);

            // Act
            var result = await _reportRepositoryMock.Object.UpdateAsync(reportId, updateReport);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Status); // Changed property
            Assert.Equal("jane.smith@company.com", result.CreateBy); // Unchanged property
            _reportRepositoryMock.Verify(repo => repo.UpdateAsync(reportId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating CreateBy to null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateCreateByToNull_ReturnsUpdatedReport()
        {
            // Arrange
            var reportId = 3;
            var updateReport = new Report
            {
                CreateBy = null, // Changed from "mike.johnson@company.com" to null
                CreateDate = new DateTime(2024, 2, 10, 9, 15, 0), // Same as original
                Status = true // Same as original
            };

            var updatedReport = new Report
            {
                Id = 3,
                CreateBy = null,
                CreateDate = new DateTime(2024, 2, 10, 9, 15, 0),
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(reportId, updateReport))
                .ReturnsAsync(updatedReport);

            // Act
            var result = await _reportRepositoryMock.Object.UpdateAsync(reportId, updateReport);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.CreateBy);
            Assert.Equal(new DateTime(2024, 2, 10, 9, 15, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.UpdateAsync(reportId, updateReport), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateReport = new Report
            {
                CreateBy = "updated.user@company.com",
                CreateDate = new DateTime(2024, 9, 1, 10, 0, 0),
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateReport))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.UpdateAsync(zeroId, updateReport);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateReport), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing Report.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedReport()
        {
            // Arrange
            var reportId = 1;
            var deletedReport = GetSampleReports().First(x => x.Id == reportId);

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(reportId))
                .ReturnsAsync(deletedReport);

            // Act
            var result = await _reportRepositoryMock.Object.DeleteAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("john.doe@company.com", result.CreateBy);
            Assert.Equal(new DateTime(2024, 1, 15, 10, 30, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when Report with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _reportRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with Report containing null CreateBy.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ReportWithNullCreateBy_ReturnsDeletedReport()
        {
            // Arrange
            var reportId = 5;
            var deletedReport = GetSampleReports().First(x => x.Id == reportId);

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(reportId))
                .ReturnsAsync(deletedReport);

            // Act
            var result = await _reportRepositoryMock.Object.DeleteAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Null(result.CreateBy);
            Assert.Equal(new DateTime(2024, 3, 5, 11, 0, 0), result.CreateDate);
            Assert.True(result.Status);
            _reportRepositoryMock.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with inactive Report.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InactiveReport_ReturnsDeletedReport()
        {
            // Arrange
            var reportId = 4;
            var deletedReport = GetSampleReports().First(x => x.Id == reportId);

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(reportId))
                .ReturnsAsync(deletedReport);

            // Act
            var result = await _reportRepositoryMock.Object.DeleteAsync(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal("sarah.wilson@company.com", result.CreateBy);
            Assert.Equal(new DateTime(2024, 2, 10, 16, 20, 0), result.CreateDate);
            Assert.False(result.Status);
            _reportRepositoryMock.Verify(repo => repo.DeleteAsync(reportId), Times.Once);
        }

        #endregion
    }
}


