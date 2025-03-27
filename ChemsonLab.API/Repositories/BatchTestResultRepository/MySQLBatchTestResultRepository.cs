using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.BatchTestResultRepository
{
    public class MySQLBatchTestResultRepository : IBatchTestResultRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLBatchTestResultRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BatchTestResult> CreateAsync(BatchTestResult batchTestResult)
        {
            try
            {
                await dbContext.BatchTestResult.AddAsync(batchTestResult);
                await dbContext.SaveChangesAsync();
                return batchTestResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the BatchTestResult item.", ex);
            }
        }

        public async Task<BatchTestResult?> DeleteAsync(int id)
        {
            try
            {
                var existingBatchTestResult = await dbContext.BatchTestResult.FirstOrDefaultAsync(x => x.Id == id);
                if (existingBatchTestResult == null)
                {
                    return null;
                }
                dbContext.BatchTestResult.Remove(existingBatchTestResult);
                await dbContext.SaveChangesAsync();
                return existingBatchTestResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the BatchTestResult data ID {id}.", ex);
            }
        }

        public async Task<List<BatchTestResult>> GetAllAsync(string? productName = null, string? batchName = null, string? testDate = null, string? batchGroup = null, string? testNumber = null, string? machineName = null, string? exactBatchName = null, string? testResultId = null ,string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var batchTestResults = dbContext.BatchTestResult.Include(x => x.Batch).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Machine).AsQueryable();

                // Filter function
                if (!string.IsNullOrWhiteSpace(productName))
                    batchTestResults = batchTestResults.Where(x => x.TestResult.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(batchName))
                    batchTestResults = batchTestResults.Where(x => x.Batch.BatchName.Contains(batchName));

                if (!string.IsNullOrWhiteSpace(exactBatchName))
                    batchTestResults = batchTestResults.Where(x => x.Batch.BatchName.Equals(exactBatchName));

                if (!string.IsNullOrWhiteSpace(testDate) && DateTime.TryParse(testDate, out var date))
                    batchTestResults = batchTestResults.Where(x => x.TestResult.TestDate.Date.Equals(date.Date));

                if (!string.IsNullOrWhiteSpace(batchGroup))
                    batchTestResults = batchTestResults.Where(x => x.TestResult.BatchGroup.Equals(batchGroup));

                if (!string.IsNullOrWhiteSpace(testNumber) && int.TryParse(testNumber, out var number))
                    batchTestResults = batchTestResults.Where(x => x.TestResult.TestNumber.Equals(number));

                if (!string.IsNullOrWhiteSpace(testResultId) && int.TryParse(testResultId, out var resultId))
                    batchTestResults = batchTestResults.Where(x => x.TestResult.Id.Equals(resultId));

                if (!string.IsNullOrWhiteSpace(machineName))
                    batchTestResults = batchTestResults.Where(x => x.TestResult.Machine.Name.Contains(machineName));

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("testDate", StringComparison.OrdinalIgnoreCase))
                        batchTestResults = isAscending ? batchTestResults.OrderBy(x => x.TestResult.TestDate) : batchTestResults.OrderByDescending(x => x.TestResult.TestDate);

                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        batchTestResults = isAscending ? batchTestResults.OrderBy(x => x.TestResult.Product.Name) : batchTestResults.OrderByDescending(x => x.TestResult.Product.Name);

                    if (sortBy.Equals("batchName", StringComparison.OrdinalIgnoreCase))
                        batchTestResults = isAscending ? batchTestResults.OrderBy(x => x.Batch.BatchName) : batchTestResults.OrderByDescending(x => x.Batch.BatchName);

                    if (sortBy.Equals("testNumber", StringComparison.OrdinalIgnoreCase))
                        batchTestResults = isAscending ? batchTestResults.OrderBy(x => x.TestResult.TestNumber) : batchTestResults.OrderByDescending(x => x.TestResult.TestNumber);
                }

                return await batchTestResults.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the BatchTestResult data from database.", ex);
            }
        }

        public async Task<BatchTestResult?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.BatchTestResult.Include(x => x.Batch).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Machine).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the BatchTestResult data ID {id}.", ex);
            }
        }

        public async Task<BatchTestResult?> UpdateAsync(int id, BatchTestResult batchTestResult)
        {
            try
            {
                var existingBatchTestResult = await dbContext.BatchTestResult.FirstOrDefaultAsync(x => x.Id == id);
                if (existingBatchTestResult == null)
                {
                    return null;
                }

                existingBatchTestResult.BatchId = batchTestResult.BatchId;
                existingBatchTestResult.TestResultId = batchTestResult.TestResultId;
                await dbContext.SaveChangesAsync();
                return existingBatchTestResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the BatchTestResult data ID {id}", ex);
            }
        }
    }
}
