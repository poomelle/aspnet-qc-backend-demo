using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.BatchTestResult;
using ChemsonLab.API.Models.DTO.Coa;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Repositories.CoaRepository;
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
    public class CoaControllerTests
    {
        private readonly Mock<ICoaRepository> _coaRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CoaController>> _loggerMock;
        private readonly CoaController _controller;

        private readonly Mock<Product> _productDomainMock;
        private readonly Mock<ProductDTO> _productDtoMock;

        public CoaControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _coaRepositoryMock = new Mock<ICoaRepository>();
            _loggerMock = new Mock<ILogger<CoaController>>();
            _controller = new CoaController(_coaRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            _productDomainMock = new Mock<Product>();
            _productDtoMock = new Mock<ProductDTO>();
        }

        /// <summary>
        /// Test to ensure that the GetAll method returns an Ok result with a list of CoaDTOs.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCoaDTOs()
        {
            // Arrange
            var coaList = new List<Coa> { new Coa { Id = 1, BatchName = "Test Batch" } };

            _coaRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(coaList);

            _mapperMock.Setup(mapper => mapper.Map<List<CoaDTO>>(It.IsAny<List<Coa>>()))
                .Returns(new List<CoaDTO> { new CoaDTO { Id = 1, BatchName = "Test Batch" } });

            // Act
            var result = await _controller.GetAll(null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CoaDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests the GetById method of BatchTestResultController to ensure it returns Ok response for valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ReturnsOkResult_ForValidId()
        {
            // Arrange
            var coa = new Coa { Id = 1, BatchName = "Test Batch" };

            _coaRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(coa);

            _mapperMock.Setup(mapper => mapper.Map<CoaDTO>(coa)).Returns(new CoaDTO { Id = 1, BatchName = "Test Batch" });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CoaDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }



        /// <summary>
        /// Test the GetById method of BatchTestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            _coaRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Coa)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        /// <summary>
        /// Test the Create method of BatchTestResultController to ensure it returns CreatedAtAction for successful creation.
        /// </summary>
        /// <summary>
        /// Test the Create method of BatchTestResultController to ensure it returns CreatedAtAction for successful creation.
        /// </summary>
        [Fact]
        public async Task Create_ReturnsCreatedAtAction_ForValidInput()
        {
            // Arrange
            var addCoaRequestDto = new AddCoaRequestDTO
            {
                ProductId = 1,
                BatchName = "New Batch"
            };

            _coaRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Coa>()))
                .ReturnsAsync(new Coa { Id = 1, Product = _productDomainMock.Object, BatchName = "New Batch" });

            _mapperMock.Setup(mapper => mapper.Map<Coa>(It.IsAny<AddCoaRequestDTO>()))
                .Returns(new Coa { Id = 1, Product = _productDomainMock.Object, BatchName = "New Batch" });

            _mapperMock.Setup(mapper => mapper.Map<CoaDTO>(It.IsAny<Coa>()))
                .Returns(new CoaDTO { Id = 1, BatchName = "New Batch", Product = _productDomainMock.Object });

            // Act
            var result = await _controller.Create(addCoaRequestDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CoaDTO>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        /// <summary>
        /// Test the Create method of BatchTestResultController to ensure it returns BadRequest for invalid input.
        /// </summary>
        [Fact]
        public async Task Create_ReturnsBadRequest_ForInvalidInput()
        {
            // Arrange
            var addCoaRequestDto = new AddCoaRequestDTO
            {
                ProductId = 1
                // Missing required BatchName
            };

            _controller.ModelState.AddModelError("BatchName", "The BatchName field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<Coa>(It.IsAny<AddCoaRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Create(addCoaRequestDto);

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
            var id = 1;
            var updateCoaRequestDto = new UpdateCoaRequestDTO
            {
                Id = 1,
                ProductId = 1,
                BatchName = "Updated Batch"
            };

            _mapperMock.Setup(mapper => mapper.Map<Coa>(It.IsAny<UpdateCoaRequestDTO>()))
                .Returns(new Coa { Id = 1, ProductId = 1, BatchName = "Updated Batch" });

            _coaRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<Coa>()))
                .ReturnsAsync(new Coa { Id = 1, ProductId = 1, BatchName = "Updated Batch" });

            _mapperMock.Setup(mapper => mapper.Map<CoaDTO>(It.IsAny<Coa>()))
                .Returns(new CoaDTO { Id = 1, BatchName = "Updated Batch" });

            // Act
            var result = await _controller.Update(id, updateCoaRequestDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CoaDTO>(okResult.Value);
            Assert.Equal("Updated Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Test the Update method of BatchTestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var id = 999;
            var updateCoaRequestDto = new UpdateCoaRequestDTO
            {
                Id = 999,
                ProductId = 1,
                BatchName = "Updated Batch"
            };

            _mapperMock.Setup(mapper => mapper.Map<Coa>(It.IsAny<UpdateCoaRequestDTO>()))
                .Returns(new Coa { Id = 999, ProductId = 1, BatchName = "Updated Batch" });

            _coaRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<Coa>()))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _controller.Update(id, updateCoaRequestDto);

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
            var id = 1;
            var updateCoaRequestDto = new UpdateCoaRequestDTO
            {
                Id = 1,
                ProductId = 1
                // Missing required BatchName
            };

            _controller.ModelState.AddModelError("BatchName", "The BatchName field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<Coa>(It.IsAny<UpdateCoaRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateCoaRequestDto);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of BatchTestResultController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsOk_ForSuccessfulDeletion()
        {
            // Arrange
            var id = 1;
            var coaToDelete = new Coa { Id = 1, ProductId = 1, BatchName = "Test Batch" };

            _coaRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(coaToDelete);

            _mapperMock.Setup(mapper => mapper.Map<CoaDTO>(It.IsAny<Coa>()))
                .Returns(new CoaDTO { Id = 1, BatchName = "Test Batch" });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CoaDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Batch", returnValue.BatchName);
        }

        /// <summary>
        /// Test the Delete method of BatchTestResultController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            var id = 999;

            _coaRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Coa?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
