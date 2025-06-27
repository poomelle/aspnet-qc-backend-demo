using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Repositories.ProductRepository;
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
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductController>>();

            _controller = new ProductController(_productRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// Tests the GetAll method of ProductController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Test Product",
                    Status = true,
                    SampleAmount = 10.5,
                    Comment = "Test Comment",
                    DBDate = DateTime.Now,
                    COA = true,
                    Colour = "Blue",
                    TorqueWarning = 15.0,
                    TorqueFail = 20.0,
                    FusionWarning = 180.0,
                    FusionFail = 200.0,
                    BulkWeight = 25.0,
                    PaperBagWeight = 0.5,
                    PaperBagNo = 50,
                    BatchWeight = 25.5
                }
            };

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(productList);

            _mapperMock.Setup(mapper => mapper.Map<List<ProductDTO>>(It.IsAny<List<Product>>()))
                .Returns(new List<ProductDTO>
                {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Test Product",
                        Status = true,
                        SampleAmount = 10.5,
                        Comment = "Test Comment",
                        DBDate = DateTime.Now,
                        COA = true,
                        Colour = "Blue",
                        TorqueWarning = 15.0,
                        TorqueFail = 20.0,
                        FusionWarning = 180.0,
                        FusionFail = 200.0,
                        BulkWeight = 25.0,
                        PaperBagWeight = 0.5,
                        PaperBagNo = 50,
                        BatchWeight = 25.5
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of ProductController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Status = true,
                SampleAmount = 10.5,
                Comment = "Test Comment",
                DBDate = DateTime.Now,
                COA = true,
                Colour = "Blue",
                TorqueWarning = 15.0,
                TorqueFail = 20.0,
                FusionWarning = 180.0,
                FusionFail = 200.0,
                BulkWeight = 25.0,
                PaperBagWeight = 0.5,
                PaperBagNo = 50,
                BatchWeight = 25.5
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(It.IsAny<Product>()))
                .Returns(new ProductDTO
                {
                    Id = 1,
                    Name = "Test Product",
                    Status = true,
                    SampleAmount = 10.5,
                    Comment = "Test Comment",
                    DBDate = DateTime.Now,
                    COA = true,
                    Colour = "Blue",
                    TorqueWarning = 15.0,
                    TorqueFail = 20.0,
                    FusionWarning = 180.0,
                    FusionFail = 200.0,
                    BulkWeight = 25.0,
                    PaperBagWeight = 0.5,
                    PaperBagNo = 50,
                    BatchWeight = 25.5
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Product", returnValue.Name);
            Assert.True(returnValue.Status);
            Assert.Equal(10.5, returnValue.SampleAmount);
            Assert.Equal("Blue", returnValue.Colour);
        }

        /// <summary>
        /// Test the GetById method of ProductController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of ProductController to ensure it returns CreatedAtAction for a valid product creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var newProduct = new AddProductRequestDTO
            {
                Name = "New Product",
                Status = true,
                SampleAmount = 15.0,
                Comment = "New Product Comment",
                DBDate = DateTime.Now,
                COA = false,
                TorqueWarning = 12.0,
                TorqueFail = 18.0,
                FusionWarning = 175.0,
                FusionFail = 195.0,
                BulkWeight = 30.0,
                PaperBagWeight = 0.6,
                PaperBagNo = 60,
                BatchWeight = 30.6
            };

            _mapperMock.Setup(mapper => mapper.Map<Product>(It.IsAny<AddProductRequestDTO>()))
                .Returns(new Product
                {
                    Id = 1,
                    Name = "New Product",
                    Status = true,
                    SampleAmount = 15.0,
                    Comment = "New Product Comment",
                    DBDate = DateTime.Now,
                    COA = false,
                    TorqueWarning = 12.0,
                    TorqueFail = 18.0,
                    FusionWarning = 175.0,
                    FusionFail = 195.0,
                    BulkWeight = 30.0,
                    PaperBagWeight = 0.6,
                    PaperBagNo = 60,
                    BatchWeight = 30.6
                });

            _productRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(new Product
                {
                    Id = 1,
                    Name = "New Product",
                    Status = true,
                    SampleAmount = 15.0,
                    Comment = "New Product Comment",
                    DBDate = DateTime.Now,
                    COA = false,
                    TorqueWarning = 12.0,
                    TorqueFail = 18.0,
                    FusionWarning = 175.0,
                    FusionFail = 195.0,
                    BulkWeight = 30.0,
                    PaperBagWeight = 0.6,
                    PaperBagNo = 60,
                    BatchWeight = 30.6
                });

            _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(It.IsAny<Product>()))
                .Returns(new ProductDTO
                {
                    Id = 1,
                    Name = "New Product",
                    Status = true,
                    SampleAmount = 15.0,
                    Comment = "New Product Comment",
                    DBDate = DateTime.Now,
                    COA = false,
                    TorqueWarning = 12.0,
                    TorqueFail = 18.0,
                    FusionWarning = 175.0,
                    FusionFail = 195.0,
                    BulkWeight = 30.0,
                    PaperBagWeight = 0.6,
                    PaperBagNo = 60,
                    BatchWeight = 30.6
                });

            // Act
            var result = await _controller.Create(newProduct);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ProductDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("New Product", returnValue.Name);
            Assert.True(returnValue.Status);
            Assert.Equal(15.0, returnValue.SampleAmount);
        }

        /// <summary>
        /// Test the Create method of ProductController to ensure it returns error for an invalid product creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidProduct_ReturnsError()
        {
            // Arrange
            var newProduct = new AddProductRequestDTO
            {
                Name = null, // Invalid - missing required name
                Status = true
            };

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Create(newProduct);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of ProductController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidProduct_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateProduct = new UpdateProductRequestDTO
            {
                Name = "Updated Product",
                Status = false,
                SampleAmount = 20.0,
                Comment = "Updated Comment",
                DBDate = DateTime.Now.AddDays(-1),
                COA = true,
                Colour = "Red",
                TorqueWarning = 18.0,
                TorqueFail = 25.0,
                FusionWarning = 185.0,
                FusionFail = 210.0,
                BulkWeight = 35.0,
                PaperBagWeight = 0.7,
                PaperBagNo = 70,
                BatchWeight = 35.7
            };

            _mapperMock.Setup(mapper => mapper.Map<Product>(It.IsAny<UpdateProductRequestDTO>()))
                .Returns(new Product
                {
                    Id = 1,
                    Name = "Updated Product",
                    Status = false,
                    SampleAmount = 20.0,
                    Comment = "Updated Comment",
                    DBDate = DateTime.Now.AddDays(-1),
                    COA = true,
                    Colour = "Red",
                    TorqueWarning = 18.0,
                    TorqueFail = 25.0,
                    FusionWarning = 185.0,
                    FusionFail = 210.0,
                    BulkWeight = 35.0,
                    PaperBagWeight = 0.7,
                    PaperBagNo = 70,
                    BatchWeight = 35.7
                });

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<Product>()))
                .ReturnsAsync(new Product
                {
                    Id = 1,
                    Name = "Updated Product",
                    Status = false,
                    SampleAmount = 20.0,
                    Comment = "Updated Comment",
                    DBDate = DateTime.Now.AddDays(-1),
                    COA = true,
                    Colour = "Red",
                    TorqueWarning = 18.0,
                    TorqueFail = 25.0,
                    FusionWarning = 185.0,
                    FusionFail = 210.0,
                    BulkWeight = 35.0,
                    PaperBagWeight = 0.7,
                    PaperBagNo = 70,
                    BatchWeight = 35.7
                });

            _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(It.IsAny<Product>()))
                .Returns(new ProductDTO
                {
                    Id = 1,
                    Name = "Updated Product",
                    Status = false,
                    SampleAmount = 20.0,
                    Comment = "Updated Comment",
                    DBDate = DateTime.Now.AddDays(-1),
                    COA = true,
                    Colour = "Red",
                    TorqueWarning = 18.0,
                    TorqueFail = 25.0,
                    FusionWarning = 185.0,
                    FusionFail = 210.0,
                    BulkWeight = 35.0,
                    PaperBagWeight = 0.7,
                    PaperBagNo = 70,
                    BatchWeight = 35.7
                });

            // Act
            var result = await _controller.Update(id, updateProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Updated Product", returnValue.Name);
            Assert.False(returnValue.Status);
            Assert.Equal(20.0, returnValue.SampleAmount);
            Assert.Equal("Red", returnValue.Colour);
        }

        /// <summary>
        /// Test the Update method of ProductController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateProduct = new UpdateProductRequestDTO
            {
                Name = "Updated Product",
                Status = true
            };

            _mapperMock.Setup(mapper => mapper.Map<Product>(It.IsAny<UpdateProductRequestDTO>()))
                .Returns(new Product { Id = 999, Name = "Updated Product", Status = true });

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<Product>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.Update(id, updateProduct);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of ProductController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidProduct_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateProduct = new UpdateProductRequestDTO
            {
                Name = null, // Invalid - missing required name
                Status = true
            };

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<Product>(It.IsAny<UpdateProductRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateProduct);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of ProductController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var productToDelete = new Product
            {
                Id = 1,
                Name = "Test Product",
                Status = true,
                SampleAmount = 10.5,
                Comment = "Test Comment",
                DBDate = DateTime.Now,
                COA = true,
                Colour = "Blue",
                TorqueWarning = 15.0,
                TorqueFail = 20.0,
                FusionWarning = 180.0,
                FusionFail = 200.0,
                BulkWeight = 25.0,
                PaperBagWeight = 0.5,
                PaperBagNo = 50,
                BatchWeight = 25.5
            };

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(productToDelete);

            _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(It.IsAny<Product>()))
                .Returns(new ProductDTO
                {
                    Id = 1,
                    Name = "Test Product",
                    Status = true,
                    SampleAmount = 10.5,
                    Comment = "Test Comment",
                    DBDate = DateTime.Now,
                    COA = true,
                    Colour = "Blue",
                    TorqueWarning = 15.0,
                    TorqueFail = 20.0,
                    FusionWarning = 180.0,
                    FusionFail = 200.0,
                    BulkWeight = 25.0,
                    PaperBagWeight = 0.5,
                    PaperBagNo = 50,
                    BatchWeight = 25.5
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Product", returnValue.Name);
            Assert.True(returnValue.Status);
            Assert.Equal("Blue", returnValue.Colour);
        }

        /// <summary>
        /// Test the Delete method of ProductController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
