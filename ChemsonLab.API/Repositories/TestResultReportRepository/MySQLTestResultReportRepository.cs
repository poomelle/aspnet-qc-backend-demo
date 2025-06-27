using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.TestResultReportRepository
{
    public class MySQLTestResultReportRepository : ITestResultReportRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLTestResultReportRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new TestResultReport item in the database.
        /// </summary>
        /// <param name="resultReport"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResultReport> CreateAsync(TestResultReport resultReport)
        {
            try
            {
                await dbContext.TestResultReport.AddAsync(resultReport);
                await dbContext.SaveChangesAsync();
                return resultReport;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the TestResultReport item.", ex);
            }
        }

        /// <summary>
        /// Deletes a TestResultReport item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResultReport?> DeleteAsync(int id)
        {
            try
            {
                var existingTestResultReport = await dbContext.TestResultReport.FirstOrDefaultAsync(x => x.Id == id);
                if (existingTestResultReport == null)
                {
                    return null;
                }

                dbContext.TestResultReport.Remove(existingTestResultReport);
                await dbContext.SaveChangesAsync();
                return existingTestResultReport;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the TestResulReport data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all TestResultReport items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="createBy"></param>
        /// <param name="createDate"></param>
        /// <param name="productName"></param>
        /// <param name="batchName"></param>
        /// <param name="result"></param>
        /// <param name="batchTestResultId"></param>
        /// <param name="exactBatchName"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<TestResultReport>> GetAllAsync(string? createBy = null, string? createDate = null, string? productName = null, string? batchName = null, string? result = null, string? batchTestResultId = null, string? exactBatchName = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var testResultReports = dbContext.TestResultReport.Include(x => x.Report).Include(x => x.BatchTestResult).ThenInclude(x => x.Batch).Include(x => x.BatchTestResult).ThenInclude(x => x.TestResult).AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(createBy))
                    testResultReports = testResultReports.Where(x => x.Report.CreateBy.Contains(createBy));

                if (!string.IsNullOrWhiteSpace(createDate) && DateTime.TryParse(createDate, out var date))
                    testResultReports = testResultReports.Where(x => x.Report.CreateDate.Date == date.Date);

                if (!string.IsNullOrWhiteSpace(productName))
                    testResultReports = testResultReports.Where(x => x.BatchTestResult.TestResult.Product.Name.Contains(productName));

                if (!string.IsNullOrWhiteSpace(batchName))
                    testResultReports = testResultReports.Where(x => x.BatchTestResult.Batch.BatchName.Contains(batchName));

                if (!string.IsNullOrWhiteSpace(batchTestResultId) && int.TryParse(batchTestResultId, out var batchTestResultIdValue))
                    testResultReports = testResultReports.Where(x => x.BatchTestResultId == batchTestResultIdValue);

                if (!string.IsNullOrWhiteSpace(exactBatchName))
                    testResultReports = testResultReports.Where(x => x.BatchTestResult.Batch.BatchName.Equals(exactBatchName, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(result) && bool.TryParse(result, out var resultValue))
                    testResultReports = testResultReports.Where(x => x.Result == resultValue);

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("createDate", StringComparison.OrdinalIgnoreCase))
                        testResultReports = isAscending ? testResultReports.OrderBy(x => x.Report.CreateDate) : testResultReports.OrderByDescending(x => x.Report.CreateDate);

                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        testResultReports = isAscending ? testResultReports.OrderBy(x => x.BatchTestResult.TestResult.Product.Name) : testResultReports.OrderByDescending(x => x.BatchTestResult.TestResult.Product.Name);

                    if (sortBy.Equals("batchName", StringComparison.OrdinalIgnoreCase))
                        testResultReports = isAscending ? testResultReports.OrderBy(x => x.BatchTestResult.Batch.BatchName) : testResultReports.OrderByDescending(x => x.BatchTestResult.Batch.BatchName);
                }

                return await testResultReports.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the TestResultReport data from database.", ex);
            }
        }

        /// <summary>
        /// Retrieves a TestResultReport item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<TestResultReport?> GetByIdAsync(int id)
        {
            try
            {
                return dbContext.TestResultReport.Include(x => x.Report).Include(x => x.BatchTestResult).ThenInclude(x => x.Batch).Include(x => x.BatchTestResult).ThenInclude(x => x.TestResult).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the TestResultReport data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing TestResultReport item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resultReport"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResultReport?> UpdateAsync(int id, TestResultReport resultReport)
        {
            try
            {
                var existingTestResultReport = await dbContext.TestResultReport.FirstOrDefaultAsync(x => x.Id == id);
                if (existingTestResultReport == null)
                {
                    return null;
                }

                existingTestResultReport.ReportId = resultReport.ReportId;
                existingTestResultReport.BatchTestResultId = resultReport.BatchTestResultId;
                existingTestResultReport.StandardReference = resultReport.StandardReference;
                existingTestResultReport.TorqueDiff = resultReport.TorqueDiff;
                existingTestResultReport.FusionDiff = resultReport.FusionDiff;
                existingTestResultReport.Result = resultReport.Result;

                await dbContext.SaveChangesAsync();
                return existingTestResultReport;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the TestResultReport data ID {id}.", ex);
            }
        }
    }
}
