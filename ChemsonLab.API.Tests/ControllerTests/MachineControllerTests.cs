using AutoMapper;
using Castle.Core.Logging;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Machine;
using ChemsonLab.API.Repositories.MachineRepository;
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
    public class MachineControllerTests
    {
        private readonly Mock<IMachineRepository> _mockMachineRepository;
        private readonly Mock<ILogger<MachineController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;

        private readonly MachineController _controller;

        public MachineControllerTests()
        {
            _mockLogger = new Mock<ILogger<MachineController>>();
            _mockMachineRepository = new Mock<IMachineRepository>();
            _mockMapper = new Mock<IMapper>();

            _controller = new MachineController(_mockMachineRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        /// <summary>
        /// Tests the GetAll method of MachineController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var machineList = new List<Machine>
            {
                new Machine { Id = 1, Name = "Test Machine", Status = true }
            };

            _mockMachineRepository.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(machineList);

            _mockMapper.Setup(mapper => mapper.Map<List<MachineDTO>>(It.IsAny<List<Machine>>()))
                .Returns(new List<MachineDTO>
                {
                    new MachineDTO { Id = 1, Name = "Test Machine", Status = true }
                });

            // Act
            var result = await _controller.GetAll(null, null, null, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MachineDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of MachineController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var machine = new Machine { Id = 1, Name = "Test Machine", Status = true };

            _mockMachineRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(machine);

            _mockMapper.Setup(mapper => mapper.Map<MachineDTO>(It.IsAny<Machine>()))
                .Returns(new MachineDTO { Id = 1, Name = "Test Machine", Status = true });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MachineDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Machine", returnValue.Name);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Test the GetById method of MachineController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockMachineRepository.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of MachineController to ensure it returns CreatedAtAction for a valid machine creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidMachine_ReturnsCreatedAtAction()
        {
            // Arrange
            var newMachine = new AddMachineReuqestDTO
            {
                Name = "New Machine",
                Status = true
            };

            _mockMapper.Setup(mapper => mapper.Map<Machine>(It.IsAny<AddMachineReuqestDTO>()))
                .Returns(new Machine { Id = 1, Name = "New Machine", Status = true });

            _mockMachineRepository.Setup(repo => repo.CreateAsync(It.IsAny<Machine>()))
                .ReturnsAsync(new Machine { Id = 1, Name = "New Machine", Status = true });

            _mockMapper.Setup(mapper => mapper.Map<MachineDTO>(It.IsAny<Machine>()))
                .Returns(new MachineDTO { Id = 1, Name = "New Machine", Status = true });

            // Act
            var result = await _controller.Create(newMachine);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<MachineDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("New Machine", returnValue.Name);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Test the Create method of MachineController to ensure it returns error for an invalid machine creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidMachine_ReturnsError()
        {
            // Arrange
            var newMachine = new AddMachineReuqestDTO
            {
                Name = null, // Invalid - missing required name
                Status = true
            };

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Create(newMachine);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of MachineController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidMachine_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateMachine = new UpdateMachineRequestDTO
            {
                Name = "Updated Machine",
                Status = false
            };

            _mockMapper.Setup(mapper => mapper.Map<Machine>(It.IsAny<UpdateMachineRequestDTO>()))
                .Returns(new Machine { Id = 1, Name = "Updated Machine", Status = false });

            _mockMachineRepository.Setup(repo => repo.UpdateAsync(1, It.IsAny<Machine>()))
                .ReturnsAsync(new Machine { Id = 1, Name = "Updated Machine", Status = false });

            _mockMapper.Setup(mapper => mapper.Map<MachineDTO>(It.IsAny<Machine>()))
                .Returns(new MachineDTO { Id = 1, Name = "Updated Machine", Status = false });

            // Act
            var result = await _controller.Update(id, updateMachine);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MachineDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Updated Machine", returnValue.Name);
            Assert.False(returnValue.Status);
        }

        /// <summary>
        /// Test the Update method of MachineController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateMachine = new UpdateMachineRequestDTO
            {
                Name = "Updated Machine",
                Status = true
            };

            _mockMapper.Setup(mapper => mapper.Map<Machine>(It.IsAny<UpdateMachineRequestDTO>()))
                .Returns(new Machine { Id = 999, Name = "Updated Machine", Status = true });

            _mockMachineRepository.Setup(repo => repo.UpdateAsync(id, It.IsAny<Machine>()))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _controller.Update(id, updateMachine);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of MachineController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidMachine_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateMachine = new UpdateMachineRequestDTO
            {
                Name = null, // Invalid - missing required name
                Status = true
            };

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mockMapper.Setup(mapper => mapper.Map<Machine>(It.IsAny<UpdateMachineRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateMachine);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of MachineController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var machineToDelete = new Machine { Id = 1, Name = "Test Machine", Status = true };

            _mockMachineRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(machineToDelete);

            _mockMapper.Setup(mapper => mapper.Map<MachineDTO>(It.IsAny<Machine>()))
                .Returns(new MachineDTO { Id = 1, Name = "Test Machine", Status = true });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MachineDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Test Machine", returnValue.Name);
            Assert.True(returnValue.Status);
        }

        /// <summary>
        /// Test the Delete method of MachineController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _mockMachineRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
