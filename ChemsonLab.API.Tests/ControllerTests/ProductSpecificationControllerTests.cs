using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Machine;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Models.DTO.ProductSpecification;
using ChemsonLab.API.Repositories.ProductSpecificationRepository;
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
    public class ProductSpecificationControllerTests
    {
        private readonly Mock<IProductSpecificationRepository> _productSpecificationRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<Controllers.ProductSpecificationController>> _loggerMock;

        private Controllers.ProductSpecificationController _controller;

        public ProductSpecificationControllerTests()
        {
            _productSpecificationRepositoryMock = new Mock<IProductSpecificationRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<Controllers.ProductSpecificationController>>();

            _controller = new Controllers.ProductSpecificationController(
                _productSpecificationRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of ProductSpecificationController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var productSpecificationList = new List<ProductSpecification>
            {
                new ProductSpecification
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    InUse = true,
                    Temp = 180,
                    Load = 50,
                    RPM = 100,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, null, true))
                .ReturnsAsync(productSpecificationList);

            _mapperMock.Setup(mapper => mapper.Map<List<ProductSpecificationDTO>>(It.IsAny<List<ProductSpecification>>()))
                .Returns(new List<ProductSpecificationDTO>
                {
                    new ProductSpecificationDTO
                    {
                        Id = 1,
                        InUse = true,
                        Temp = 180,
                        Load = 50,
                        RPM = 100,
                        Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                        Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductSpecificationDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of ProductSpecificationController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var productSpecification = new ProductSpecification
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                InUse = true,
                Temp = 180,
                Load = 50,
                RPM = 100,
                Product = new Product { Id = 1, Name = "Test Product", Status = true },
                Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(productSpecification);

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecificationDTO>(It.IsAny<ProductSpecification>()))
                .Returns(new ProductSpecificationDTO
                {
                    Id = 1,
                    InUse = true,
                    Temp = 180,
                    Load = 50,
                    RPM = 100,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductSpecificationDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.True(returnValue.InUse);
            Assert.Equal(180, returnValue.Temp);
            Assert.Equal(50, returnValue.Load);
            Assert.Equal(100, returnValue.RPM);
        }

        /// <summary>
        /// Test the GetById method of ProductSpecificationController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _productSpecificationRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of ProductSpecificationController to ensure it returns CreatedAtAction for a valid product specification creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidProductSpecification_ReturnsCreatedAtAction()
        {
            // Arrange
            var newProductSpecification = new AddProductSpecificationRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                InUse = true,
                Temp = 190,
                Load = 60,
                RPM = 120
            };

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecification>(It.IsAny<AddProductSpecificationRequestDTO>()))
                .Returns(new ProductSpecification
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    InUse = true,
                    Temp = 190,
                    Load = 60,
                    RPM = 120
                });

            _productSpecificationRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductSpecification>()))
                .ReturnsAsync(new ProductSpecification
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    InUse = true,
                    Temp = 190,
                    Load = 60,
                    RPM = 120,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecificationDTO>(It.IsAny<ProductSpecification>()))
                .Returns(new ProductSpecificationDTO
                {
                    Id = 1,
                    InUse = true,
                    Temp = 190,
                    Load = 60,
                    RPM = 120,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.Create(newProductSpecification);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ProductSpecificationDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.True(returnValue.InUse);
            Assert.Equal(190, returnValue.Temp);
            Assert.Equal(60, returnValue.Load);
            Assert.Equal(120, returnValue.RPM);
        }

        /// <summary>
        /// Test the Create method of ProductSpecificationController to ensure it returns error for an invalid product specification creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidProductSpecification_ReturnsError()
        {
            // Arrange
            var newProductSpecification = new AddProductSpecificationRequestDTO
            {
                ProductId = 0, // Invalid ProductId
                MachineId = 1,
                InUse = true
            };

            _controller.ModelState.AddModelError("ProductId", "The ProductId field is required.");

            // Act
            var result = await _controller.Create(newProductSpecification);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of ProductSpecificationController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidProductSpecification_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateProductSpecification = new UpdateProductSpecificationRequestDTO
            {
                ProductId = 1,
                MachineId = 2,
                InUse = false,
                Temp = 200,
                Load = 70,
                RPM = 130
            };

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecification>(It.IsAny<UpdateProductSpecificationRequestDTO>()))
                .Returns(new ProductSpecification
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 2,
                    InUse = false,
                    Temp = 200,
                    Load = 70,
                    RPM = 130
                });

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<ProductSpecification>()))
                .ReturnsAsync(new ProductSpecification
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 2,
                    InUse = false,
                    Temp = 200,
                    Load = 70,
                    RPM = 130,
                    Product = new Product { Id = 1, Name = "Test Product", Status = true },
                    Machine = new Machine { Id = 2, Name = "Updated Machine", Status = true }
                });

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecificationDTO>(It.IsAny<ProductSpecification>()))
                .Returns(new ProductSpecificationDTO
                {
                    Id = 1,
                    InUse = false,
                    Temp = 200,
                    Load = 70,
                    RPM = 130,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 2, Name = "Updated Machine", Status = true }
                });

            // Act
            var result = await _controller.Update(id, updateProductSpecification);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductSpecificationDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.False(returnValue.InUse);
            Assert.Equal(200, returnValue.Temp);
            Assert.Equal(70, returnValue.Load);
            Assert.Equal(130, returnValue.RPM);
            Assert.Equal("Updated Machine", returnValue.Machine.Name);
        }

        /// <summary>
        /// Test the Update method of ProductSpecificationController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateProductSpecification = new UpdateProductSpecificationRequestDTO
            {
                ProductId = 1,
                MachineId = 1,
                InUse = true
            };

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecification>(It.IsAny<UpdateProductSpecificationRequestDTO>()))
                .Returns(new ProductSpecification { Id = 999, ProductId = 1, MachineId = 1, InUse = true });

            _productSpecificationRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<ProductSpecification>()))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _controller.Update(id, updateProductSpecification);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of ProductSpecificationController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidProductSpecification_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateProductSpecification = new UpdateProductSpecificationRequestDTO
            {
                ProductId = 0, // Invalid ProductId
                MachineId = 1,
                InUse = true
            };

            _controller.ModelState.AddModelError("ProductId", "The ProductId field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<ProductSpecification>(It.IsAny<UpdateProductSpecificationRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateProductSpecification);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of ProductSpecificationController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var productSpecificationToDelete = new ProductSpecification
            {
                Id = 1,
                ProductId = 1,
                MachineId = 1,
                InUse = true,
                Temp = 180,
                Load = 50,
                RPM = 100,
                Product = new Product { Id = 1, Name = "Test Product", Status = true },
                Machine = new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(productSpecificationToDelete);

            _mapperMock.Setup(mapper => mapper.Map<ProductSpecificationDTO>(It.IsAny<ProductSpecification>()))
                .Returns(new ProductSpecificationDTO
                {
                    Id = 1,
                    InUse = true,
                    Temp = 180,
                    Load = 50,
                    RPM = 100,
                    Product = new ProductDTO { Id = 1, Name = "Test Product", Status = true },
                    Machine = new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductSpecificationDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.True(returnValue.InUse);
            Assert.Equal(180, returnValue.Temp);
            Assert.Equal(50, returnValue.Load);
            Assert.Equal(100, returnValue.RPM);
        }

        /// <summary>
        /// Test the Delete method of ProductSpecificationController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _productSpecificationRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((ProductSpecification?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
