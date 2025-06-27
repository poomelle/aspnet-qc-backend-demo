using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.TestResultRepository
{
    public class MySQLTestResultRepository : ITestResultRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLTestResultRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new TestResult item in the database.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResult> CreateAsync(TestResult result)
        {
            try
            {
                await dbContext.TestResult.AddAsync(result);
                await dbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the TestResult item.", ex);
            }
        }

        /// <summary>
        /// Deletes a TestResult item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResult?> DeleteAsync(int id)
        {
            try
            {
                var existingTestResult = await dbContext.TestResult.FirstOrDefaultAsync(x => x.Id == id);
                if (existingTestResult == null)
                {
                    return null;
                }
                dbContext.TestResult.Remove(existingTestResult);
                await dbContext.SaveChangesAsync();
                return existingTestResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the TestResult data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all TestResult items from the database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<TestResult>> GetAllAsync()
        {
            try
            {
                return await dbContext.TestResult.Include(x => x.Product).Include(x => x.Machine).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the TestResult data from database.");
            }
        }

        /// <summary>
        /// Retrieves a TestResult item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResult?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.TestResult.Include(x => x.Product).Include(x => x.Machine).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the TestResult data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing TestResult item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TestResult?> UpdateAsync(int id, TestResult result)
        {
            try
            {
                var existingTestResult = await dbContext.TestResult.FirstOrDefaultAsync(x => x.Id == id);
                if (existingTestResult == null)
                {
                    return null;
                }

                existingTestResult.ProductId = result.ProductId;
                existingTestResult.MachineId = result.MachineId;
                existingTestResult.TestDate = result.TestDate;
                existingTestResult.OperatorName = result.OperatorName;
                existingTestResult.DriveUnit = result.DriveUnit;
                existingTestResult.Mixer = result.Mixer;
                existingTestResult.LoadingChute = result.LoadingChute;
                existingTestResult.Additive = result.Additive;
                existingTestResult.Speed = result.Speed;
                existingTestResult.MixerTemp = result.MixerTemp;
                existingTestResult.StartTemp = result.StartTemp;
                existingTestResult.MeasRange = result.MeasRange;
                existingTestResult.Damping = result.Damping;
                existingTestResult.TestTime = result.TestTime;
                existingTestResult.SampleWeight = result.SampleWeight;
                existingTestResult.CodeNumber = result.CodeNumber;
                existingTestResult.Plasticizer = result.Plasticizer;
                existingTestResult.PlastWeight = result.PlastWeight;
                existingTestResult.LoadTime = result.LoadTime;
                existingTestResult.LoadSpeed = result.LoadSpeed;
                existingTestResult.Liquid = result.Liquid;
                existingTestResult.Titrate = result.Titrate;
                existingTestResult.TestNumber = result.TestNumber;
                existingTestResult.TestType = result.TestType;
                existingTestResult.BatchGroup = result.BatchGroup;
                existingTestResult.TestMethod = result.TestMethod;
                existingTestResult.Colour = result.Colour;
                existingTestResult.Status = result.Status;

                await dbContext.SaveChangesAsync();
                return existingTestResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the TestResult data ID {id}.", ex);
            }
        }
    }
}
