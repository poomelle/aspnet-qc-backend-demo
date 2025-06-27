using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.BatchTestResult;
using ChemsonLab.API.Models.DTO.Report;
using ChemsonLab.API.Models.DTO.TestResultReport;
using ChemsonLab.API.Repositories.TestResultReportRepository;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.ControllerTests
{
    public class TestResultReportControllerTests
    {
        private readonly Mock<ITestResultReportRepository> _testResultReportRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TestResultReportController>> _loggerMock;
        private readonly TestResultReportController _controller;

        public TestResultReportControllerTests()
        {
            _testResultReportRepositoryMock = new Mock<ITestResultReportRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TestResultReportController>>();

            _controller = new TestResultReportController(
                _testResultReportRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of TestResultReportController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var testResultReportList = new List<TestResultReport>
            {
                new TestResultReport
                {
                    Id = 1,
                    ReportId = 1,
                    BatchTestResultId = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 5.5,
                    FusionDiff = 2.3,
                    Result = true,
                    Comment = "Test passed",
                    AveTestTime = 120000,
                    FileLocation = "/files/test1.pdf",
                    Report = new Report { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResult { Id = 1 }
                }
            };

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(testResultReportList);

            _mapperMock.Setup(mapper => mapper.Map<List<TestResultReportDTO>>(It.IsAny<List<TestResultReport>>()))
                .Returns(new List<TestResultReportDTO>
                {
                    new TestResultReportDTO
                    {
                        Id = 1,
                        StandardReference = "STD-001",
                        TorqueDiff = 5.5,
                        FusionDiff = 2.3,
                        Result = true,
                        Comment = "Test passed",
                        AveTestTime = 120000,
                        FileLocation = "/files/test1.pdf",
                        Report = new ReportDTO { Id = 1, CreateBy = "TestUser", Status = true },
                        BatchTestResult = new BatchTestResultDTO { Id = 1 }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TestResultReportDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests the GetAll method with filters to ensure it passes parameters correctly.
        /// </summary>
        [Fact]
        public async Task GetAll_WithFilters_ReturnsOkResult()
        {
            // Arrange
            var createBy = "TestUser";
            var testDate = "2024-01-01";
            var productName = "TestProduct";
            var batchName = "TestBatch";
            var result = "true";
            var batchTestResultId = "1";
            var exactBatchName = "ExactBatch";
            var sortBy = "StandardReference";
            var isAscending = false;

            var testResultReportList = new List<TestResultReport>
            {
                new TestResultReport
                {
                    Id = 1,
                    StandardReference = "STD-001",
                    Result = true
                }
            };

            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, testDate, productName, batchName, result, batchTestResultId, exactBatchName, sortBy, isAscending))
                .ReturnsAsync(testResultReportList);

            _mapperMock.Setup(mapper => mapper.Map<List<TestResultReportDTO>>(It.IsAny<List<TestResultReport>>()))
                .Returns(new List<TestResultReportDTO>());

            // Act
            var result_actual = await _controller.GetAll(createBy, testDate, productName, batchName, result, batchTestResultId, exactBatchName, sortBy, isAscending);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result_actual);
            _testResultReportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, testDate, productName, batchName, result, batchTestResultId, exactBatchName, sortBy, isAscending), Times.Once);
        }

        /// <summary>
        /// Tests the GetAll method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetAll_ThrowsException_Returns500Error()
        {
            // Arrange
            _testResultReportRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, null, null, null, true);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the GetById method of TestResultReportController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var testResultReport = new TestResultReport
            {
                Id = 1,
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-001",
                TorqueDiff = 5.5,
                FusionDiff = 2.3,
                Result = true,
                Comment = "Test passed",
                AveTestTime = 120000,
                FileLocation = "/files/test1.pdf",
                Report = new Report { Id = 1, CreateBy = "TestUser", Status = true },
                BatchTestResult = new BatchTestResult { Id = 1 }
            };

            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(testResultReport);

            _mapperMock.Setup(mapper => mapper.Map<TestResultReportDTO>(It.IsAny<TestResultReport>()))
                .Returns(new TestResultReportDTO
                {
                    Id = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 5.5,
                    FusionDiff = 2.3,
                    Result = true,
                    Comment = "Test passed",
                    AveTestTime = 120000,
                    FileLocation = "/files/test1.pdf",
                    Report = new ReportDTO { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TestResultReportDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("STD-001", returnValue.StandardReference);
            Assert.Equal(5.5, returnValue.TorqueDiff);
            Assert.True(returnValue.Result);
        }

        /// <summary>
        /// Tests the GetById method of TestResultReportController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests the GetById method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetById_ThrowsException_Returns500Error()
        {
            // Arrange
            _testResultReportRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Create method of TestResultReportController to ensure it returns CreatedAtAction for valid creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidTestResultReport_ReturnsCreatedAtAction()
        {
            // Arrange
            var newTestResultReport = new AddTestResultReportRequestDTO
            {
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-001",
                TorqueDiff = 5.5,
                FusionDiff = 2.3,
                Result = true,
                Comment = "Test passed",
                AveTestTime = 120000,
                FileLocation = "/files/test1.pdf"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResultReport>(It.IsAny<AddTestResultReportRequestDTO>()))
                .Returns(new TestResultReport
                {
                    ReportId = 1,
                    BatchTestResultId = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 5.5,
                    FusionDiff = 2.3,
                    Result = true,
                    Comment = "Test passed",
                    AveTestTime = 120000,
                    FileLocation = "/files/test1.pdf"
                });

            _testResultReportRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TestResultReport>()))
                .ReturnsAsync(new TestResultReport
                {
                    Id = 1,
                    ReportId = 1,
                    BatchTestResultId = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 5.5,
                    FusionDiff = 2.3,
                    Result = true,
                    Comment = "Test passed",
                    AveTestTime = 120000,
                    FileLocation = "/files/test1.pdf",
                    Report = new Report { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResult { Id = 1 }
                });

            _mapperMock.Setup(mapper => mapper.Map<TestResultReportDTO>(It.IsAny<TestResultReport>()))
                .Returns(new TestResultReportDTO
                {
                    Id = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 5.5,
                    FusionDiff = 2.3,
                    Result = true,
                    Comment = "Test passed",
                    AveTestTime = 120000,
                    FileLocation = "/files/test1.pdf",
                    Report = new ReportDTO { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Create(newTestResultReport);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TestResultReportDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("STD-001", returnValue.StandardReference);
            Assert.True(returnValue.Result);
            Assert.Equal(nameof(TestResultReportController.GetById), createdResult.ActionName);
        }

        /// <summary>
        /// Tests the Create method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Create_ThrowsException_Returns500Error()
        {
            // Arrange
            var newTestResultReport = new AddTestResultReportRequestDTO
            {
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-001"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResultReport>(It.IsAny<AddTestResultReportRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Create(newTestResultReport);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Update method of TestResultReportController to ensure it returns Ok for valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidTestResultReport_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateTestResultReport = new UpdateTestResultReportRquestDTO
            {
                ReportId = 1,
                BatchTestResultId = 2,
                StandardReference = "STD-002",
                TorqueDiff = 7.5,
                FusionDiff = 3.1,
                Result = false,
                Comment = "Test failed",
                AveTestTime = 150000,
                FileLocation = "/files/test2.pdf"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResultReport>(It.IsAny<UpdateTestResultReportRquestDTO>()))
                .Returns(new TestResultReport
                {
                    ReportId = 1,
                    BatchTestResultId = 2,
                    StandardReference = "STD-002",
                    TorqueDiff = 7.5,
                    FusionDiff = 3.1,
                    Result = false,
                    Comment = "Test failed",
                    AveTestTime = 150000,
                    FileLocation = "/files/test2.pdf"
                });

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<TestResultReport>()))
                .ReturnsAsync(new TestResultReport
                {
                    Id = 1,
                    ReportId = 1,
                    BatchTestResultId = 2,
                    StandardReference = "STD-002",
                    TorqueDiff = 7.5,
                    FusionDiff = 3.1,
                    Result = false,
                    Comment = "Test failed",
                    AveTestTime = 150000,
                    FileLocation = "/files/test2.pdf",
                    Report = new Report { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResult { Id = 2 }
                });

            _mapperMock.Setup(mapper => mapper.Map<TestResultReportDTO>(It.IsAny<TestResultReport>()))
                .Returns(new TestResultReportDTO
                {
                    Id = 1,
                    StandardReference = "STD-002",
                    TorqueDiff = 7.5,
                    FusionDiff = 3.1,
                    Result = false,
                    Comment = "Test failed",
                    AveTestTime = 150000,
                    FileLocation = "/files/test2.pdf",
                    Report = new ReportDTO { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResultDTO { Id = 2 }
                });

            // Act
            var result = await _controller.Update(id, updateTestResultReport);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TestResultReportDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("STD-002", returnValue.StandardReference);
            Assert.False(returnValue.Result);
            Assert.Equal("Test failed", returnValue.Comment);
        }

        /// <summary>
        /// Tests the Update method of TestResultReportController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateTestResultReport = new UpdateTestResultReportRquestDTO
            {
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-001"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResultReport>(It.IsAny<UpdateTestResultReportRquestDTO>()))
                .Returns(new TestResultReport
                {
                    ReportId = 1,
                    BatchTestResultId = 1,
                    StandardReference = "STD-001"
                });

            _testResultReportRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<TestResultReport>()))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _controller.Update(id, updateTestResultReport);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests the Update method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Update_ThrowsException_Returns500Error()
        {
            // Arrange
            var id = 1;
            var updateTestResultReport = new UpdateTestResultReportRquestDTO
            {
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-001"
            };

            _mapperMock.Setup(mapper => mapper.Map<TestResultReport>(It.IsAny<UpdateTestResultReportRquestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _controller.Update(id, updateTestResultReport);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Delete method of TestResultReportController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var testResultReportToDelete = new TestResultReport
            {
                Id = 1,
                ReportId = 1,
                BatchTestResultId = 1,
                StandardReference = "STD-001",
                TorqueDiff = 5.5,
                FusionDiff = 2.3,
                Result = true,
                Comment = "Test passed",
                Report = new Report { Id = 1, CreateBy = "TestUser", Status = true },
                BatchTestResult = new BatchTestResult { Id = 1 }
            };

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(testResultReportToDelete);

            _mapperMock.Setup(mapper => mapper.Map<TestResultReportDTO>(It.IsAny<TestResultReport>()))
                .Returns(new TestResultReportDTO
                {
                    Id = 1,
                    StandardReference = "STD-001",
                    TorqueDiff = 5.5,
                    FusionDiff = 2.3,
                    Result = true,
                    Comment = "Test passed",
                    Report = new ReportDTO { Id = 1, CreateBy = "TestUser", Status = true },
                    BatchTestResult = new BatchTestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TestResultReportDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("STD-001", returnValue.StandardReference);
            Assert.True(returnValue.Result);
        }

        /// <summary>
        /// Tests the Delete method of TestResultReportController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((TestResultReport?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests the Delete method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Delete_ThrowsException_Returns500Error()
        {
            // Arrange
            _testResultReportRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
