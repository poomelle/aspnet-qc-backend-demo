using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.MeasurementRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class MeasurementRepositoryTests
    {
        private readonly Mock<IMeasurementRepository> _measurementRepositoryMock;

        public MeasurementRepositoryTests()
        {
            _measurementRepositoryMock = new Mock<IMeasurementRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample Measurement data for testing purposes.
        /// </summary>
        /// <returns>List of sample Measurement objects</returns>
        private List<Measurement> GetSampleMeasurements()
        {
            return new List<Measurement>
            {
                new Measurement
                {
                    Id = 1,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(5.5),
                    Torque = 25.75,
                    Bandwidth = 10.5,
                    StockTemp = 85.2,
                    Speed = 50.25,
                    FileName = "measurement_001.dat",
                    TestResult = new TestResult
                    {
                        Id = 1,
                        TestDate = new DateTime(2024, 1, 15),
                        DriveUnit = "Unit1",
                        TestTime = 330.5,
                        TestNumber = 1,
                        TestType = "Standard",
                        TestMethod = "Method A",
                        FileName = "test_001.dat",
                        Product = new Product { Id = 1, Name = "Product A" },
                        Machine = new Machine { Id = 1, Name = "Machine 1" }
                    }
                },
                new Measurement
                {
                    Id = 2,
                    TestResultId = 1,
                    TimeAct = TimeSpan.FromMinutes(7.2),
                    Torque = 30.0,
                    Bandwidth = 12.8,
                    StockTemp = 90.5,
                    Speed = 55.0,
                    FileName = "measurement_002.dat",
                    TestResult = new TestResult
                    {
                        Id = 1,
                        TestDate = new DateTime(2024, 1, 15),
                        DriveUnit = "Unit1",
                        TestTime = 330.5,
                        TestNumber = 1,
                        TestType = "Standard",
                        TestMethod = "Method A",
                        FileName = "test_001.dat",
                        Product = new Product { Id = 1, Name = "Product A" },
                        Machine = new Machine { Id = 1, Name = "Machine 1" }
                    }
                },
                new Measurement
                {
                    Id = 3,
                    TestResultId = 2,
                    TimeAct = TimeSpan.FromMinutes(4.8),
                    Torque = 22.5,
                    Bandwidth = 9.2,
                    StockTemp = 82.0,
                    Speed = 48.0,
                    FileName = "measurement_003.dat",
                    TestResult = new TestResult
                    {
                        Id = 2,
                        TestDate = new DateTime(2024, 1, 16),
                        DriveUnit = "Unit2",
                        TestTime = 288.0,
                        TestNumber = 2,
                        TestType = "Extended",
                        TestMethod = "Method B",
                        FileName = "test_002.dat",
                        Product = new Product { Id = 2, Name = "Product B" },
                        Machine = new Machine { Id = 2, Name = "Machine 2" }
                    }
                },
                new Measurement
                {
                    Id = 4,
                    TestResultId = 3,
                    TimeAct = TimeSpan.FromMinutes(6.0),
                    Torque = null,
                    Bandwidth = null,
                    StockTemp = 88.0,
                    Speed = null,
                    FileName = null,
                    TestResult = new TestResult
                    {
                        Id = 3,
                        TestDate = new DateTime(2024, 1, 17),
                        DriveUnit = "Unit3",
                        TestTime = 360.0,
                        TestNumber = 3,
                        TestType = "Minimal",
                        TestMethod = "Method C",
                        FileName = "test_003.dat",
                        Product = new Product { Id = 3, Name = "Product C" },
                        Machine = new Machine { Id = 3, Name = "Machine 3" }
                    }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all Measurement items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllMeasurements()
        {
            // Arrange
            var expectedMeasurements = GetSampleMeasurements();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedMeasurements, result);
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(null), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Measurement items by TestResultId.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestResultId_ReturnsFilteredMeasurements()
        {
            // Arrange
            var testResultId = "1";
            var expectedMeasurements = GetSampleMeasurements().Where(x => x.TestResultId == int.Parse(testResultId)).ToList();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(testResultId))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, measurement => Assert.Equal(int.Parse(testResultId), measurement.TestResultId));
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Measurement items by different TestResultId.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByDifferentTestResultId_ReturnsFilteredMeasurements()
        {
            // Arrange
            var testResultId = "2";
            var expectedMeasurements = GetSampleMeasurements().Where(x => x.TestResultId == int.Parse(testResultId)).ToList();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(testResultId))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(int.Parse(testResultId), result.First().TestResultId);
            Assert.Equal(3, result.First().Id);
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no Measurement items match filter.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingMeasurements_ReturnsEmptyList()
        {
            // Arrange
            var testResultId = "999";
            var expectedMeasurements = new List<Measurement>();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(testResultId))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid TestResultId filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidTestResultIdFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidTestResultId = "invalid";
            var expectedMeasurements = new List<Measurement>();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(invalidTestResultId))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync(invalidTestResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(invalidTestResultId), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with empty string filter returns all measurements.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_EmptyStringFilter_ReturnsAllMeasurements()
        {
            // Arrange
            var emptyFilter = "";
            var expectedMeasurements = GetSampleMeasurements();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(emptyFilter))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync(emptyFilter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedMeasurements, result);
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(emptyFilter), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with whitespace filter returns all measurements.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_WhitespaceFilter_ReturnsAllMeasurements()
        {
            // Arrange
            var whitespaceFilter = "   ";
            var expectedMeasurements = GetSampleMeasurements();

            _measurementRepositoryMock.Setup(repo => repo.GetAllAsync(whitespaceFilter))
                .ReturnsAsync(expectedMeasurements);

            // Act
            var result = await _measurementRepositoryMock.Object.GetAllAsync(whitespaceFilter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal(expectedMeasurements, result);
            _measurementRepositoryMock.Verify(repo => repo.GetAllAsync(whitespaceFilter), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns Measurement when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsMeasurement()
        {
            // Arrange
            var measurementId = 1;
            var expectedMeasurement = GetSampleMeasurements().First(x => x.Id == measurementId);

            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(measurementId))
                .ReturnsAsync(expectedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.GetByIdAsync(measurementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(measurementId, result.Id);
            Assert.Equal(1, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(5.5), result.TimeAct);
            Assert.Equal(25.75, result.Torque);
            Assert.Equal("measurement_001.dat", result.FileName);
            Assert.NotNull(result.TestResult);
            Assert.Equal("Product A", result.TestResult.Product.Name);
            _measurementRepositoryMock.Verify(repo => repo.GetByIdAsync(measurementId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns measurement with null optional properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_MeasurementWithNullProperties_ReturnsMeasurement()
        {
            // Arrange
            var measurementId = 4;
            var expectedMeasurement = GetSampleMeasurements().First(x => x.Id == measurementId);

            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(measurementId))
                .ReturnsAsync(expectedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.GetByIdAsync(measurementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(measurementId, result.Id);
            Assert.Equal(3, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(6.0), result.TimeAct);
            Assert.Null(result.Torque);
            Assert.Null(result.Bandwidth);
            Assert.Equal(88.0, result.StockTemp);
            Assert.Null(result.Speed);
            Assert.Null(result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.GetByIdAsync(measurementId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _measurementRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new Measurement.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidMeasurement_ReturnsCreatedMeasurement()
        {
            // Arrange
            var newMeasurement = new Measurement
            {
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(8.5),
                Torque = 35.5,
                Bandwidth = 15.2,
                StockTemp = 92.5,
                Speed = 60.0,
                FileName = "measurement_new.dat",
                TestResult = new TestResult
                {
                    Id = 1,
                    TestDate = new DateTime(2024, 1, 15),
                    Product = new Product { Id = 1, Name = "Product A" },
                    Machine = new Machine { Id = 1, Name = "Machine 1" }
                }
            };

            var createdMeasurement = new Measurement
            {
                Id = 5,
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(8.5),
                Torque = 35.5,
                Bandwidth = 15.2,
                StockTemp = 92.5,
                Speed = 60.0,
                FileName = "measurement_new.dat",
                TestResult = new TestResult
                {
                    Id = 1,
                    TestDate = new DateTime(2024, 1, 15),
                    Product = new Product { Id = 1, Name = "Product A" },
                    Machine = new Machine { Id = 1, Name = "Machine 1" }
                }
            };

            _measurementRepositoryMock.Setup(repo => repo.CreateAsync(newMeasurement))
                .ReturnsAsync(createdMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.CreateAsync(newMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(1, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(8.5), result.TimeAct);
            Assert.Equal(35.5, result.Torque);
            Assert.Equal("measurement_new.dat", result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.CreateAsync(newMeasurement), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalMeasurement_ReturnsCreatedMeasurement()
        {
            // Arrange
            var newMeasurement = new Measurement
            {
                TestResultId = 2,
                TimeAct = TimeSpan.FromMinutes(3.0),
                TestResult = new TestResult
                {
                    Id = 2,
                    TestDate = new DateTime(2024, 1, 16),
                    Product = new Product { Id = 2, Name = "Product B" },
                    Machine = new Machine { Id = 2, Name = "Machine 2" }
                }
            };

            var createdMeasurement = new Measurement
            {
                Id = 6,
                TestResultId = 2,
                TimeAct = TimeSpan.FromMinutes(3.0),
                TestResult = new TestResult
                {
                    Id = 2,
                    TestDate = new DateTime(2024, 1, 16),
                    Product = new Product { Id = 2, Name = "Product B" },
                    Machine = new Machine { Id = 2, Name = "Machine 2" }
                }
            };

            _measurementRepositoryMock.Setup(repo => repo.CreateAsync(newMeasurement))
                .ReturnsAsync(createdMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.CreateAsync(newMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(2, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(3.0), result.TimeAct);
            Assert.Null(result.Torque);
            Assert.Null(result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.CreateAsync(newMeasurement), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MeasurementWithNullOptionalProperties_ReturnsCreatedMeasurement()
        {
            // Arrange
            var newMeasurement = new Measurement
            {
                TestResultId = 3,
                TimeAct = TimeSpan.FromMinutes(2.5),
                Torque = null,
                Bandwidth = null,
                StockTemp = null,
                Speed = null,
                FileName = null,
                TestResult = new TestResult { Id = 3 }
            };

            var createdMeasurement = new Measurement
            {
                Id = 7,
                TestResultId = 3,
                TimeAct = TimeSpan.FromMinutes(2.5),
                Torque = null,
                Bandwidth = null,
                StockTemp = null,
                Speed = null,
                FileName = null,
                TestResult = new TestResult { Id = 3 }
            };

            _measurementRepositoryMock.Setup(repo => repo.CreateAsync(newMeasurement))
                .ReturnsAsync(createdMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.CreateAsync(newMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(3, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(2.5), result.TimeAct);
            Assert.Null(result.Torque);
            Assert.Null(result.Bandwidth);
            Assert.Null(result.StockTemp);
            Assert.Null(result.Speed);
            Assert.Null(result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.CreateAsync(newMeasurement), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with extreme measurement values.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ExtremeMeasurementValues_ReturnsCreatedMeasurement()
        {
            // Arrange
            var newMeasurement = new Measurement
            {
                TestResultId = 4,
                TimeAct = TimeSpan.FromHours(2.5), // Long duration
                Torque = 999.99,
                Bandwidth = 0.01, // Very small value
                StockTemp = -10.5, // Negative temperature
                Speed = 1000.0, // High speed
                FileName = "extreme_measurement_test.dat",
                TestResult = new TestResult { Id = 4 }
            };

            var createdMeasurement = new Measurement
            {
                Id = 8,
                TestResultId = 4,
                TimeAct = TimeSpan.FromHours(2.5),
                Torque = 999.99,
                Bandwidth = 0.01,
                StockTemp = -10.5,
                Speed = 1000.0,
                FileName = "extreme_measurement_test.dat",
                TestResult = new TestResult { Id = 4 }
            };

            _measurementRepositoryMock.Setup(repo => repo.CreateAsync(newMeasurement))
                .ReturnsAsync(createdMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.CreateAsync(newMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal(TimeSpan.FromHours(2.5), result.TimeAct);
            Assert.Equal(999.99, result.Torque);
            Assert.Equal(0.01, result.Bandwidth);
            Assert.Equal(-10.5, result.StockTemp);
            Assert.Equal(1000.0, result.Speed);
            _measurementRepositoryMock.Verify(repo => repo.CreateAsync(newMeasurement), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing Measurement.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndMeasurement_ReturnsUpdatedMeasurement()
        {
            // Arrange
            var measurementId = 1;
            var updateMeasurement = new Measurement
            {
                TestResultId = 2,
                TimeAct = TimeSpan.FromMinutes(10.0),
                Torque = 40.0,
                Bandwidth = 18.5,
                StockTemp = 95.0,
                Speed = 65.0,
                FileName = "measurement_updated.dat",
                TestResult = new TestResult { Id = 2 }
            };

            var updatedMeasurement = new Measurement
            {
                Id = 1,
                TestResultId = 2,
                TimeAct = TimeSpan.FromMinutes(10.0),
                Torque = 40.0,
                Bandwidth = 18.5,
                StockTemp = 95.0,
                Speed = 65.0,
                FileName = "measurement_updated.dat",
                TestResult = new TestResult { Id = 2 }
            };

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(measurementId, updateMeasurement))
                .ReturnsAsync(updatedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.UpdateAsync(measurementId, updateMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(10.0), result.TimeAct);
            Assert.Equal(40.0, result.Torque);
            Assert.Equal("measurement_updated.dat", result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.UpdateAsync(measurementId, updateMeasurement), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when Measurement with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateMeasurement = new Measurement
            {
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(5.0),
                Torque = 25.0,
                TestResult = new TestResult { Id = 1 }
            };

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateMeasurement))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.UpdateAsync(invalidId, updateMeasurement);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateMeasurement), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedMeasurement()
        {
            // Arrange
            var measurementId = 2;
            var updateMeasurement = new Measurement
            {
                TestResultId = 1, // Same as original
                TimeAct = TimeSpan.FromMinutes(12.0), // Changed
                Torque = 45.0, // Changed
                Bandwidth = 12.8, // Same as original
                StockTemp = 90.5, // Same as original
                Speed = 70.0, // Changed
                FileName = "measurement_002_updated.dat", // Changed
                TestResult = new TestResult { Id = 1 }
            };

            var updatedMeasurement = new Measurement
            {
                Id = 2,
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(12.0),
                Torque = 45.0,
                Bandwidth = 12.8,
                StockTemp = 90.5,
                Speed = 70.0,
                FileName = "measurement_002_updated.dat",
                TestResult = new TestResult { Id = 1 }
            };

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(measurementId, updateMeasurement))
                .ReturnsAsync(updatedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.UpdateAsync(measurementId, updateMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TimeSpan.FromMinutes(12.0), result.TimeAct);
            Assert.Equal(45.0, result.Torque);
            Assert.Equal(70.0, result.Speed);
            Assert.Equal("measurement_002_updated.dat", result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.UpdateAsync(measurementId, updateMeasurement), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedMeasurement()
        {
            // Arrange
            var measurementId = 3;
            var updateMeasurement = new Measurement
            {
                TestResultId = 2,
                TimeAct = TimeSpan.FromMinutes(4.8),
                Torque = null, // Changed from 22.5 to null
                Bandwidth = null, // Changed from 9.2 to null
                StockTemp = 82.0,
                Speed = null, // Changed from 48.0 to null
                FileName = null, // Changed from "measurement_003.dat" to null
                TestResult = new TestResult { Id = 2 }
            };

            var updatedMeasurement = new Measurement
            {
                Id = 3,
                TestResultId = 2,
                TimeAct = TimeSpan.FromMinutes(4.8),
                Torque = null,
                Bandwidth = null,
                StockTemp = 82.0,
                Speed = null,
                FileName = null,
                TestResult = new TestResult { Id = 2 }
            };

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(measurementId, updateMeasurement))
                .ReturnsAsync(updatedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.UpdateAsync(measurementId, updateMeasurement);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Torque);
            Assert.Null(result.Bandwidth);
            Assert.Null(result.Speed);
            Assert.Null(result.FileName);
            Assert.Equal(82.0, result.StockTemp);
            _measurementRepositoryMock.Verify(repo => repo.UpdateAsync(measurementId, updateMeasurement), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateMeasurement = new Measurement
            {
                TestResultId = 1,
                TimeAct = TimeSpan.FromMinutes(5.0),
                TestResult = new TestResult { Id = 1 }
            };

            _measurementRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateMeasurement))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.UpdateAsync(zeroId, updateMeasurement);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateMeasurement), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing Measurement.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedMeasurement()
        {
            // Arrange
            var measurementId = 1;
            var deletedMeasurement = GetSampleMeasurements().First(x => x.Id == measurementId);

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(measurementId))
                .ReturnsAsync(deletedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.DeleteAsync(measurementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(5.5), result.TimeAct);
            Assert.Equal(25.75, result.Torque);
            Assert.Equal("measurement_001.dat", result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.DeleteAsync(measurementId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when Measurement with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Measurement?)null);

            // Act
            var result = await _measurementRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _measurementRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with measurement containing null optional properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_MeasurementWithNullProperties_ReturnsDeletedMeasurement()
        {
            // Arrange
            var measurementId = 4;
            var deletedMeasurement = GetSampleMeasurements().First(x => x.Id == measurementId);

            _measurementRepositoryMock.Setup(repo => repo.DeleteAsync(measurementId))
                .ReturnsAsync(deletedMeasurement);

            // Act
            var result = await _measurementRepositoryMock.Object.DeleteAsync(measurementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal(3, result.TestResultId);
            Assert.Equal(TimeSpan.FromMinutes(6.0), result.TimeAct);
            Assert.Null(result.Torque);
            Assert.Null(result.Bandwidth);
            Assert.Null(result.Speed);
            Assert.Null(result.FileName);
            _measurementRepositoryMock.Verify(repo => repo.DeleteAsync(measurementId), Times.Once);
        }

        #endregion
    }
}


