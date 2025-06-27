using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Report;
using ChemsonLab.API.Repositories.ReportRepository;
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
    public class ReportControllerTests
    {
        private readonly Mock<IReportRepository> _reportRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ReportController>> _loggerMock;
        private readonly ReportController _reportController;

        public ReportControllerTests()
        {
            _reportRepositoryMock = new Mock<IReportRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ReportController>>();

            _reportController = new ReportController(
                _reportRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of ReportController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var reportList = new List<Report>
            {
                new Report
                {
                    Id = 1,
                    CreateBy = "TestUser",
                    CreateDate = DateTime.Now,
                    Status = true
                }
            };

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null))
                .ReturnsAsync(reportList);

            _mapperMock.Setup(mapper => mapper.Map<List<ReportDTO>>(It.IsAny<List<Report>>()))
                .Returns(new List<ReportDTO>
                {
                    new ReportDTO
                    {
                        Id = 1,
                        CreateBy = "TestUser",
                        CreateDate = DateTime.Now,
                        Status = true
                    }
                });

            // Act
            var result = await _reportController.GetAll(null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ReportDTO>>(okResult.Value);
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
            var createDate = "2024-01-01";
            var status = "true";

            var reportList = new List<Report>
            {
                new Report
                {
                    Id = 1,
                    CreateBy = createBy,
                    CreateDate = DateTime.Parse(createDate),
                    Status = true
                }
            };

            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(createBy, createDate, status))
                .ReturnsAsync(reportList);

            _mapperMock.Setup(mapper => mapper.Map<List<ReportDTO>>(It.IsAny<List<Report>>()))
                .Returns(new List<ReportDTO>());

            // Act
            var result = await _reportController.GetAll(createBy, createDate, status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _reportRepositoryMock.Verify(repo => repo.GetAllAsync(createBy, createDate, status), Times.Once);
        }

        /// <summary>
        /// Tests the GetAll method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task GetAll_ThrowsException_Returns500Error()
        {
            // Arrange
            _reportRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _reportController.GetAll(null, null, null);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the GetById method of ReportController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var report = new Report
            {
                Id = 1,
                CreateBy = "TestUser",
                CreateDate = DateTime.Now,
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(report);

            _mapperMock.Setup(mapper => mapper.Map<ReportDTO>(It.IsAny<Report>()))
                .Returns(new ReportDTO
                {
                    Id = 1,
                    CreateBy = "TestUser",
                    CreateDate = DateTime.Now,
                    Status = true
                });

            // Act
            var result = await _reportController.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReportDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("TestUser", returnValue.CreateBy);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Tests the GetById method of ReportController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportController.GetById(999);

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
            _reportRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _reportController.GetById(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Create method of ReportController to ensure it returns CreatedAtAction for valid creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidReport_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDate = DateTime.Now;
            var newReport = new AddReportRequestDTO
            {
                CreateBy = "TestUser",
                CreateDate = createDate,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<AddReportRequestDTO>()))
                .Returns(new Report
                {
                    CreateBy = "TestUser",
                    CreateDate = createDate,
                    Status = true
                });

            _reportRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Report>()))
                .ReturnsAsync(new Report
                {
                    Id = 1,
                    CreateBy = "TestUser",
                    CreateDate = createDate,
                    Status = true
                });

            _mapperMock.Setup(mapper => mapper.Map<ReportDTO>(It.IsAny<Report>()))
                .Returns(new ReportDTO
                {
                    Id = 1,
                    CreateBy = "TestUser",
                    CreateDate = createDate,
                    Status = true
                });

            // Act
            var result = await _reportController.Create(newReport);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ReportDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("TestUser", returnValue.CreateBy);
            Assert.True(returnValue.Status);
            Assert.Equal(nameof(ReportController.GetById), createdResult.ActionName);
        }

        /// <summary>
        /// Tests the Create method to ensure it returns 500 error when exception occurs.
        /// </summary>
        [Fact]
        public async Task Create_ThrowsException_Returns500Error()
        {
            // Arrange
            var newReport = new AddReportRequestDTO
            {
                CreateBy = "TestUser",
                CreateDate = DateTime.Now,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<AddReportRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _reportController.Create(newReport);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Update method of ReportController to ensure it returns Ok for valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidReport_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateDate = DateTime.Now;
            var updateReport = new UpdateReportRequestDTO
            {
                CreateBy = "UpdatedUser",
                CreateDate = updateDate,
                Status = false
            };

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<UpdateReportRequestDTO>()))
                .Returns(new Report
                {
                    CreateBy = "UpdatedUser",
                    CreateDate = updateDate,
                    Status = false
                });

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<Report>()))
                .ReturnsAsync(new Report
                {
                    Id = 1,
                    CreateBy = "UpdatedUser",
                    CreateDate = updateDate,
                    Status = false
                });

            _mapperMock.Setup(mapper => mapper.Map<ReportDTO>(It.IsAny<Report>()))
                .Returns(new ReportDTO
                {
                    Id = 1,
                    CreateBy = "UpdatedUser",
                    CreateDate = updateDate,
                    Status = false
                });

            // Act
            var result = await _reportController.Update(id, updateReport);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReportDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("UpdatedUser", returnValue.CreateBy);
            Assert.False(returnValue.Status);
        }

        /// <summary>
        /// Tests the Update method of ReportController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateReport = new UpdateReportRequestDTO
            {
                CreateBy = "UpdatedUser",
                CreateDate = DateTime.Now,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<UpdateReportRequestDTO>()))
                .Returns(new Report
                {
                    CreateBy = "UpdatedUser",
                    CreateDate = DateTime.Now,
                    Status = true
                });

            _reportRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<Report>()))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportController.Update(id, updateReport);

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
            var updateReport = new UpdateReportRequestDTO
            {
                CreateBy = "UpdatedUser",
                CreateDate = DateTime.Now,
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<Report>(It.IsAny<UpdateReportRequestDTO>()))
                .Throws(new Exception("Mapping error"));

            // Act
            var result = await _reportController.Update(id, updateReport);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Tests the Delete method of ReportController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var reportToDelete = new Report
            {
                Id = 1,
                CreateBy = "TestUser",
                CreateDate = DateTime.Now,
                Status = true
            };

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(reportToDelete);

            _mapperMock.Setup(mapper => mapper.Map<ReportDTO>(It.IsAny<Report>()))
                .Returns(new ReportDTO
                {
                    Id = 1,
                    CreateBy = "TestUser",
                    CreateDate = DateTime.Now,
                    Status = true
                });

            // Act
            var result = await _reportController.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ReportDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("TestUser", returnValue.CreateBy);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Tests the Delete method of ReportController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Report?)null);

            // Act
            var result = await _reportController.Delete(id);

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
            _reportRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _reportController.Delete(1);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
