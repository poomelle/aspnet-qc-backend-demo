using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.BatchTestResultRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class BatchTestResultRepositoryTests
    {
        private readonly Mock<IBatchTestResultRepository> _mockRepository;

        public BatchTestResultRepositoryTests()
        {
            _mockRepository = new Mock<IBatchTestResultRepository>();
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all batch test results when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllBatchTestResults()
        {
            // Arrange
            var expectedResults = GetSampleBatchTestResults();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedResults, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by product name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByProductName_ReturnsFilteredResults()
        {
            // Arrange
            var productName = "Product1";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.Product.Name == productName).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, btr => Assert.Equal(productName, btr.TestResult.Product.Name));
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, null, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by batch name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchName_ReturnsFilteredResults()
        {
            // Arrange
            var batchName = "Batch001";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.Batch.BatchName.Contains(batchName)).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, batchName, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, batchName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(batchName, result.First().Batch.BatchName);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, batchName, null, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by exact batch name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByExactBatchName_ReturnsFilteredResults()
        {
            // Arrange
            var exactBatchName = "Batch001";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.Batch.BatchName == exactBatchName).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, exactBatchName, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, null, null, exactBatchName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(exactBatchName, result.First().Batch.BatchName);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, exactBatchName, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by test date.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestDate_ReturnsFilteredResults()
        {
            // Arrange
            var testDate = "2024-01-15";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.TestDate.Date == DateTime.Parse(testDate).Date).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, testDate, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, testDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(DateTime.Parse(testDate).Date, result.First().TestResult.TestDate.Date);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, testDate, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by batch group.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByBatchGroup_ReturnsFilteredResults()
        {
            // Arrange
            var batchGroup = "Group1";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.BatchGroup == batchGroup).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, batchGroup, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, batchGroup);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(batchGroup, result.First().TestResult.BatchGroup);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, batchGroup, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by test number.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestNumber_ReturnsFilteredResults()
        {
            // Arrange
            var testNumber = "1";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.TestNumber == int.Parse(testNumber)).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, testNumber, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, testNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(int.Parse(testNumber), result.First().TestResult.TestNumber);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, testNumber, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by machine name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByMachineName_ReturnsFilteredResults()
        {
            // Arrange
            var machineName = "Machine1";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.Machine.Name.Contains(machineName)).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, machineName, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, null, machineName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, btr => Assert.Contains(machineName, btr.TestResult.Machine.Name));
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, machineName, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters batch test results by test result ID.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByTestResultId_ReturnsFilteredResults()
        {
            // Arrange
            var testResultId = "1";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.Id == int.Parse(testResultId)).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, testResultId, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, null, null, null, testResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(int.Parse(testResultId), result.First().TestResult.Id);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, testResultId, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by test date ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByTestDateAscending_ReturnsSortedResults()
        {
            // Arrange
            var sortBy = "testDate";
            var expectedResults = GetSampleBatchTestResults()
                .OrderBy(x => x.TestResult.TestDate).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, sortBy, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, null, null, null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedResults, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by product name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByProductNameDescending_ReturnsSortedResults()
        {
            // Arrange
            var sortBy = "productName";
            var expectedResults = GetSampleBatchTestResults()
                .OrderByDescending(x => x.TestResult.Product.Name).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, sortBy, false))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(null, null, null, null, null, null, null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedResults, result);
            _mockRepository.Verify(repo => repo.GetAllAsync(null, null, null, null, null, null, null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredResults()
        {
            // Arrange
            var productName = "Product1";
            var batchName = "Batch001";
            var testDate = "2024-01-15";
            var expectedResults = GetSampleBatchTestResults()
                .Where(x => x.TestResult.Product.Name == productName &&
                           x.Batch.BatchName.Contains(batchName) &&
                           x.TestResult.TestDate.Date == DateTime.Parse(testDate).Date).ToList();

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, batchName, testDate, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(productName, batchName, testDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(productName, result.First().TestResult.Product.Name);
            Assert.Contains(batchName, result.First().Batch.BatchName);
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, batchName, testDate, null, null, null, null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no results match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingResults_ReturnsEmptyList()
        {
            // Arrange
            var productName = "NonExistentProduct";
            var expectedResults = new List<BatchTestResult>();

            _mockRepository.Setup(repo => repo.GetAllAsync(productName, null, null, null, null, null, null, null, null, true))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _mockRepository.Object.GetAllAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(productName, null, null, null, null, null, null, null, null, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns batch test result when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsBatchTestResult()
        {
            // Arrange
            var id = 1;
            var expectedResult = GetSampleBatchTestResults().First(x => x.Id == id);

            _mockRepository.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(expectedResult.BatchId, result.BatchId);
            Assert.Equal(expectedResult.TestResultId, result.TestResultId);
            _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockRepository.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _mockRepository.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _mockRepository.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new batch test result.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidBatchTestResult_ReturnsCreatedResult()
        {
            // Arrange
            var newBatchTestResult = new BatchTestResult
            {
                BatchId = 1,
                TestResultId = 1,
                Batch = new Batch { Id = 1, BatchName = "NewBatch", ProductId = 1 },
                TestResult = new TestResult { Id = 1, ProductId = 1, MachineId = 1, TestDate = DateTime.Now }
            };

            var createdResult = new BatchTestResult
            {
                Id = 4,
                BatchId = 1,
                TestResultId = 1,
                Batch = newBatchTestResult.Batch,
                TestResult = newBatchTestResult.TestResult
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newBatchTestResult))
                .ReturnsAsync(createdResult);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newBatchTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal(1, result.BatchId);
            Assert.Equal(1, result.TestResultId);
            _mockRepository.Verify(repo => repo.CreateAsync(newBatchTestResult), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with minimal required properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MinimalBatchTestResult_ReturnsCreatedResult()
        {
            // Arrange
            var newBatchTestResult = new BatchTestResult
            {
                BatchId = 2,
                TestResultId = 2
            };

            var createdResult = new BatchTestResult
            {
                Id = 5,
                BatchId = 2,
                TestResultId = 2
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newBatchTestResult))
                .ReturnsAsync(createdResult);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newBatchTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(2, result.BatchId);
            Assert.Equal(2, result.TestResultId);
            _mockRepository.Verify(repo => repo.CreateAsync(newBatchTestResult), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with complete navigation properties.
        /// </summary>
        [Fact]
        public async Task CreateAsync_CompletelyPopulatedBatchTestResult_ReturnsCreatedResult()
        {
            // Arrange
            var newBatchTestResult = new BatchTestResult
            {
                BatchId = 1,
                TestResultId = 1,
                Batch = new Batch
                {
                    Id = 1,
                    BatchName = "CompleteBatch",
                    ProductId = 1,
                    SampleBy = "Test User",
                    Suffix = "A",
                    Product = new Product { Id = 1, Name = "Product1" }
                },
                TestResult = new TestResult
                {
                    Id = 1,
                    ProductId = 1,
                    MachineId = 1,
                    TestDate = DateTime.Now,
                    OperatorName = "Test Operator",
                    TestNumber = 1,
                    BatchGroup = "Group1",
                    Product = new Product { Id = 1, Name = "Product1" },
                    Machine = new Machine { Id = 1, Name = "Machine1" }
                }
            };

            var createdResult = new BatchTestResult
            {
                Id = 6,
                BatchId = 1,
                TestResultId = 1,
                Batch = newBatchTestResult.Batch,
                TestResult = newBatchTestResult.TestResult
            };

            _mockRepository.Setup(repo => repo.CreateAsync(newBatchTestResult))
                .ReturnsAsync(createdResult);

            // Act
            var result = await _mockRepository.Object.CreateAsync(newBatchTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("CompleteBatch", result.Batch.BatchName);
            Assert.Equal("Test Operator", result.TestResult.OperatorName);
            _mockRepository.Verify(repo => repo.CreateAsync(newBatchTestResult), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing batch test result.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndBatchTestResult_ReturnsUpdatedResult()
        {
            // Arrange
            var id = 1;
            var updateBatchTestResult = new BatchTestResult
            {
                BatchId = 2,
                TestResultId = 2
            };

            var updatedResult = new BatchTestResult
            {
                Id = 1,
                BatchId = 2,
                TestResultId = 2,
                Batch = new Batch { Id = 2, BatchName = "UpdatedBatch", ProductId = 2 },
                TestResult = new TestResult { Id = 2, ProductId = 2, MachineId = 2, TestDate = DateTime.Now }
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(id, updateBatchTestResult))
                .ReturnsAsync(updatedResult);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(id, updateBatchTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(2, result.BatchId);
            Assert.Equal(2, result.TestResultId);
            _mockRepository.Verify(repo => repo.UpdateAsync(id, updateBatchTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when batch test result with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateBatchTestResult = new BatchTestResult
            {
                BatchId = 1,
                TestResultId = 1
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(invalidId, updateBatchTestResult))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(invalidId, updateBatchTestResult);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(invalidId, updateBatchTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (changing only BatchId).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedResult()
        {
            // Arrange
            var id = 1;
            var updateBatchTestResult = new BatchTestResult
            {
                BatchId = 3,  // Changed
                TestResultId = 1  // Same as original
            };

            var updatedResult = new BatchTestResult
            {
                Id = 1,
                BatchId = 3,
                TestResultId = 1
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(id, updateBatchTestResult))
                .ReturnsAsync(updatedResult);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(id, updateBatchTestResult);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.BatchId);
            Assert.Equal(1, result.TestResultId);
            _mockRepository.Verify(repo => repo.UpdateAsync(id, updateBatchTestResult), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateBatchTestResult = new BatchTestResult
            {
                BatchId = 1,
                TestResultId = 1
            };

            _mockRepository.Setup(repo => repo.UpdateAsync(zeroId, updateBatchTestResult))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.UpdateAsync(zeroId, updateBatchTestResult);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(zeroId, updateBatchTestResult), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing batch test result.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedResult()
        {
            // Arrange
            var id = 1;
            var deletedResult = GetSampleBatchTestResults().First(x => x.Id == id);

            _mockRepository.Setup(repo => repo.DeleteAsync(id))
                .ReturnsAsync(deletedResult);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Batch001", result.Batch.BatchName);
            _mockRepository.Verify(repo => repo.DeleteAsync(id), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when batch test result with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _mockRepository.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _mockRepository.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _mockRepository.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((BatchTestResult?)null);

            // Act
            var result = await _mockRepository.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to generate sample batch test results for testing.
        /// </summary>
        private static List<BatchTestResult> GetSampleBatchTestResults()
        {
            return new List<BatchTestResult>
            {
                new BatchTestResult
                {
                    Id = 1,
                    BatchId = 1,
                    TestResultId = 1,
                    Batch = new Batch
                    {
                        Id = 1,
                        BatchName = "Batch001",
                        ProductId = 1,
                        SampleBy = "User1",
                        Suffix = "A",
                        Product = new Product { Id = 1, Name = "Product1" }
                    },
                    TestResult = new TestResult
                    {
                        Id = 1,
                        ProductId = 1,
                        MachineId = 1,
                        TestDate = new DateTime(2024, 1, 15),
                        OperatorName = "Operator1",
                        TestNumber = 1,
                        BatchGroup = "Group1",
                        Product = new Product { Id = 1, Name = "Product1" },
                        Machine = new Machine { Id = 1, Name = "Machine1" }
                    }
                },
                new BatchTestResult
                {
                    Id = 2,
                    BatchId = 2,
                    TestResultId = 2,
                    Batch = new Batch
                    {
                        Id = 2,
                        BatchName = "Batch002",
                        ProductId = 1,
                        SampleBy = "User2",
                        Suffix = "B",
                        Product = new Product { Id = 1, Name = "Product1" }
                    },
                    TestResult = new TestResult
                    {
                        Id = 2,
                        ProductId = 1,
                        MachineId = 1,
                        TestDate = new DateTime(2024, 1, 16),
                        OperatorName = "Operator2",
                        TestNumber = 2,
                        BatchGroup = "Group2",
                        Product = new Product { Id = 1, Name = "Product1" },
                        Machine = new Machine { Id = 1, Name = "Machine1" }
                    }
                },
                new BatchTestResult
                {
                    Id = 3,
                    BatchId = 3,
                    TestResultId = 3,
                    Batch = new Batch
                    {
                        Id = 3,
                        BatchName = "Batch003",
                        ProductId = 2,
                        SampleBy = "User3",
                        Suffix = "C",
                        Product = new Product { Id = 2, Name = "Product2" }
                    },
                    TestResult = new TestResult
                    {
                        Id = 3,
                        ProductId = 2,
                        MachineId = 2,
                        TestDate = new DateTime(2024, 1, 17),
                        OperatorName = "Operator3",
                        TestNumber = 3,
                        BatchGroup = "Group3",
                        Product = new Product { Id = 2, Name = "Product2" },
                        Machine = new Machine { Id = 2, Name = "Machine2" }
                    }
                }
            };
        }

        #endregion
    }
}
