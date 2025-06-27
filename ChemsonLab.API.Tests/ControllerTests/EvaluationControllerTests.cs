using AutoMapper;
using ChemsonLab.API.Controllers;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Evaluation;
using ChemsonLab.API.Models.DTO.TestResult;
using ChemsonLab.API.Repositories.EvaluationRepository;
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
    public class EvaluationControllerTests
    {
        private readonly Mock<IEvaluationRepository> _evaluationRepositoryMock;
        private readonly Mock<ILogger<EvaluationController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EvaluationController _controller;

        public EvaluationControllerTests()
        {
            _evaluationRepositoryMock = new Mock<IEvaluationRepository>();
            _loggerMock = new Mock<ILogger<EvaluationController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new EvaluationController(
                _evaluationRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object
            );
        }

        /// <summary>
        /// Tests the GetAll method of EvaluationController to ensure it returns Ok response.
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {
            // Arrange
            var evaluationList = new List<Evaluation>
            {
                new Evaluation
                {
                    Id = 1,
                    TestResultId = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = TimeSpan.FromMinutes(10),
                    Torque = 15.5,
                    Bandwidth = 2.5,
                    StockTemp = 180.0,
                    Speed = 100.0,
                    Energy = 25.0,
                    TimeRange = TimeSpan.FromMinutes(5),
                    TorqueRange = 5.0,
                    FileName = "test_file.txt",
                    TestResult = new TestResult { Id = 1 }
                }
            };

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null))
                .ReturnsAsync(evaluationList);

            _mapperMock.Setup(mapper => mapper.Map<List<EvaluationDTO>>(It.IsAny<List<Evaluation>>()))
                .Returns(new List<EvaluationDTO>
                {
                    new EvaluationDTO
                    {
                        Id = 1,
                        Point = 1,
                        PointName = 'A',
                        TimeEval = "10:00",
                        Torque = 15.5,
                        Bandwidth = 2.5,
                        StockTemp = 180.0,
                        Speed = 100.0,
                        Energy = 25.0,
                        TimeRange = "05:00",
                        TorqueRange = 5.0,
                        FileName = "test_file.txt",
                        TestResult = new TestResultDTO { Id = 1 }
                    }
                });

            // Act
            var result = await _controller.GetAll(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<EvaluationDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Test the GetById method of EvaluationController to ensure it returns Ok response for a valid ID.
        /// </summary>
        [Fact]
        public async Task GetById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var evaluation = new Evaluation
            {
                Id = 1,
                TestResultId = 1,
                Point = 1,
                PointName = 'A',
                TimeEval = TimeSpan.FromMinutes(10),
                Torque = 15.5,
                Bandwidth = 2.5,
                StockTemp = 180.0,
                Speed = 100.0,
                Energy = 25.0,
                TimeRange = TimeSpan.FromMinutes(5),
                TorqueRange = 5.0,
                FileName = "test_file.txt",
                TestResult = new TestResult { Id = 1 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(evaluation);

            _mapperMock.Setup(mapper => mapper.Map<EvaluationDTO>(It.IsAny<Evaluation>()))
                .Returns(new EvaluationDTO
                {
                    Id = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = "10:00",
                    Torque = 15.5,
                    Bandwidth = 2.5,
                    StockTemp = 180.0,
                    Speed = 100.0,
                    Energy = 25.0,
                    TimeRange = "05:00",
                    TorqueRange = 5.0,
                    FileName = "test_file.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<EvaluationDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal('A', returnValue.PointName);
            Assert.Equal(15.5, returnValue.Torque);
            Assert.Equal("test_file.txt", returnValue.FileName);
        }

        /// <summary>
        /// Test the GetById method of EvaluationController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _evaluationRepositoryMock.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Create method of EvaluationController to ensure it returns CreatedAtAction for a valid evaluation creation.
        /// </summary>
        [Fact]
        public async Task Create_ValidEvaluation_ReturnsCreatedAtAction()
        {
            // Arrange
            var newEvaluation = new AddEvaluationRequestDTO
            {
                TestResultId = 1,
                Point = 1,
                PointName = 'A',
                TimeEval = "10:00",
                Torque = 15.5,
                Bandwidth = 2.5,
                StockTemp = 180.0,
                Speed = 100.0,
                Energy = 25.0,
                TimeRange = "05:00",
                TorqueRange = 5.0,
                FileName = "test_file.txt"
            };

            _mapperMock.Setup(mapper => mapper.Map<Evaluation>(It.IsAny<AddEvaluationRequestDTO>()))
                .Returns(new Evaluation
                {
                    Id = 1,
                    TestResultId = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = TimeSpan.FromMinutes(10),
                    Torque = 15.5,
                    Bandwidth = 2.5,
                    StockTemp = 180.0,
                    Speed = 100.0,
                    Energy = 25.0,
                    TimeRange = TimeSpan.FromMinutes(5),
                    TorqueRange = 5.0,
                    FileName = "test_file.txt"
                });

            _evaluationRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Evaluation>()))
                .ReturnsAsync(new Evaluation
                {
                    Id = 1,
                    TestResultId = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = TimeSpan.FromMinutes(10),
                    Torque = 15.5,
                    Bandwidth = 2.5,
                    StockTemp = 180.0,
                    Speed = 100.0,
                    Energy = 25.0,
                    TimeRange = TimeSpan.FromMinutes(5),
                    TorqueRange = 5.0,
                    FileName = "test_file.txt",
                    TestResult = new TestResult { Id = 1 }
                });

            _mapperMock.Setup(mapper => mapper.Map<EvaluationDTO>(It.IsAny<Evaluation>()))
                .Returns(new EvaluationDTO
                {
                    Id = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = "10:00",
                    Torque = 15.5,
                    Bandwidth = 2.5,
                    StockTemp = 180.0,
                    Speed = 100.0,
                    Energy = 25.0,
                    TimeRange = "05:00",
                    TorqueRange = 5.0,
                    FileName = "test_file.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Create(newEvaluation);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<EvaluationDTO>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal('A', returnValue.PointName);
            Assert.Equal(15.5, returnValue.Torque);
        }

        /// <summary>
        /// Test the Create method of EvaluationController to ensure it returns error for an invalid evaluation creation.
        /// </summary>
        [Fact]
        public async Task Create_InvalidEvaluation_ReturnsError()
        {
            // Arrange
            var newEvaluation = new AddEvaluationRequestDTO
            {
                TestResultId = 0, // Invalid TestResultId
                Point = 1,
                PointName = 'A'
            };

            _controller.ModelState.AddModelError("TestResultId", "The TestResultId field is required.");

            // Act
            var result = await _controller.Create(newEvaluation);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Update method of EvaluationController to ensure it returns Ok for a valid update.
        /// </summary>
        [Fact]
        public async Task Update_ValidEvaluation_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updateEvaluation = new UpdateEvaluationRequestDTO
            {
                TestResultId = 1,
                Point = 2,
                PointName = 'B',
                TimeEval = "15:00",
                Torque = 20.0,
                Bandwidth = 3.0,
                StockTemp = 190.0,
                Speed = 120.0,
                Energy = 30.0,
                TimeRange = "07:00",
                TorqueRange = 7.5,
                FileName = "updated_file.txt"
            };

            _mapperMock.Setup(mapper => mapper.Map<Evaluation>(It.IsAny<UpdateEvaluationRequestDTO>()))
                .Returns(new Evaluation
                {
                    Id = 1,
                    TestResultId = 1,
                    Point = 2,
                    PointName = 'B',
                    TimeEval = TimeSpan.FromMinutes(15),
                    Torque = 20.0,
                    Bandwidth = 3.0,
                    StockTemp = 190.0,
                    Speed = 120.0,
                    Energy = 30.0,
                    TimeRange = TimeSpan.FromMinutes(7),
                    TorqueRange = 7.5,
                    FileName = "updated_file.txt"
                });

            _evaluationRepositoryMock.Setup(repo => repo.UpdateAsync(1, It.IsAny<Evaluation>()))
                .ReturnsAsync(new Evaluation
                {
                    Id = 1,
                    TestResultId = 1,
                    Point = 2,
                    PointName = 'B',
                    TimeEval = TimeSpan.FromMinutes(15),
                    Torque = 20.0,
                    Bandwidth = 3.0,
                    StockTemp = 190.0,
                    Speed = 120.0,
                    Energy = 30.0,
                    TimeRange = TimeSpan.FromMinutes(7),
                    TorqueRange = 7.5,
                    FileName = "updated_file.txt",
                    TestResult = new TestResult { Id = 1 }
                });

            _mapperMock.Setup(mapper => mapper.Map<EvaluationDTO>(It.IsAny<Evaluation>()))
                .Returns(new EvaluationDTO
                {
                    Id = 1,
                    Point = 2,
                    PointName = 'B',
                    TimeEval = "15:00",
                    Torque = 20.0,
                    Bandwidth = 3.0,
                    StockTemp = 190.0,
                    Speed = 120.0,
                    Energy = 30.0,
                    TimeRange = "07:00",
                    TorqueRange = 7.5,
                    FileName = "updated_file.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Update(id, updateEvaluation);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<EvaluationDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal('B', returnValue.PointName);
            Assert.Equal(20.0, returnValue.Torque);
            Assert.Equal("updated_file.txt", returnValue.FileName);
        }

        /// <summary>
        /// Test the Update method of EvaluationController to ensure it returns NotFound for an invalid ID.
        /// </summary>
        [Fact]
        public async Task Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updateEvaluation = new UpdateEvaluationRequestDTO
            {
                TestResultId = 1,
                Point = 1,
                PointName = 'A',
                TimeEval = "10:00",
                Torque = 15.5
            };

            _mapperMock.Setup(mapper => mapper.Map<Evaluation>(It.IsAny<UpdateEvaluationRequestDTO>()))
                .Returns(new Evaluation { Id = 999, TestResultId = 1, Point = 1, PointName = 'A' });

            _evaluationRepositoryMock.Setup(repo => repo.UpdateAsync(id, It.IsAny<Evaluation>()))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _controller.Update(id, updateEvaluation);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Test the Update method of EvaluationController to ensure it returns error for invalid input.
        /// </summary>
        [Fact]
        public async Task Update_InvalidEvaluation_ReturnsError()
        {
            // Arrange
            var id = 1;
            var updateEvaluation = new UpdateEvaluationRequestDTO
            {
                TestResultId = 0, // Invalid TestResultId
                Point = 1,
                PointName = 'A'
            };

            _controller.ModelState.AddModelError("TestResultId", "The TestResultId field is required.");

            // Set up mapper to throw exception when mapping invalid data
            _mapperMock.Setup(mapper => mapper.Map<Evaluation>(It.IsAny<UpdateEvaluationRequestDTO>()))
                .Throws(new Exception("Mapping failed due to invalid data"));

            // Act
            var result = await _controller.Update(id, updateEvaluation);

            // Assert
            var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeActionResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Test the Delete method of EvaluationController to ensure it returns Ok for successful deletion.
        /// </summary>
        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var evaluationToDelete = new Evaluation
            {
                Id = 1,
                TestResultId = 1,
                Point = 1,
                PointName = 'A',
                TimeEval = TimeSpan.FromMinutes(10),
                Torque = 15.5,
                Bandwidth = 2.5,
                StockTemp = 180.0,
                Speed = 100.0,
                Energy = 25.0,
                TimeRange = TimeSpan.FromMinutes(5),
                TorqueRange = 5.0,
                FileName = "test_file.txt",
                TestResult = new TestResult { Id = 1 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(evaluationToDelete);

            _mapperMock.Setup(mapper => mapper.Map<EvaluationDTO>(It.IsAny<Evaluation>()))
                .Returns(new EvaluationDTO
                {
                    Id = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = "10:00",
                    Torque = 15.5,
                    Bandwidth = 2.5,
                    StockTemp = 180.0,
                    Speed = 100.0,
                    Energy = 25.0,
                    TimeRange = "05:00",
                    TorqueRange = 5.0,
                    FileName = "test_file.txt",
                    TestResult = new TestResultDTO { Id = 1 }
                });

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<EvaluationDTO>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal('A', returnValue.PointName);
            Assert.Equal(15.5, returnValue.Torque);
            Assert.Equal("test_file.txt", returnValue.FileName);
        }

        /// <summary>
        /// Test the Delete method of EvaluationController to ensure it returns NotFound for invalid ID.
        /// </summary>
        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            _evaluationRepositoryMock.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
