using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChemsonLab.API.Tests.Data
{
    public class ChemsonLabDbContextTests
    {
        private DbContextOptions<ChemsonLabDbContext> GetInMemoryDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ChemsonLabDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public void DbContext_HasAllRequiredDbSets()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("TestDb_VerifyDbSets");

            // Act
            using var context = new ChemsonLabDbContext(options);
            
            // Assert
            Assert.NotNull(context.Product);
            Assert.NotNull(context.Machine);
            Assert.NotNull(context.ProductSpecification);
            Assert.NotNull(context.Customer);
            Assert.NotNull(context.CustomerOrder);
            Assert.NotNull(context.TestResult);
            Assert.NotNull(context.Measurement);
            Assert.NotNull(context.Evaluation);
            Assert.NotNull(context.Batch);
            Assert.NotNull(context.BatchTestResult);
            Assert.NotNull(context.Report);
            Assert.NotNull(context.TestResultReport);
            Assert.NotNull(context.QcLabel);
            Assert.NotNull(context.QcPerformanceKpi);
            Assert.NotNull(context.QcAveTestTimeKpi);
            Assert.NotNull(context.DailyQc);
            Assert.NotNull(context.Coa);
        }
    }
}
