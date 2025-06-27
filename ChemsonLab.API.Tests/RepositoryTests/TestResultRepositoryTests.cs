using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.TestResultRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class TestResultRepositoryTests
    {
        private readonly Mock<ITestResultRepository> _testResultRepositoryMock;

        public TestResultRepositoryTests()
        {
            _testResultRepositoryMock = new Mock<ITestResultRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample TestResult data for testing purposes.
        /// </summary>
        /// <returns>List of sample TestResult objects</returns>
        private List<TestResult> GetSampleTestResults()
        {
            return new List<TestResult>
            {
                new TestResult
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    TestDate = new DateTime(2024, 1, 15, 10, 30, 0),
                    OperatorName = "John Doe",
                    DriveUnit = "Unit-A",
                    Mixer = "Mixer-001",
                    LoadingChute = "Chute-1",
                    Additive = "Add-001",
                    Speed = 50.5,
                    MixerTemp = 85.2,
                    StartTemp = 20.5,
                    MeasRange = 100,
                    Damping = 5,
                    TestTime = 300.5,
                    SampleWeight = 25.8,
                    CodeNumber = "CODE-001",
                    Plasticizer = "PLAST-001",
                    PlastWeight = 5.2,
                    LoadTime = 30.0,
                    LoadSpeed = 10.5,
                    Liquid = "Water",
                    Titrate = 2.5,
                    TestNumber = 1,
                    TestType = "Standard",
                    BatchGroup = "Group-A",
                    TestMethod = "Method-1",
                    Colour = "Blue",
                    Status = true,
                    FileName = "test_001.dat",
                    Product = new Product { Id = 1, Name = "Product Alpha" },
                    Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
                },
                new TestResult
                {
                    Id = 2,
                    ProductId = 2,
                    MachineId = 1,
                    TestDate = new DateTime(2024, 1, 16, 14, 45, 0),
                    OperatorName = "Jane Smith",
                    DriveUnit = "Unit-B",
                    Mixer = "Mixer-002",
                    LoadingChute = "Chute-2",
                    Additive = "Add-002",
                    Speed = 45.0,
                    MixerTemp = 90.0,
                    StartTemp = 22.0,
                    MeasRange = 150,
                    Damping = 8,
                    TestTime = 280.0,
                    SampleWeight = 30.5,
                    CodeNumber = "CODE-002",
                    Plasticizer = "PLAST-002",
                    PlastWeight = 6.8,
                    LoadTime = 25.0,
                    LoadSpeed = 12.0,
                    Liquid = "Ethanol",
                    Titrate = 3.2,
                    TestNumber = 2,
                    TestType = "Extended",
                    BatchGroup = "Group-B",
                    TestMethod = "Method-2",
                    Colour = "Red",
                    Status = false,
                    FileName = "test_002.dat",
                    Product = new Product { Id = 2, Name = "Product Beta" },
                    Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
                },
                new TestResult
                {
                    Id = 3,
                    ProductId = 1,
                    MachineId = 2,
                    TestDate = new DateTime(2024, 2, 10, 9, 15, 0),
                    OperatorName = "Mike Johnson",
                    DriveUnit = "Unit-C",
                    Mixer = "Mixer-003",
                    LoadingChute = "Chute-3",
                    Additive = "Add-003",
                    Speed = 60.0,
                    MixerTemp = 88.5,
                    StartTemp = 21.8,
                    MeasRange = 200,
                    Damping = 10,
                    TestTime = 350.2,
                    SampleWeight = 28.0,
                    CodeNumber = "CODE-003",
                    Plasticizer = "PLAST-003",
                    PlastWeight = 4.5,
                    LoadTime = 35.0,
                    LoadSpeed = 8.5,
                    Liquid = "Acetone",
                    Titrate = 1.8,
                    TestNumber = 3,
                    TestType = "Quality Check",
                    BatchGroup = "Group-C",
                    TestMethod = "Method-3",
                    Colour = "Green",
                    Status = true,
                    FileName = "test_003.dat",
                    Product = new Product { Id = 1, Name = "Product Alpha" },
                    Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
                },
                new TestResult
                {
                    Id = 4,
                    ProductId = 3,
                    MachineId = 3,
                    TestDate = new DateTime(2024, 3, 5, 16, 20, 0),
                    OperatorName = null,
                    DriveUnit = null,
                    Mixer = null,
                    LoadingChute = null,
                    Additive = null,
                    Speed = null,
                    MixerTemp = null,
                    StartTemp = null,
                    MeasRange = null,
                    Damping = null,
                    TestTime = null,
                    SampleWeight = null,
                    CodeNumber = null,
                    Plasticizer = null,
                    PlastWeight = null,
                    LoadTime = null,
                    LoadSpeed = null,
                    Liquid = null,
                    Titrate = null,
                    TestNumber = null,
                    TestType = null,
                    BatchGroup = null,
                    TestMethod = null,
                    Colour = null,
                    Status = null,
                    FileName = null,
                    Product = new Product { Id = 3, Name = "Product Gamma" },
                    Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
                },
                new TestResult
                {
                    Id = 5,
                    ProductId = 2,
                    MachineId = 2,
                    TestDate = new DateTime(2024, 3, 15, 11, 0, 0),
                    OperatorName = "Sarah Wilson",
                    DriveUnit = "Unit-D",
                    Mixer = "Mixer-004",
                    LoadingChute = "Chute-4",
                    Additive = "Add-004",
                    Speed = 55.8,
                    MixerTemp = 92.3,
                    StartTemp = 19.5,
                    MeasRange = 120,
                    Damping = 6,
                    TestTime = 295.8,
                    SampleWeight = 32.1,
                    CodeNumber = "CODE-004",
                    Plasticizer = "PLAST-004",
                    PlastWeight = 7.2,
                    LoadTime = 28.0,
                    LoadSpeed = 11.5,
                    Liquid = "Methanol",
                    Titrate = 2.9,
                    TestNumber = 4,
                    TestType = "Calibration",
                    BatchGroup = "Group-D",
                    TestMethod = "Method-4",
                    Colour = "Yellow",
                    Status = true,
                    FileName = "test_004.dat",
                    Product = new Product { Id = 2, Name = "Product Beta" },
                    Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all TestResult items.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ReturnsAllTestResults()
        {
            // Arrange
            var expectedTestResults = GetSampleTestResults();

            _testResultRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedTestResults);

            // Act
            var result = await _testResultRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedTestResults, result);
            _testResultRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no TestResult items exist.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoTestResults_ReturnsEmptyList()
        {
            // Arrange
            var expectedTestResults = new List<TestResult>();

            _testResultRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedTestResults);

            // Act
            var result = await _testResultRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _testResultRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method includes Product and Machine navigation properties.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_IncludesNavigationProperties_ReturnsTestResultsWithRelatedData()
        {
            // Arrange
            var expectedTestResults = GetSampleTestResults();

            _testResultRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedTestResults);

            // Act
            var result = await _testResultRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result.Where(x => x.Product != null), testResult =>
            {
                Assert.NotNull(testResult.Product);
                Assert.NotNull(testResult.Product.Name);
            });
            Assert.All(result.Where(x => x.Machine != null), testResult =>
            {
                Assert.NotNull(testResult.Machine);
                Assert.NotNull(testResult.Machine.Name);
            });
            _testResultRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns TestResult when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsTestResult()
        {
            // Arrange
            var testResultId = 1;
            var expectedTestResult = GetSampleTestResults().First(x => x.Id == testResultId);

            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(testResultId))
                .ReturnsAsync(expectedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.GetByIdAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testResultId, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Equal(new DateTime(2024, 1, 15, 10, 30, 0), result.TestDate);
            Assert.Equal("John Doe", result.OperatorName);
            Assert.Equal("Unit-A", result.DriveUnit);
            Assert.Equal("Mixer-001", result.Mixer);
            Assert.Equal("test_001.dat", result.FileName);
            Assert.NotNull(result.Product);
            Assert.Equal("Product Alpha", result.Product.Name);
            Assert.NotNull(result.Machine);
            Assert.Equal("Testing Machine Alpha", result.Machine.Name);
            _testResultRepositoryMock.Verify(repo => repo.GetByIdAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns TestResult with null optional properties.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_TestResultWithNullProperties_ReturnsTestResult()
        {
            // Arrange
            var testResultId = 4;
            var expectedTestResult = GetSampleTestResults().First(x => x.Id == testResultId);

            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(testResultId))
                .ReturnsAsync(expectedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.GetByIdAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testResultId, result.Id);
            Assert.Equal(3, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.Equal(new DateTime(2024, 3, 5, 16, 20, 0), result.TestDate);
            Assert.Null(result.OperatorName);
            Assert.Null(result.DriveUnit);
            Assert.Null(result.Mixer);
            Assert.Null(result.Speed);
            Assert.Null(result.TestTime);
            Assert.Null(result.Status);
            Assert.Null(result.FileName);
            _testResultRepositoryMock.Verify(repo => repo.GetByIdAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _testResultRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new TestResult with all properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidTestResultWithAllProperties_ReturnsCreatedTestResult()
        {
            // Arrange
            var newTestResult = new TestResult
            {
                ProductId = 4,
                MachineId = 4,
                TestDate = new DateTime(2024, 4, 20, 13, 15, 0),
                OperatorName = "Alice Brown",
                DriveUnit = "Unit-E",
                Mixer = "Mixer-005",
                LoadingChute = "Chute-5",
                Additive = "Add-005",
                Speed = 65.2,
                MixerTemp = 87.8,
                StartTemp = 23.1,
                MeasRange = 180,
                Damping = 7,
                TestTime = 320.5,
                SampleWeight = 27.5,
                CodeNumber = "CODE-005",
                Plasticizer = "PLAST-005",
                PlastWeight = 6.0,
                LoadTime = 32.0,
                LoadSpeed = 9.8,
                Liquid = "Isopropanol",
                Titrate = 3.5,
                TestNumber = 5,
                TestType = "Validation",
                BatchGroup = "Group-E",
                TestMethod = "Method-5",
                Colour = "Purple",
                Status = true,
                FileName = "test_new.dat",
                Product = new Product { Id = 4, Name = "Product Delta" },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine" }
            };

            var createdTestResult = new TestResult
            {
                Id = 6,
                ProductId = 4,
                MachineId = 4,
                TestDate = new DateTime(2024, 4, 20, 13, 15, 0),
                OperatorName = "Alice Brown",
                DriveUnit = "Unit-E",
                Mixer = "Mixer-005",
                LoadingChute = "Chute-5",
                Additive = "Add-005",
                Speed = 65.2,
                MixerTemp = 87.8,
                StartTemp = 23.1,
                MeasRange = 180,
                Damping = 7,
                TestTime = 320.5,
                SampleWeight = 27.5,
                CodeNumber = "CODE-005",
                Plasticizer = "PLAST-005",
                PlastWeight = 6.0,
                LoadTime = 32.0,
                LoadSpeed = 9.8,
                Liquid = "Isopropanol",
                Titrate = 3.5,
                TestNumber = 5,
                TestType = "Validation",
                BatchGroup = "Group-E",
                TestMethod = "Method-5",
                Colour = "Purple",
                Status = true,
                FileName = "test_new.dat",
                Product = new Product { Id = 4, Name = "Product Delta" },
                Machine = new Machine { Id = 4, Name = "Advanced Testing Machine" }
            };

            _testResultRepositoryMock.Setup(repo => repo.CreateAsync(newTestResult))
                .ReturnsAsync(createdTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.CreateAsync(newTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal(4, result.ProductId);
            Assert.Equal(4, result.MachineId);
            Assert.Equal("Alice Brown", result.OperatorName);
            Assert.Equal("Unit-E", result.DriveUnit);
            Assert.Equal(65.2, result.Speed);
            Assert.Equal(320.5, result.TestTime);
            Assert.True(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.CreateAsync(newTestResult), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalTestResult_ReturnsCreatedTestResult()
        {
            // Arrange
            var newTestResult = new TestResult
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = new DateTime(2024, 5, 1, 9, 0, 0),
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            var createdTestResult = new TestResult
            {
                Id = 7,
                ProductId = 1,
                MachineId = 1,
                TestDate = new DateTime(2024, 5, 1, 9, 0, 0),
                Product = new Product { Id = 1, Name = "Product Alpha" },
                Machine = new Machine { Id = 1, Name = "Testing Machine Alpha" }
            };

            _testResultRepositoryMock.Setup(repo => repo.CreateAsync(newTestResult))
                .ReturnsAsync(createdTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.CreateAsync(newTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Equal(new DateTime(2024, 5, 1, 9, 0, 0), result.TestDate);
            Assert.Null(result.OperatorName);
            Assert.Null(result.DriveUnit);
            Assert.Null(result.TestTime);
            _testResultRepositoryMock.Verify(repo => repo.CreateAsync(newTestResult), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with all optional properties set to null.
        /// </summary>
        [Fact]
        public async Task CreateAsync_TestResultWithNullOptionalProperties_ReturnsCreatedTestResult()
        {
            // Arrange
            var newTestResult = new TestResult
            {
                ProductId = 2,
                MachineId = 2,
                TestDate = new DateTime(2024, 6, 1, 15, 30, 0),
                OperatorName = null,
                DriveUnit = null,
                Mixer = null,
                LoadingChute = null,
                Additive = null,
                Speed = null,
                MixerTemp = null,
                StartTemp = null,
                MeasRange = null,
                Damping = null,
                TestTime = null,
                SampleWeight = null,
                CodeNumber = null,
                Plasticizer = null,
                PlastWeight = null,
                LoadTime = null,
                LoadSpeed = null,
                Liquid = null,
                Titrate = null,
                TestNumber = null,
                TestType = null,
                BatchGroup = null,
                TestMethod = null,
                Colour = null,
                Status = null,
                FileName = null,
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
            };

            var createdTestResult = new TestResult
            {
                Id = 8,
                ProductId = 2,
                MachineId = 2,
                TestDate = new DateTime(2024, 6, 1, 15, 30, 0),
                OperatorName = null,
                DriveUnit = null,
                Mixer = null,
                LoadingChute = null,
                Additive = null,
                Speed = null,
                MixerTemp = null,
                StartTemp = null,
                MeasRange = null,
                Damping = null,
                TestTime = null,
                SampleWeight = null,
                CodeNumber = null,
                Plasticizer = null,
                PlastWeight = null,
                LoadTime = null,
                LoadSpeed = null,
                Liquid = null,
                Titrate = null,
                TestNumber = null,
                TestType = null,
                BatchGroup = null,
                TestMethod = null,
                Colour = null,
                Status = null,
                FileName = null,
                Product = new Product { Id = 2, Name = "Product Beta" },
                Machine = new Machine { Id = 2, Name = "Testing Machine Beta" }
            };

            _testResultRepositoryMock.Setup(repo => repo.CreateAsync(newTestResult))
                .ReturnsAsync(createdTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.CreateAsync(newTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(2, result.MachineId);
            Assert.Null(result.OperatorName);
            Assert.Null(result.Speed);
            Assert.Null(result.TestTime);
            Assert.Null(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.CreateAsync(newTestResult), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with extreme measurement values.
        /// </summary>
        [Fact]
        public async Task CreateAsync_TestResultWithExtremeValues_ReturnsCreatedTestResult()
        {
            // Arrange
            var newTestResult = new TestResult
            {
                ProductId = 3,
                MachineId = 3,
                TestDate = new DateTime(2024, 7, 1, 8, 0, 0),
                OperatorName = "Test Operator",
                Speed = 999.99,
                MixerTemp = 500.0,
                StartTemp = -10.0,
                TestTime = 0.1,
                SampleWeight = 0.001,
                MeasRange = 1,
                Damping = 100,
                PlastWeight = 1000.0,
                LoadTime = 0.01,
                LoadSpeed = 0.1,
                Titrate = 50.0,
                Status = false,
                Product = new Product { Id = 3, Name = "Product Gamma" },
                Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
            };

            var createdTestResult = new TestResult
            {
                Id = 9,
                ProductId = 3,
                MachineId = 3,
                TestDate = new DateTime(2024, 7, 1, 8, 0, 0),
                OperatorName = "Test Operator",
                Speed = 999.99,
                MixerTemp = 500.0,
                StartTemp = -10.0,
                TestTime = 0.1,
                SampleWeight = 0.001,
                MeasRange = 1,
                Damping = 100,
                PlastWeight = 1000.0,
                LoadTime = 0.01,
                LoadSpeed = 0.1,
                Titrate = 50.0,
                Status = false,
                Product = new Product { Id = 3, Name = "Product Gamma" },
                Machine = new Machine { Id = 3, Name = "Quality Control Machine" }
            };

            _testResultRepositoryMock.Setup(repo => repo.CreateAsync(newTestResult))
                .ReturnsAsync(createdTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.CreateAsync(newTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Equal(999.99, result.Speed);
            Assert.Equal(500.0, result.MixerTemp);
            Assert.Equal(-10.0, result.StartTemp);
            Assert.Equal(0.1, result.TestTime);
            Assert.False(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.CreateAsync(newTestResult), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing TestResult.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndTestResult_ReturnsUpdatedTestResult()
        {
            // Arrange
            var testResultId = 1;
            var updateTestResult = new TestResult
            {
                ProductId = 5,
                MachineId = 5,
                TestDate = new DateTime(2024, 8, 15, 12, 0, 0),
                OperatorName = "Updated Operator",
                DriveUnit = "Unit-Updated",
                Mixer = "Mixer-Updated",
                LoadingChute = "Chute-Updated",
                Additive = "Add-Updated",
                Speed = 75.5,
                MixerTemp = 95.0,
                StartTemp = 25.0,
                MeasRange = 250,
                Damping = 12,
                TestTime = 400.0,
                SampleWeight = 35.0,
                CodeNumber = "CODE-UPDATED",
                Plasticizer = "PLAST-UPDATED",
                PlastWeight = 8.5,
                LoadTime = 40.0,
                LoadSpeed = 15.0,
                Liquid = "Updated Liquid",
                Titrate = 4.0,
                TestNumber = 10,
                TestType = "Updated Type",
                BatchGroup = "Group-Updated",
                TestMethod = "Method-Updated",
                Colour = "Updated Color",
                Status = false,
                FileName = "test_updated.dat"
            };

            var updatedTestResult = new TestResult
            {
                Id = 1,
                ProductId = 5,
                MachineId = 5,
                TestDate = new DateTime(2024, 8, 15, 12, 0, 0),
                OperatorName = "Updated Operator",
                DriveUnit = "Unit-Updated",
                Mixer = "Mixer-Updated",
                LoadingChute = "Chute-Updated",
                Additive = "Add-Updated",
                Speed = 75.5,
                MixerTemp = 95.0,
                StartTemp = 25.0,
                MeasRange = 250,
                Damping = 12,
                TestTime = 400.0,
                SampleWeight = 35.0,
                CodeNumber = "CODE-UPDATED",
                Plasticizer = "PLAST-UPDATED",
                PlastWeight = 8.5,
                LoadTime = 40.0,
                LoadSpeed = 15.0,
                Liquid = "Updated Liquid",
                Titrate = 4.0,
                TestNumber = 10,
                TestType = "Updated Type",
                BatchGroup = "Group-Updated",
                TestMethod = "Method-Updated",
                Colour = "Updated Color",
                Status = false,
                FileName = "test_updated.dat"
            };

            _testResultRepositoryMock.Setup(repo => repo.UpdateAsync(testResultId, updateTestResult))
                .ReturnsAsync(updatedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.UpdateAsync(testResultId, updateTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(5, result.ProductId);
            Assert.Equal(5, result.MachineId);
            Assert.Equal("Updated Operator", result.OperatorName);
            Assert.Equal("Unit-Updated", result.DriveUnit);
            Assert.Equal(75.5, result.Speed);
            Assert.Equal(400.0, result.TestTime);
            Assert.False(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.UpdateAsync(testResultId, updateTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when TestResult with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateTestResult = new TestResult
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = new DateTime(2024, 9, 1, 10, 0, 0),
                OperatorName = "Test Operator"
            };

            _testResultRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateTestResult))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.UpdateAsync(invalidId, updateTestResult);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only some properties changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedTestResult()
        {
            // Arrange
            var testResultId = 2;
            var updateTestResult = new TestResult
            {
                ProductId = 2, // Same as original
                MachineId = 1, // Same as original
                TestDate = new DateTime(2024, 1, 16, 14, 45, 0), // Same as original
                OperatorName = "Jane Smith Updated", // Changed
                DriveUnit = "Unit-B", // Same as original
                Mixer = "Mixer-002", // Same as original
                Speed = 50.0, // Changed from 45.0 to 50.0
                TestTime = 320.0, // Changed from 280.0 to 320.0
                Status = true, // Changed from false to true
                FileName = "test_002_updated.dat" // Changed
            };

            var updatedTestResult = new TestResult
            {
                Id = 2,
                ProductId = 2,
                MachineId = 1,
                TestDate = new DateTime(2024, 1, 16, 14, 45, 0),
                OperatorName = "Jane Smith Updated",
                DriveUnit = "Unit-B",
                Mixer = "Mixer-002",
                Speed = 50.0,
                TestTime = 320.0,
                Status = true,
                FileName = "test_002_updated.dat"
            };

            _testResultRepositoryMock.Setup(repo => repo.UpdateAsync(testResultId, updateTestResult))
                .ReturnsAsync(updatedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.UpdateAsync(testResultId, updateTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Smith Updated", result.OperatorName);
            Assert.Equal(50.0, result.Speed);
            Assert.Equal(320.0, result.TestTime);
            Assert.True(result.Status);
            Assert.Equal("test_002_updated.dat", result.FileName);
            _testResultRepositoryMock.Verify(repo => repo.UpdateAsync(testResultId, updateTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method updating properties to null values.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdateToNullValues_ReturnsUpdatedTestResult()
        {
            // Arrange
            var testResultId = 3;
            var updateTestResult = new TestResult
            {
                ProductId = 1,
                MachineId = 2,
                TestDate = new DateTime(2024, 2, 10, 9, 15, 0),
                OperatorName = null, // Changed from "Mike Johnson" to null
                DriveUnit = null, // Changed from "Unit-C" to null
                Mixer = null, // Changed from "Mixer-003" to null
                Speed = null, // Changed from 60.0 to null
                TestTime = null, // Changed from 350.2 to null
                Status = null, // Changed from true to null
                FileName = null // Changed from "test_003.dat" to null
            };

            var updatedTestResult = new TestResult
            {
                Id = 3,
                ProductId = 1,
                MachineId = 2,
                TestDate = new DateTime(2024, 2, 10, 9, 15, 0),
                OperatorName = null,
                DriveUnit = null,
                Mixer = null,
                Speed = null,
                TestTime = null,
                Status = null,
                FileName = null
            };

            _testResultRepositoryMock.Setup(repo => repo.UpdateAsync(testResultId, updateTestResult))
                .ReturnsAsync(updatedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.UpdateAsync(testResultId, updateTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.OperatorName);
            Assert.Null(result.DriveUnit);
            Assert.Null(result.Mixer);
            Assert.Null(result.Speed);
            Assert.Null(result.TestTime);
            Assert.Null(result.Status);
            Assert.Null(result.FileName);
            _testResultRepositoryMock.Verify(repo => repo.UpdateAsync(testResultId, updateTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateTestResult = new TestResult
            {
                ProductId = 1,
                MachineId = 1,
                TestDate = new DateTime(2024, 10, 1, 10, 0, 0)
            };

            _testResultRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateTestResult))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.UpdateAsync(zeroId, updateTestResult);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateTestResult), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing TestResult.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedTestResult()
        {
            // Arrange
            var testResultId = 1;
            var deletedTestResult = GetSampleTestResults().First(x => x.Id == testResultId);

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(testResultId))
                .ReturnsAsync(deletedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.DeleteAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Equal("John Doe", result.OperatorName);
            Assert.Equal("Unit-A", result.DriveUnit);
            Assert.Equal(50.5, result.Speed);
            Assert.Equal(300.5, result.TestTime);
            Assert.True(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.DeleteAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when TestResult with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((TestResult?)null);

            // Act
            var result = await _testResultRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _testResultRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with TestResult containing null optional properties.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_TestResultWithNullProperties_ReturnsDeletedTestResult()
        {
            // Arrange
            var testResultId = 4;
            var deletedTestResult = GetSampleTestResults().First(x => x.Id == testResultId);

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(testResultId))
                .ReturnsAsync(deletedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.DeleteAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal(3, result.ProductId);
            Assert.Equal(3, result.MachineId);
            Assert.Null(result.OperatorName);
            Assert.Null(result.DriveUnit);
            Assert.Null(result.Speed);
            Assert.Null(result.TestTime);
            Assert.Null(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.DeleteAsync(testResultId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with inactive TestResult.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InactiveTestResult_ReturnsDeletedTestResult()
        {
            // Arrange
            var testResultId = 2;
            var deletedTestResult = GetSampleTestResults().First(x => x.Id == testResultId);

            _testResultRepositoryMock.Setup(repo => repo.DeleteAsync(testResultId))
                .ReturnsAsync(deletedTestResult);

            // Act
            var result = await _testResultRepositoryMock.Object.DeleteAsync(testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal(2, result.ProductId);
            Assert.Equal(1, result.MachineId);
            Assert.Equal("Jane Smith", result.OperatorName);
            Assert.Equal("Unit-B", result.DriveUnit);
            Assert.Equal(45.0, result.Speed);
            Assert.Equal(280.0, result.TestTime);
            Assert.False(result.Status);
            _testResultRepositoryMock.Verify(repo => repo.DeleteAsync(testResultId), Times.Once);
        }

        #endregion
    }
}


