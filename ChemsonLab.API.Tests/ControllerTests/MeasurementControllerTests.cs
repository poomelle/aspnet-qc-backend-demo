using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Measurement;
using ChemsonLab.API.Models.DTO.TestResult;
using ChemsonLab.API.Repositories.MeasurementRepository;
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
    public class MeasurementControllerTests
    {
        private readonly Mock<IMeasurementRepository> _measurementRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<MeasurementController>> _loggerMock;

        private readonly MeasurementController _controller;

        public MeasurementControllerTests()
        {
            _measurementRepositoryMock = new Mock<IMeasurementRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<MeasurementController>>();

            _controller = new MeasurementController(
                _measurementRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of MeasurementController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var measurementList = new List<Measurement>
            {
                new Measurement
                {
                    Id = 1,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(5),
                    Torque = 12.5,
                    Bandwidth = 2.3,
                    StockTemp = 175.0,
                    Speed = 90.0,
                    FileName = "measurement_data.txt",
                    TestResult = new TestResult { Id = 1 }
                }
            };

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                .ReturnsAsync(measurementList);

            _mapperMock.Setup(mapper => mapper.Map<List<MeasurementDTO>>(It.IsAny<List<Measurement>>()))
                .Returns(new List<MeasurementDTO>
                {
                    new MeasurementDTO
                    {
                        Id = 1,
                        TimeAct = "05:00",
                        Torque = 12.5,
                        Bandwidth = 2.3,
                        StockTemp = 175.0,
                        Speed = 90.0,
                        FileName = "measurement_data.txt",
                        TestResult = new TestResultDTO { Id = 1 }
                    }
                });

            // Act
            var result = await _controller.GetAll(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MeasurementDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of MeasurementController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var measurement = new Measurement
            {
                Id = 1,
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(5),
                Torque = 12.5,
                Bandwidth = 2.3,
                StockTemp = 175.0,
                Speed = 90.0,
                FileName = "measurement_data.txt",
                TestResult = new TestResult { Id = 1 }
            };

            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(measurement);

            _mapperMock.Setup(mapper => mapper.Map<MeasurementDTO>(It.IsAny<Measurement>()))
                .Returns(new MeasurementDTO
                {
                    Id = 1,
                    TimeAct = "05:00",
                    Torque = 12.5,
                    Bandwidth = 2.3,
                    StockTemp = 175.0,
                    Speed = 90.0,
                    FileName = "measurement_data.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MeasurementDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal(12.5, returnValue.Torque);
            Assert.Equal("measurement_data.txt", returnValue.FileName);
            Assert.Equal("05:00", returnValue.TimeAct);
        }

        /// <summary>
        /// Test the GetById method of MeasurementController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of MeasurementController to ensure it returns CreatedAtAction for a valid measurement creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidMeasurement_ReturnsCreatedAtAction()
        {
            // Arrange
            var newMeasurement = new AddMeasurementRequestDTO
            {
                TestResultId = 1,
                TimeAct = "05:00",
                Torque = 12.5,
                Bandwidth = 2.3,
                StockTemp = 175.0,
                Speed = 90.0,
                FileName = "new_measurement.txt"
            };

            _mapperMock.Setup(mapper => mapper.Map<Measurement>(It.IsAny<AddMeasurementRequestDTO>()))
                .Returns(new Measurement
                {
                    Id = 1,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(5),
                    Torque = 12.5,
                    Bandwidth = 2.3,
                    StockTemp = 175.0,
                    Speed = 90.0,
                    FileName = "new_measurement.txt"
                });

            _measurementRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Measurement>()))
                .ReturnsAsync(new Measurement
                {
                    Id = 1,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(5),
                    Torque = 12.5,
                    Bandwidth = 2.3,
                    StockTemp = 175.0,
                    Speed = 90.0,
                    FileName = "new_measurement.txt",
                    TestResult = new TestResult { Id = 1 }
                });

            _mapperMock.Setup(mapper => mapper.Map<MeasurementDTO>(It.IsAny<Measurement>()))
                .Returns(new MeasurementDTO
                {
                    Id = 1,
                    TimeAct = "05:00",
                    Torque = 12.5,
                    Bandwidth = 2.3,
                    StockTemp = 175.0,
                    Speed = 90.0,
                    FileName = "new_measurement.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Create(newMeasurement);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<MeasurementDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal(12.5, returnValue.Torque);
            Assert.Equal("new_measurement.txt", returnValue.FileName);
        }

        /// <summary>
        /// Test the Create method of MeasurementController to ensure it returns error for an invalid measurement creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidMeasurement_ReturnsError()
        {
            // Arrange
            var newMeasurement = new AddMeasurementRequestDTO
            {
                TestResultId = 0, // Invalid TestResultId
                TimeAct = "05:00",
                Torque = 12.5
            };

            _controller.ModelState.AddModelError("TestResultId", "The TestResultId field is required.");

            // Act
            var result = await _controller.Create(newMeasurement);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of MeasurementController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidMeasurement_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateMeasurement = new UpdateMeasurementRequestDTO
            {
                TestResultId = 1,
                TimeAct = "07:30",
                Torque = 15.0,
                Bandwidth = 3.0,
                StockTemp = 185.0,
                Speed = 100.0,
                FileName = "updated_measurement.txt"
            };

            _mapperMock.Setup(mapper => mapper.Map<Measurement>(It.IsAny<UpdateMeasurementRequestDTO>()))
                .Returns(new Measurement
                {
                    Id = 1,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(7.5),
                    Torque = 15.0,
                    Bandwidth = 3.0,
                    StockTemp = 185.0,
                    Speed = 100.0,
                    FileName = "updated_measurement.txt"
                });

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<Measurement>()))
                .ReturnsAsync(new Measurement
                {
                    Id = 1,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(7.5),
                    Torque = 15.0,
                    Bandwidth = 3.0,
                    StockTemp = 185.0,
                    Speed = 100.0,
                    FileName = "updated_measurement.txt",
                    TestResult = new TestResult { Id = 1 }
                });

            _mapperMock.Setup(mapper => mapper.Map<MeasurementDTO>(It.IsAny<Measurement>()))
                .Returns(new MeasurementDTO
                {
                    Id = 1,
                    TimeAct = "07:30",
                    Torque = 15.0,
                    Bandwidth = 3.0,
                    StockTemp = 185.0,
                    Speed = 100.0,
                    FileName = "updated_measurement.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Update(id, updateMeasurement);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MeasurementDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal(15.0, returnValue.Torque);
            Assert.Equal("updated_measurement.txt", returnValue.FileName);
            Assert.Equal("07:30", returnValue.TimeAct);
        }

        /// <summary>
        /// Test the Update method of MeasurementController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateMeasurement = new UpdateMeasurementRequestDTO
            {
                TestResultId = 1,
                TimeAct = "05:00",
                Torque = 12.5
            };

            _mapperMock.Setup(mapper => mapper.Map<Measurement>(It.IsAny<UpdateMeasurementRequestDTO>()))
                .Returns(new Measurement { Id = 999, TestResultId = 1, Torque = 12.5 });

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<Measurement>()))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _controller.Update(id, updateMeasurement);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of MeasurementController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidMeasurement_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateMeasurement = new UpdateMeasurementRequestDTO
            {
                TestResultId = 0, // Invalid TestResultId
                TimeAct = "05:00"
            };

            _controller.ModelState.AddModelError("TestResultId", "The TestResultId field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<Measurement>(It.IsAny<UpdateMeasurementRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateMeasurement);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of MeasurementController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var measurementToDelete = new Measurement
            {
                Id = 1,
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(5),
                Torque = 12.5,
                Bandwidth = 2.3,
                StockTemp = 175.0,
                Speed = 90.0,
                FileName = "measurement_data.txt",
                TestResult = new TestResult { Id = 1 }
            };

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(measurementToDelete);

            _mapperMock.Setup(mapper => mapper.Map<MeasurementDTO>(It.IsAny<Measurement>()))
                .Returns(new MeasurementDTO
                {
                    Id = 1,
                    TimeAct = "05:00",
                    Torque = 12.5,
                    Bandwidth = 2.3,
                    StockTemp = 175.0,
                    Speed = 90.0,
                    FileName = "measurement_data.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MeasurementDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal(12.5, returnValue.Torque);
            Assert.Equal("measurement_data.txt", returnValue.FileName);
        }

        /// <summary>
        /// Test the Delete method of MeasurementController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
