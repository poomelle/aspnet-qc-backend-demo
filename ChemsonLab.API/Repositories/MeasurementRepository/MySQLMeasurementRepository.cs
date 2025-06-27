using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.MeasurementRepository
{
    public class MySQLMeasurementRepository : IMeasurementRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLMeasurementRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new Measurement item in the database.
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Measurement> CreateAsync(Measurement measurement)
        {
            try
            {
                await dbContext.Measurement.AddAsync(measurement);
                await dbContext.SaveChangesAsync();
                return measurement;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Measurement item.", ex);
            }
        }

        /// <summary>
        /// Deletes a Measurement item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Measurement?> DeleteAsync(int id)
        {
            try
            {
                var existingMeasurement = await dbContext.Measurement.FirstOrDefaultAsync(x => x.Id == id);
                if (existingMeasurement == null)
                {
                    return null;
                }

                dbContext.Measurement.Remove(existingMeasurement);
                await dbContext.SaveChangesAsync();
                return existingMeasurement;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Measurement data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Measurement items from the database, optionally filtered by TestResultId.
        /// </summary>
        /// <param name="testResultId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Measurement>> GetAllAsync(string? testResultId = null)
        {
            try
            {
                var measurements = dbContext.Measurement.Include(x => x.TestResult).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Machine).AsQueryable();

                // Filtering Function
                if (!string.IsNullOrWhiteSpace(testResultId) && int.TryParse(testResultId, out var resultId))
                    measurements = measurements.Where(x => x.TestResultId == resultId);

                return await measurements.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the Measurement data from database.", ex);
            }
        }

        /// <summary>
        /// Retrieves a Measurement item by its ID, including related TestResult and Product data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Measurement?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Measurement.Include(x => x.TestResult).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Machine).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Measurement data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Measurement item by its ID with the provided data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="measurement"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Measurement?> UpdateAsync(int id, Measurement measurement)
        {
            try
            {
                var existingMeasurement = await dbContext.Measurement.FirstOrDefaultAsync(x => x.Id == id);
                if (existingMeasurement == null)
                {
                    return null;
                }

                existingMeasurement.TestResultId = measurement.TestResultId;
                existingMeasurement.TimeAct = measurement.TimeAct;
                existingMeasurement.Torque = measurement.Torque;
                existingMeasurement.Bandwidth = measurement.Bandwidth;
                existingMeasurement.StockTemp = measurement.StockTemp;
                existingMeasurement.Speed = measurement.Speed;
                existingMeasurement.FileName = measurement.FileName;

                await dbContext.SaveChangesAsync();
                return existingMeasurement;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Measurement data ID {id}.", ex);
            }
        }
    }
}
