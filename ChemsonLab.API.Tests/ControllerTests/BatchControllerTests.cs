using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Batch;
using ChemsonLab.API.Repositories.BatchRepository;
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
    public class BatchControllerTests
    {
        private readonly Mock<IBatchRepository> _batchRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BatchController>> _loggerMock;
        private readonly BatchController _controller;

        public BatchControllerTests()
        {
            _batchRepositoryMock = new Mock<IBatchRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BatchController>>();
            _controller = new BatchController(_batchRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// Tests the GetAll method of BatchController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var batchList = new List<Batch> { new Batch { Id = 1, BatchName = "Test Batch" } };

            _batchRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, true))
                .ReturnsAsync(batchList);

            _mapperMock.Setup(mapper => mapper.Map<List<BatchDTO>>(It.IsAny<List<Batch>>()))
                .Returns(new List<BatchDTO> { new BatchDTO { Id = 1, BatchName = "Test Batch" } });


            // Act
            var result = await _controller.GetAll(null, null, null, null, true);

            // Assert
            // Verify that the repository method was called
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Verify that the mapper was called to map the domain model to DTO
            var returnValue = Assert.IsType<List<BatchDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of BatchController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var batch = new Batch { Id = 1, BatchName = "Test Batch" };
            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(batch);
            _mapperMock.Setup(mapper => mapper.Map<BatchDTO>(It.IsAny<Batch>()))
                .Returns(new BatchDTO { Id = 1, BatchName = "Test Batch" });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BatchDTO>(okResult.Value);
            Assert.Equal("Test Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Test the GetById method of BatchController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of BatchController to ensure it returns CreatedAtAction for a valid batch creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidBatch_ReturnsCreatedAtAction()
        {
            // Arrange
            var newBatch = new AddBatchRequestDTO { BatchName = "New Batch" };
            _mapperMock.Setup(mapper => mapper.Map<Batch>(It.IsAny<AddBatchRequestDTO>()))
                .Returns(new Batch { Id = 1, BatchName = "New Batch" });
            _batchRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Batch>()))
                .ReturnsAsync(new Batch { Id = 1, BatchName = "New Batch" });
            _mapperMock.Setup(mapper => mapper.Map<BatchDTO>(It.IsAny<Batch>()))
                .Returns(new BatchDTO { Id = 1, BatchName = "New Batch" });

            // Act
            var result = await _controller.Create(newBatch);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<BatchDTO>(createdResult.Value);
            Assert.Equal("New Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Test the Create method of BatchController to ensure it returns BadRequest for an invalid batch creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidBatch_ReturnsBadRequest()
        {
            // Arrange
            var newBatch = new AddBatchRequestDTO { ProductId = 1 }; // Missing required BatchName
            _controller.ModelState.AddModelError("BatchName", "The BatchName field is required.");

            // Act
            var result = await _controller.Create(newBatch);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of BatchController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidBatch_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateBatch = new UpdateBatchRequestDTO { BatchName = "Updated Batch" };

            _mapperMock.Setup(mapper => mapper.Map<Batch>(It.IsAny<UpdateBatchRequestDTO>()))
                .Returns(new Batch { Id = 1, BatchName = "Updated Batch" });

            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<Batch>()))
                .ReturnsAsync(new Batch { Id = 1, BatchName = "Updated Batch" });

            _mapperMock.Setup(mapper => mapper.Map<BatchDTO>(It.IsAny<Batch>()))
                .Returns(new BatchDTO { Id = 1, BatchName = "Updated Batch" });

            // Act
            var result = await _controller.Update(id, updateBatch);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BatchDTO>(okResult.Value);
            Assert.Equal("Updated Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Test the Update method of BatchController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateBatch = new UpdateBatchRequestDTO { BatchName = "Updated Batch" };
            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<Batch>()))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Update(id, updateBatch);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of BatchController to ensure it returns BadRequest for an invalid update.
        /// </summary>
        [Fact]
        public async Task Update_InvalidBatch_ReturnsBadRequest()
        {
            // Arrange
            var id = 1;
            var updateBatch = new UpdateBatchRequestDTO { ProductId = 1 }; // Missing required BatchName
            _controller.ModelState.AddModelError("BatchName", "The BatchName field is required.");
            // Act

            var result = await _controller.Update(id, updateBatch);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of BatchController to ensure it returns Ok for a valid deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(new Batch { Id = 1, BatchName = "Deleted Batch" });

            _mapperMock.Setup(mapper => mapper.Map<BatchDTO>(It.IsAny<Batch>()))
                .Returns(new BatchDTO { Id = 1, BatchName = "Deleted Batch" });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BatchDTO>(okResult.Value);
            Assert.Equal("Deleted Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Test the Delete method of BatchController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Batch?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
