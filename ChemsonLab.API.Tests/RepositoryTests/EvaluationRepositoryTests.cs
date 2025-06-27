using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.EvaluationRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class EvaluationRepositoryTests
    {
        private readonly Mock<IEvaluationRepository> _evaluationRepositoryMock;

        public EvaluationRepositoryTests()
        {
            _evaluationRepositoryMock = new Mock<IEvaluationRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample Evaluation data for testing purposes.
        /// </summary>
        /// <returns>List of sample Evaluation objects</returns>
        private List<Evaluation> GetSampleEvaluations()
        {
            return new List<Evaluation>
            {
                new Evaluation
                {
                    Id = 1,
                    TestResultId = 1,
                    Point = 1,
                    PointName = 'A',
                    TimeEval = TimeSpan.FromMinutes(5),
                    Torque = 25.5,
                    Bandwidth = 10.2,
                    StockTemp = 85.0,
                    Speed = 50.0,
                    Energy = 150.75,
                    TimeRange = TimeSpan.FromMinutes(2),
                    TorqueRange = 5.5,
                    TimeEvalInt = 300,
                    TimeRangeInt = 120,
                    FileName = "eval_001.dat",
                    TestResult = new TestResult
                    {
                        Id = 1,
                        TestDate = new DateTime(2024, 1, 15),
                        DriveUnit = "Unit1",
                        TestTime = 300.5,
                        TestNumber = 1,
                        TestType = "Standard",
                        TestMethod = "Method A",
                        FileName = "test_001.dat",
                        Product = new Product { Id = 1, Name = "Product A" },
                        Machine = new Machine { Id = 1, Name = "Machine 1" }
                    }
                },
                new Evaluation
                {
                    Id = 2,
                    TestResultId = 1,
                    Point = 2,
                    PointName = 'B',
                    TimeEval = TimeSpan.FromMinutes(7),
                    Torque = 30.2,
                    Bandwidth = 12.5,
                    StockTemp = 90.0,
                    Speed = 55.0,
                    Energy = 175.25,
                    TimeRange = TimeSpan.FromMinutes(3),
                    TorqueRange = 7.2,
                    TimeEvalInt = 420,
                    TimeRangeInt = 180,
                    FileName = "eval_002.dat",
                    TestResult = new TestResult
                    {
                        Id = 1,
                        TestDate = new DateTime(2024, 1, 15),
                        DriveUnit = "Unit1",
                        TestTime = 300.5,
                        TestNumber = 1,
                        TestType = "Standard",
                        TestMethod = "Method A",
                        FileName = "test_001.dat",
                        Product = new Product { Id = 1, Name = "Product A" },
                        Machine = new Machine { Id = 1, Name = "Machine 1" }
                    }
                },
                new Evaluation
                {
                    Id = 3,
                    TestResultId = 2,
                    Point = 1,
                    PointName = 'C',
                    TimeEval = TimeSpan.FromMinutes(4),
                    Torque = 22.8,
                    Bandwidth = 9.8,
                    StockTemp = 80.0,
                    Speed = 45.0,
                    Energy = 130.50,
                    TimeRange = TimeSpan.FromMinutes(1.5),
                    TorqueRange = 4.1,
                    TimeEvalInt = 240,
                    TimeRangeInt = 90,
                    FileName = "eval_003.dat",
                    TestResult = new TestResult
                    {
                        Id = 2,
                        TestDate = new DateTime(2024, 1, 16),
                        DriveUnit = "Unit2",
                        TestTime = 280.0,
                        TestNumber = 2,
                        TestType = "Extended",
                        TestMethod = "Method B",
                        FileName = "test_002.dat",
                        Product = new Product { Id = 2, Name = "Product B" },
                        Machine = new Machine { Id = 2, Name = "Machine 2" }
                    }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all Evaluation items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllEvaluations()
        {
            // Arrange
            var expectedEvaluations = GetSampleEvaluations();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(null, null))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedEvaluations, result);
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(null, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Evaluation items by TestResultId.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestResultId_ReturnsFilteredEvaluations()
        {
            // Arrange
            var testResultId = "1";
            var expectedEvaluations = GetSampleEvaluations().Where(x => x.TestResultId == int.Parse(testResultId)).ToList();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(testResultId, null))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, evaluation => Assert.Equal(int.Parse(testResultId), evaluation.TestResultId));
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(testResultId, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Evaluation items by PointName.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByPointName_ReturnsFilteredEvaluations()
        {
            // Arrange
            var pointName = "A";
            var expectedEvaluations = GetSampleEvaluations().Where(x => x.PointName == char.Parse(pointName)).ToList();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(null, pointName))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync(null, pointName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(char.Parse(pointName), result.First().PointName);
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(null, pointName), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with both TestResultId and PointName filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestResultIdAndPointName_ReturnsFilteredEvaluations()
        {
            // Arrange
            var testResultId = "1";
            var pointName = "B";
            var expectedEvaluations = GetSampleEvaluations()
                .Where(x => x.TestResultId == int.Parse(testResultId) && x.PointName == char.Parse(pointName))
                .ToList();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(testResultId, pointName))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync(testResultId, pointName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(int.Parse(testResultId), result.First().TestResultId);
            Assert.Equal(char.Parse(pointName), result.First().PointName);
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(testResultId, pointName), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no Evaluation items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingEvaluations_ReturnsEmptyList()
        {
            // Arrange
            var testResultId = "999";
            var expectedEvaluations = new List<Evaluation>();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(testResultId, null))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(testResultId, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid TestResultId filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidTestResultIdFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidTestResultId = "invalid";
            var expectedEvaluations = new List<Evaluation>();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(invalidTestResultId, null))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync(invalidTestResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(invalidTestResultId, null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid PointName filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidPointNameFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidPointName = "invalid";
            var expectedEvaluations = new List<Evaluation>();

            _evaluationRepositoryMock.Setup(repo => repo.GetAllAsync(null, invalidPointName))
                .ReturnsAsync(expectedEvaluations);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetAllAsync(null, invalidPointName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _evaluationRepositoryMock.Verify(repo => repo.GetAllAsync(null, invalidPointName), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns Evaluation when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsEvaluation()
        {
            // Arrange
            var evaluationId = 1;
            var expectedEvaluation = GetSampleEvaluations().First(x => x.Id == evaluationId);

            _evaluationRepositoryMock.Setup(repo => repo.GetByIdAsync(evaluationId))
                .ReturnsAsync(expectedEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetByIdAsync(evaluationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(evaluationId, result.Id);
            Assert.Equal('A', result.PointName);
            Assert.Equal(25.5, result.Torque);
            Assert.Equal("eval_001.dat", result.FileName);
            Assert.NotNull(result.TestResult);
            Assert.Equal("Product A", result.TestResult.Product.Name);
            _evaluationRepositoryMock.Verify(repo => repo.GetByIdAsync(evaluationId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _evaluationRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _evaluationRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _evaluationRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new Evaluation.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidEvaluation_ReturnsCreatedEvaluation()
        {
            // Arrange
            var newEvaluation = new Evaluation
            {
                TestResultId = 1,
                Point = 3,
                PointName = 'D',
                TimeEval = TimeSpan.FromMinutes(6),
                Torque = 28.5,
                Bandwidth = 11.5,
                StockTemp = 88.0,
                Speed = 52.0,
                Energy = 165.25,
                TimeRange = TimeSpan.FromMinutes(2.5),
                TorqueRange = 6.5,
                TimeEvalInt = 360,
                TimeRangeInt = 150,
                FileName = "eval_new.dat",
                TestResult = new TestResult
                {
                    Id = 1,
                    TestDate = new DateTime(2024, 1, 15),
                    Product = new Product { Id = 1, Name = "Product A" },
                    Machine = new Machine { Id = 1, Name = "Machine 1" }
                }
            };

            var createdEvaluation = new Evaluation
            {
                Id = 4,
                TestResultId = 1,
                Point = 3,
                PointName = 'D',
                TimeEval = TimeSpan.FromMinutes(6),
                Torque = 28.5,
                Bandwidth = 11.5,
                StockTemp = 88.0,
                Speed = 52.0,
                Energy = 165.25,
                TimeRange = TimeSpan.FromMinutes(2.5),
                TorqueRange = 6.5,
                TimeEvalInt = 360,
                TimeRangeInt = 150,
                FileName = "eval_new.dat",
                TestResult = new TestResult
                {
                    Id = 1,
                    TestDate = new DateTime(2024, 1, 15),
                    Product = new Product { Id = 1, Name = "Product A" },
                    Machine = new Machine { Id = 1, Name = "Machine 1" }
                }
            };

            _evaluationRepositoryMock.Setup(repo => repo.CreateAsync(newEvaluation))
                .ReturnsAsync(createdEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.CreateAsync(newEvaluation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal('D', result.PointName);
            Assert.Equal(28.5, result.Torque);
            Assert.Equal(1, result.TestResultId);
            Assert.Equal("eval_new.dat", result.FileName);
            _evaluationRepositoryMock.Verify(repo => repo.CreateAsync(newEvaluation), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalEvaluation_ReturnsCreatedEvaluation()
        {
            // Arrange
            var newEvaluation = new Evaluation
            {
                TestResultId = 2,
                TimeEval = TimeSpan.FromMinutes(1),
                TimeRange = TimeSpan.FromSeconds(30),
                TestResult = new TestResult
                {
                    Id = 2,
                    TestDate = new DateTime(2024, 1, 16),
                    Product = new Product { Id = 2, Name = "Product B" },
                    Machine = new Machine { Id = 2, Name = "Machine 2" }
                }
            };

            var createdEvaluation = new Evaluation
            {
                Id = 5,
                TestResultId = 2,
                TimeEval = TimeSpan.FromMinutes(1),
                TimeRange = TimeSpan.FromSeconds(30),
                TestResult = new TestResult
                {
                    Id = 2,
                    TestDate = new DateTime(2024, 1, 16),
                    Product = new Product { Id = 2, Name = "Product B" },
                    Machine = new Machine { Id = 2, Name = "Machine 2" }
                }
            };

            _evaluationRepositoryMock.Setup(repo => repo.CreateAsync(newEvaluation))
                .ReturnsAsync(createdEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.CreateAsync(newEvaluation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(2, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(1), result.TimeEval);
            _evaluationRepositoryMock.Verify(repo => repo.CreateAsync(newEvaluation), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_EvaluationWithNullOptionalProperties_ReturnsCreatedEvaluation()
        {
            // Arrange
            var newEvaluation = new Evaluation
            {
                TestResultId = 1,
                Point = null,
                PointName = null,
                TimeEval = TimeSpan.FromMinutes(3),
                Torque = null,
                Bandwidth = null,
                StockTemp = null,
                Speed = null,
                Energy = null,
                TimeRange = TimeSpan.FromMinutes(1),
                TorqueRange = null,
                TimeEvalInt = null,
                TimeRangeInt = null,
                FileName = null,
                TestResult = new TestResult { Id = 1 }
            };

            var createdEvaluation = new Evaluation
            {
                Id = 6,
                TestResultId = 1,
                Point = null,
                PointName = null,
                TimeEval = TimeSpan.FromMinutes(3),
                Torque = null,
                Bandwidth = null,
                StockTemp = null,
                Speed = null,
                Energy = null,
                TimeRange = TimeSpan.FromMinutes(1),
                TorqueRange = null,
                TimeEvalInt = null,
                TimeRangeInt = null,
                FileName = null,
                TestResult = new TestResult { Id = 1 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.CreateAsync(newEvaluation))
                .ReturnsAsync(createdEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.CreateAsync(newEvaluation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(1, result.TestResultId);
            Assert.Null(result.PointName);
            Assert.Null(result.Torque);
            Assert.Null(result.FileName);
            _evaluationRepositoryMock.Verify(repo => repo.CreateAsync(newEvaluation), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing Evaluation.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndEvaluation_ReturnsUpdatedEvaluation()
        {
            // Arrange
            var evaluationId = 1;
            var updateEvaluation = new Evaluation
            {
                TestResultId = 2,
                Point = 5,
                PointName = 'E',
                TimeEval = TimeSpan.FromMinutes(8),
                Torque = 35.0,
                Bandwidth = 15.0,
                StockTemp = 95.0,
                Speed = 60.0,
                Energy = 200.0,
                TimeRange = TimeSpan.FromMinutes(4),
                TorqueRange = 8.0,
                TimeEvalInt = 480,
                TimeRangeInt = 240,
                FileName = "eval_updated.dat",
                TestResult = new TestResult { Id = 2 }
            };

            var updatedEvaluation = new Evaluation
            {
                Id = 1,
                TestResultId = 2,
                Point = 5,
                PointName = 'E',
                TimeEval = TimeSpan.FromMinutes(8),
                Torque = 35.0,
                Bandwidth = 15.0,
                StockTemp = 95.0,
                Speed = 60.0,
                Energy = 200.0,
                TimeRange = TimeSpan.FromMinutes(4),
                TorqueRange = 8.0,
                TimeEvalInt = 480,
                TimeRangeInt = 240,
                FileName = "eval_updated.dat",
                TestResult = new TestResult { Id = 2 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.UpdateAsync(evaluationId, updateEvaluation))
                .ReturnsAsync(updatedEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.UpdateAsync(evaluationId, updateEvaluation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.TestResultId);
            Assert.Equal('E', result.PointName);
            Assert.Equal(35.0, result.Torque);
            Assert.Equal("eval_updated.dat", result.FileName);
            _evaluationRepositoryMock.Verify(repo => repo.UpdateAsync(evaluationId, updateEvaluation), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when Evaluation with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateEvaluation = new Evaluation
            {
                TestResultId = 1,
                PointName = 'F',
                TimeEval = TimeSpan.FromMinutes(5),
                TimeRange = TimeSpan.FromMinutes(2),
                TestResult = new TestResult { Id = 1 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateEvaluation))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.UpdateAsync(invalidId, updateEvaluation);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateEvaluation), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedEvaluation()
        {
            // Arrange
            var evaluationId = 2;
            var updateEvaluation = new Evaluation
            {
                TestResultId = 1, // Same as original
                Point = 2, // Same as original
                PointName = 'B', // Same as original
                TimeEval = TimeSpan.FromMinutes(10), // Changed
                Torque = 40.0, // Changed
                Bandwidth = 12.5, // Same as original
                StockTemp = 90.0, // Same as original
                Speed = 70.0, // Changed
                Energy = 175.25, // Same as original
                TimeRange = TimeSpan.FromMinutes(3), // Same as original
                TorqueRange = 7.2, // Same as original
                TimeEvalInt = 600, // Changed
                TimeRangeInt = 180, // Same as original
                FileName = "eval_002_updated.dat", // Changed
                TestResult = new TestResult { Id = 1 }
            };

            var updatedEvaluation = new Evaluation
            {
                Id = 2,
                TestResultId = 1,
                Point = 2,
                PointName = 'B',
                TimeEval = TimeSpan.FromMinutes(10),
                Torque = 40.0,
                Bandwidth = 12.5,
                StockTemp = 90.0,
                Speed = 70.0,
                Energy = 175.25,
                TimeRange = TimeSpan.FromMinutes(3),
                TorqueRange = 7.2,
                TimeEvalInt = 600,
                TimeRangeInt = 180,
                FileName = "eval_002_updated.dat",
                TestResult = new TestResult { Id = 1 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.UpdateAsync(evaluationId, updateEvaluation))
                .ReturnsAsync(updatedEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.UpdateAsync(evaluationId, updateEvaluation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TimeSpan.FromMinutes(10), result.TimeEval);
            Assert.Equal(40.0, result.Torque);
            Assert.Equal(70.0, result.Speed);
            Assert.Equal(600, result.TimeEvalInt);
            Assert.Equal("eval_002_updated.dat", result.FileName);
            _evaluationRepositoryMock.Verify(repo => repo.UpdateAsync(evaluationId, updateEvaluation), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateEvaluation = new Evaluation
            {
                TestResultId = 1,
                TimeEval = TimeSpan.FromMinutes(5),
                TimeRange = TimeSpan.FromMinutes(2),
                TestResult = new TestResult { Id = 1 }
            };

            _evaluationRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateEvaluation))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.UpdateAsync(zeroId, updateEvaluation);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateEvaluation), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing Evaluation.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedEvaluation()
        {
            // Arrange
            var evaluationId = 1;
            var deletedEvaluation = GetSampleEvaluations().First(x => x.Id == evaluationId);

            _evaluationRepositoryMock.Setup(repo => repo.DeleteAsync(evaluationId))
                .ReturnsAsync(deletedEvaluation);

            // Act
            var result = await _evaluationRepositoryMock.Object.DeleteAsync(evaluationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal('A', result.PointName);
            Assert.Equal(25.5, result.Torque);
            Assert.Equal("eval_001.dat", result.FileName);
            _evaluationRepositoryMock.Verify(repo => repo.DeleteAsync(evaluationId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when Evaluation with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _evaluationRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _evaluationRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _evaluationRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Evaluation?)null);

            // Act
            var result = await _evaluationRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _evaluationRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion
    }

}
