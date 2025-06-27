using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Repositories.MachineRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemsonLab.API.Tests.RepositoryTests
{
    public class MachineRepositoryTests
    {
        private readonly Mock<IMachineRepository> _machineRepositoryMock;

        public MachineRepositoryTests()
        {
            _machineRepositoryMock = new Mock<IMachineRepository>();
        }

        #region Helper Methods

        /// <summary>
        /// Creates sample Machine data for testing purposes.
        /// </summary>
        /// <returns>List of sample Machine objects</returns>
        private List<Machine> GetSampleMachines()
        {
            return new List<Machine>
            {
                new Machine
                {
                    Id = 1,
                    Name = "Testing Machine Alpha",
                    Status = true
                },
                new Machine
                {
                    Id = 2,
                    Name = "Testing Machine Beta",
                    Status = false
                },
                new Machine
                {
                    Id = 3,
                    Name = "Quality Control Machine",
                    Status = true
                },
                new Machine
                {
                    Id = 4,
                    Name = "Calibration Machine",
                    Status = false
                },
                new Machine
                {
                    Id = 5,
                    Name = "Production Machine Gamma",
                    Status = true
                }
            };
        }

        #endregion

        #region GetAllAsync Tests

        /// <summary>
        /// Test GetAllAsync method returns all Machine items when no filters are applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoFilters_ReturnsAllMachines()
        {
            // Arrange
            var expectedMachines = GetSampleMachines();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedMachines, result);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Machine items by name.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByName_ReturnsFilteredMachines()
        {
            // Arrange
            var name = "Testing";
            var expectedMachines = GetSampleMachines().Where(x => x.Name.Contains(name)).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, machine => Assert.Contains(name, machine.Name));
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Machine items by exact name match.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByExactName_ReturnsFilteredMachines()
        {
            // Arrange
            var name = "Quality Control Machine";
            var expectedMachines = GetSampleMachines().Where(x => x.Name.Contains(name)).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(name, result.First().Name);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Machine items by status (active).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByActiveStatus_ReturnsActiveMachines()
        {
            // Arrange
            var status = "true";
            var expectedMachines = GetSampleMachines().Where(x => x.Status == true).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, status, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, machine => Assert.True(machine.Status));
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method filters Machine items by status (inactive).
        /// </summary>
        [Fact]
        public async Task GetAllAsync_FilterByInactiveStatus_ReturnsInactiveMachines()
        {
            // Arrange
            var status = "false";
            var expectedMachines = GetSampleMachines().Where(x => x.Status == false).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, status, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(null, status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, machine => Assert.False(machine.Status));
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, status, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by name ascending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameAscending_ReturnsSortedMachines()
        {
            // Arrange
            var sortBy = "Name";
            var expectedMachines = GetSampleMachines().OrderBy(x => x.Name).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedMachines, result);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with sorting by name descending.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SortByNameDescending_ReturnsSortedMachines()
        {
            // Arrange
            var sortBy = "Name";
            var expectedMachines = GetSampleMachines().OrderByDescending(x => x.Name).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, false))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(null, null, sortBy, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedMachines, result);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, false), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with multiple filters applied.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_MultipleFilters_ReturnsFilteredMachines()
        {
            // Arrange
            var name = "Machine";
            var status = "true";
            var sortBy = "Name";
            var expectedMachines = GetSampleMachines()
                .Where(x => x.Name.Contains(name) && x.Status == true)
                .OrderBy(x => x.Name).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(name, status, sortBy, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(name, status, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, machine =>
            {
                Assert.Contains(name, machine.Name);
                Assert.True(machine.Status);
            });
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(name, status, sortBy, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method returns empty list when no Machine items match filters.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_NoMatchingMachines_ReturnsEmptyList()
        {
            // Arrange
            var name = "NonExistentMachine";
            var expectedMachines = new List<Machine>();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(name, null, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(name);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(name, null, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with invalid status filter returns empty list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_InvalidStatusFilter_ReturnsEmptyList()
        {
            // Arrange
            var invalidStatus = "invalid";
            var expectedMachines = new List<Machine>();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, invalidStatus, null, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(null, invalidStatus);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, invalidStatus, null, true), Times.Once);
        }

        /// <summary>
        /// Test GetAllAsync method with case-insensitive sorting.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_CaseInsensitiveSorting_ReturnsSortedMachines()
        {
            // Arrange
            var sortBy = "name"; // lowercase
            var expectedMachines = GetSampleMachines().OrderBy(x => x.Name).ToList();

            _machineRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, sortBy, true))
                .ReturnsAsync(expectedMachines);

            // Act
            var result = await _machineRepositoryMock.Object.GetAllAsync(null, null, sortBy, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(expectedMachines, result);
            _machineRepositoryMock.Verify(repo => repo.GetAllAsync(null, null, sortBy, true), Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Test GetByIdAsync method returns Machine when valid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsMachine()
        {
            // Arrange
            var machineId = 1;
            var expectedMachine = GetSampleMachines().First(x => x.Id == machineId);

            _machineRepositoryMock.Setup(repo => repo.GetByIdAsync(machineId))
                .ReturnsAsync(expectedMachine);

            // Act
            var result = await _machineRepositoryMock.Object.GetByIdAsync(machineId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(machineId, result.Id);
            Assert.Equal("Testing Machine Alpha", result.Name);
            Assert.True(result.Status);
            _machineRepositoryMock.Verify(repo => repo.GetByIdAsync(machineId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when invalid ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _machineRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.GetByIdAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _machineRepositoryMock.Setup(repo => repo.GetByIdAsync(zeroId))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.GetByIdAsync(zeroId);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.GetByIdAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test GetByIdAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _machineRepositoryMock.Setup(repo => repo.GetByIdAsync(negativeId))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.GetByIdAsync(negativeId);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.GetByIdAsync(negativeId), Times.Once);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Test CreateAsync method successfully creates a new Machine.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ValidMachine_ReturnsCreatedMachine()
        {
            // Arrange
            var newMachine = new Machine
            {
                Name = "New Testing Machine",
                Status = true
            };

            var createdMachine = new Machine
            {
                Id = 6,
                Name = "New Testing Machine",
                Status = true
            };

            _machineRepositoryMock.Setup(repo => repo.CreateAsync(newMachine))
                .ReturnsAsync(createdMachine);

            // Act
            var result = await _machineRepositoryMock.Object.CreateAsync(newMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Equal("New Testing Machine", result.Name);
            Assert.True(result.Status);
            _machineRepositoryMock.Verify(repo => repo.CreateAsync(newMachine), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method creates Machine with inactive status.
        /// </summary>
        [Fact]
        public async Task CreateAsync_InactiveMachine_ReturnsCreatedMachine()
        {
            // Arrange
            var newMachine = new Machine
            {
                Name = "Inactive Machine",
                Status = false
            };

            var createdMachine = new Machine
            {
                Id = 7,
                Name = "Inactive Machine",
                Status = false
            };

            _machineRepositoryMock.Setup(repo => repo.CreateAsync(newMachine))
                .ReturnsAsync(createdMachine);

            // Act
            var result = await _machineRepositoryMock.Object.CreateAsync(newMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("Inactive Machine", result.Name);
            Assert.False(result.Status);
            _machineRepositoryMock.Verify(repo => repo.CreateAsync(newMachine), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with machine containing special characters in name.
        /// </summary>
        [Fact]
        public async Task CreateAsync_MachineWithSpecialCharacters_ReturnsCreatedMachine()
        {
            // Arrange
            var newMachine = new Machine
            {
                Name = "Test Machine #1 - Alpha/Beta (Version 2.0)",
                Status = true
            };

            var createdMachine = new Machine
            {
                Id = 8,
                Name = "Test Machine #1 - Alpha/Beta (Version 2.0)",
                Status = true
            };

            _machineRepositoryMock.Setup(repo => repo.CreateAsync(newMachine))
                .ReturnsAsync(createdMachine);

            // Act
            var result = await _machineRepositoryMock.Object.CreateAsync(newMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal("Test Machine #1 - Alpha/Beta (Version 2.0)", result.Name);
            Assert.True(result.Status);
            _machineRepositoryMock.Verify(repo => repo.CreateAsync(newMachine), Times.Once);
        }

        /// <summary>
        /// Test CreateAsync method with long machine name.
        /// </summary>
        [Fact]
        public async Task CreateAsync_LongMachineName_ReturnsCreatedMachine()
        {
            // Arrange
            var longName = "Very Long Machine Name That Exceeds Normal Length For Testing Purposes And Validation";
            var newMachine = new Machine
            {
                Name = longName,
                Status = true
            };

            var createdMachine = new Machine
            {
                Id = 9,
                Name = longName,
                Status = true
            };

            _machineRepositoryMock.Setup(repo => repo.CreateAsync(newMachine))
                .ReturnsAsync(createdMachine);

            // Act
            var result = await _machineRepositoryMock.Object.CreateAsync(newMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.Id);
            Assert.Equal(longName, result.Name);
            Assert.True(result.Status);
            _machineRepositoryMock.Verify(repo => repo.CreateAsync(newMachine), Times.Once);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Test UpdateAsync method successfully updates an existing Machine.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ValidIdAndMachine_ReturnsUpdatedMachine()
        {
            // Arrange
            var machineId = 1;
            var updateMachine = new Machine
            {
                Name = "Updated Testing Machine Alpha",
                Status = false
            };

            var updatedMachine = new Machine
            {
                Id = 1,
                Name = "Updated Testing Machine Alpha",
                Status = false
            };

            _machineRepositoryMock.Setup(repo => repo.UpdateAsync(machineId, updateMachine))
                .ReturnsAsync(updatedMachine);

            // Act
            var result = await _machineRepositoryMock.Object.UpdateAsync(machineId, updateMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Testing Machine Alpha", result.Name);
            Assert.False(result.Status);
            _machineRepositoryMock.Verify(repo => repo.UpdateAsync(machineId, updateMachine), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method returns null when Machine with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;
            var updateMachine = new Machine
            {
                Name = "Updated Machine",
                Status = true
            };

            _machineRepositoryMock.Setup(repo => repo.UpdateAsync(invalidId, updateMachine))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.UpdateAsync(invalidId, updateMachine);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.UpdateAsync(invalidId, updateMachine), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with partial update (only name changed).
        /// </summary>
        [Fact]
        public async Task UpdateAsync_PartialUpdate_ReturnsUpdatedMachine()
        {
            // Arrange
            var machineId = 2;
            var updateMachine = new Machine
            {
                Name = "Updated Testing Machine Beta", // Changed
                Status = false // Same as original
            };

            var updatedMachine = new Machine
            {
                Id = 2,
                Name = "Updated Testing Machine Beta",
                Status = false
            };

            _machineRepositoryMock.Setup(repo => repo.UpdateAsync(machineId, updateMachine))
                .ReturnsAsync(updatedMachine);

            // Act
            var result = await _machineRepositoryMock.Object.UpdateAsync(machineId, updateMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Testing Machine Beta", result.Name);
            Assert.False(result.Status);
            _machineRepositoryMock.Verify(repo => repo.UpdateAsync(machineId, updateMachine), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with only status changed.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_StatusOnlyUpdate_ReturnsUpdatedMachine()
        {
            // Arrange
            var machineId = 3;
            var updateMachine = new Machine
            {
                Name = "Quality Control Machine", // Same as original
                Status = false // Changed from true to false
            };

            var updatedMachine = new Machine
            {
                Id = 3,
                Name = "Quality Control Machine",
                Status = false
            };

            _machineRepositoryMock.Setup(repo => repo.UpdateAsync(machineId, updateMachine))
                .ReturnsAsync(updatedMachine);

            // Act
            var result = await _machineRepositoryMock.Object.UpdateAsync(machineId, updateMachine);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Quality Control Machine", result.Name);
            Assert.False(result.Status);
            _machineRepositoryMock.Verify(repo => repo.UpdateAsync(machineId, updateMachine), Times.Once);
        }

        /// <summary>
        /// Test UpdateAsync method with zero ID returns null.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;
            var updateMachine = new Machine
            {
                Name = "Updated Machine",
                Status = true
            };

            _machineRepositoryMock.Setup(repo => repo.UpdateAsync(zeroId, updateMachine))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.UpdateAsync(zeroId, updateMachine);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.UpdateAsync(zeroId, updateMachine), Times.Once);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Test DeleteAsync method successfully deletes an existing Machine.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsDeletedMachine()
        {
            // Arrange
            var machineId = 1;
            var deletedMachine = GetSampleMachines().First(x => x.Id == machineId);

            _machineRepositoryMock.Setup(repo => repo.DeleteAsync(machineId))
                .ReturnsAsync(deletedMachine);

            // Act
            var result = await _machineRepositoryMock.Object.DeleteAsync(machineId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Testing Machine Alpha", result.Name);
            Assert.True(result.Status);
            _machineRepositoryMock.Verify(repo => repo.DeleteAsync(machineId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when Machine with given ID doesn't exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 999;

            _machineRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.DeleteAsync(invalidId);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.DeleteAsync(invalidId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when zero ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            var zeroId = 0;

            _machineRepositoryMock.Setup(repo => repo.DeleteAsync(zeroId))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.DeleteAsync(zeroId);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.DeleteAsync(zeroId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method returns null when negative ID is provided.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            var negativeId = -1;

            _machineRepositoryMock.Setup(repo => repo.DeleteAsync(negativeId))
                .ReturnsAsync((Machine?)null);

            // Act
            var result = await _machineRepositoryMock.Object.DeleteAsync(negativeId);

            // Assert
            Assert.Null(result);
            _machineRepositoryMock.Verify(repo => repo.DeleteAsync(negativeId), Times.Once);
        }

        /// <summary>
        /// Test DeleteAsync method with inactive machine.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_InactiveMachine_ReturnsDeletedMachine()
        {
            // Arrange
            var machineId = 2;
            var deletedMachine = GetSampleMachines().First(x => x.Id == machineId);

            _machineRepositoryMock.Setup(repo => repo.DeleteAsync(machineId))
                .ReturnsAsync(deletedMachine);

            // Act
            var result = await _machineRepositoryMock.Object.DeleteAsync(machineId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("Testing Machine Beta", result.Name);
            Assert.False(result.Status);
            _machineRepositoryMock.Verify(repo => repo.DeleteAsync(machineId), Times.Once);
        }

        #endregion
    }
}


