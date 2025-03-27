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
