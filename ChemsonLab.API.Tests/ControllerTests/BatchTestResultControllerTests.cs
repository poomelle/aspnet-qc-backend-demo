using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Batch;
using ChemsonLab.API.Models.DTO.BatchTestResult;
using ChemsonLab.API.Models.DTO.TestResult;
using ChemsonLab.API.Repositories.BatchTestResultRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.ControllerTests
{
    public class BatchTestResultControllerTests
    {
        private readonly Mock<IBatchTestResultRepository> _batchTestResultRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BatchTestResultController>> _loggerMock;
        private readonly BatchTestResultController _controller;

        private readonly Mock<TestResultDTO> _testResultDtoMock;
        private readonly Mock<TestResult> _testResultDomainMock;
        private readonly Mock<BatchDTO> _batchDTOMock;

        public BatchTestResultControllerTests()
        {
            _batchTestResultRepositoryMock = new Mock<IBatchTestResultRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BatchTestResultController>>();
            _controller = new BatchTestResultController(_batchTestResultRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            _testResultDtoMock = new Mock<TestResultDTO>();
            _testResultDomainMock = new Mock<TestResult>();
            _batchDTOMock = new Mock<BatchDTO>();
        }

        /// <summary>
        /// Tests the GetAll method of BatchTestResultController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var batchTestResults = new List<BatchTestResult>
            {
                new BatchTestResult
                {
                    Id = 1,
                    TestResult = _testResultDomainMock.Object
                }
            };

            _batchTestResultRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(batchTestResults);

            _mapperMock.Setup(mapper => mapper.Map<List<BatchTestResultDTO>>(It.IsAny<List<BatchTestResult>>()))
                .Returns(new List<BatchTestResultDTO> { new BatchTestResultDTO { Id = 1, TestResult = _testResultDtoMock.Object } });

            // Act
            var result = await _controller.GetAll(null, null, null, null, null, null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BatchTestResultDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests the GetById method of BatchTestResultController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ReturnsOkResult_ForValidId()
        {
            // Arrange
            int testId = 1;
            var batchTestResult = new BatchTestResult
            {
                Id = testId,
                TestResult = _testResultDomainMock.Object
            };
            _batchTestResultRepositoryMock.Setup(repo => repo.GetByIdAsync(testId))
                .ReturnsAsync(batchTestResult);
            _mapperMock.Setup(mapper => mapper.Map<BatchTestResultDTO>(It.IsAny<BatchTestResult>()))
                .Returns(new BatchTestResultDTO { Id = testId, TestResult = _testResultDtoMock.Object });

            // Act
            var result = await _controller.GetById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BatchTestResultDTO>(okResult.Value);
            Assert.Equal(testId, returnValue.Id);
        }

        /// <summary>
        /// Test the GetById method of BatchTestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            int testId = 999; // Assuming this ID does not exist
            _batchTestResultRepositoryMock.Setup(repo => repo.GetByIdAsync(testId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _controller.GetById(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of BatchTestResultController to ensure it returns CreatedAtAction for successful creation.
        /// </summary>
        [Fact]
        public async Task Create_ReturnsCreatedAtAction_ForValidBatchTestResult()
        {
            // Arrange
            var addBatchTestResultRequestDTO = new AddBatchTestResultRequestDTO
            {
                TestResultId = 1,
                BatchId = 1
            };

            _batchTestResultRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<BatchTestResult>()))
                .ReturnsAsync(new BatchTestResult { Id = 1, TestResult = _testResultDomainMock.Object });

            _mapperMock.Setup(mapper => mapper.Map<BatchTestResult>(It.IsAny<AddBatchTestResultRequestDTO>()))
                .Returns(new BatchTestResult { Id = 1, TestResult = _testResultDomainMock.Object });

            _mapperMock.Setup(mapper => mapper.Map<BatchTestResultDTO>(It.IsAny<BatchTestResult>()))
                .Returns(new BatchTestResultDTO { Id = 1, TestResult = _testResultDtoMock.Object, Batch = _batchDTOMock.Object });

            // Act
            var result = await _controller.Create(addBatchTestResultRequestDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<BatchTestResultDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        /// <summary>
        /// Test the Create method of BatchTestResultController to ensure it returns BadRequest for invalid input.
        /// </summary>
        [Fact]
        public async Task Create_ReturnsBadRequest_ForInvalidInput()
        {
            // Arrange
            var addBatchTestResultRequestDTO = new AddBatchTestResultRequestDTO
            {
                BatchId = 1,
                // Simulating invalid input by not setting TestResultId
            };

            _controller.ModelState.AddModelError("TestResultId", "Required");

            // Act
            var result = await _controller.Create(addBatchTestResultRequestDTO);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of BatchTestResultController to ensure it returns Ok for valid update.
        /// </summary>
        [Fact]
        public async Task Update_ReturnsOk_ForValidUpdate()
        {
            // Arrange
            var updateBatchTestResultRequestDTO = new UpdateBatchTestResultRequestDTO
            {
                TestResultId = 1,
                BatchId = 1
            };

            _batchTestResultRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<BatchTestResult>()))
                .ReturnsAsync(new BatchTestResult { Id = 1, TestResult = _testResultDomainMock.Object });

            _mapperMock.Setup(mapper => mapper.Map<BatchTestResult>(It.IsAny<UpdateBatchTestResultRequestDTO>()))
                .Returns(new BatchTestResult { Id = 1, TestResult = _testResultDomainMock.Object });

            _mapperMock.Setup(mapper => mapper.Map<BatchTestResultDTO>(It.IsAny<BatchTestResult>()))
                .Returns(new BatchTestResultDTO { Id = 1, TestResult = _testResultDtoMock.Object, Batch = _batchDTOMock.Object });
            // Act
            var result = await _controller.Update(id: 1, updateBatchTestResultRequestDTO);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BatchTestResultDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        /// <summary>
        /// Test the Update method of BatchTestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            int testId = 999; // Assuming this ID does not exist
            var updateBatchTestResultRequestDTO = new UpdateBatchTestResultRequestDTO
            {
                TestResultId = 1,
                BatchId = 1
            };

            _batchTestResultRepositoryMock.Setup(repo => repo.UpdateAsync(testId, It.IsAny<BatchTestResult>()))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _controller.Update(testId, updateBatchTestResultRequestDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of BatchTestResultController to ensure it returns BadRequest for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_ReturnsBadRequest_ForInvalidInput()
        {
            // Arrange
            var updateBatchTestResultRequestDTO = new UpdateBatchTestResultRequestDTO
            {
                BatchId = 1,
                // Simulating invalid input by not setting TestResultId
            };

            _controller.ModelState.AddModelError("TestResultId", "Required");

            // Act
            var result = await _controller.Update(1, updateBatchTestResultRequestDTO);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of BatchTestResultController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsOk_ForSuccessfulDeletion()
        {
            // Arrange
            int testId = 1;
            _batchTestResultRepositoryMock.Setup(repo => repo.DeleteAsync(testId))
                .ReturnsAsync(new BatchTestResult { Id = testId, TestResult = _testResultDomainMock.Object });
            _mapperMock.Setup(mapper => mapper.Map<BatchTestResultDTO>(It.IsAny<BatchTestResult>()))
                .Returns(new BatchTestResultDTO { Id = testId, TestResult = _testResultDtoMock.Object });

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BatchTestResultDTO>(okResult.Value);
            Assert.Equal(testId, returnValue.Id);
        }

        /// <summary>
        /// Test the Delete method of BatchTestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            int testId = 999; // Assuming this ID does not exist
            _batchTestResultRepositoryMock.Setup(repo => repo.DeleteAsync(testId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _controller.Delete(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
