using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.DailyQcRepository
{
    public class MySQLDailyQcRepository : IDailyQcRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLDailyQcRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<DailyQc> CreateAsync(DailyQc dailyQc)
        {
            try
            {
                await dbContext.DailyQc.AddAsync(dailyQc);
                await dbContext.SaveChangesAsync();
                return dailyQc;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the DailyQc item.", ex);
            }
        }

        public async Task<DailyQc?> DeleteAsync(int id)
        {
            try
            {
                var existingDailyQc = await dbContext.DailyQc.FirstOrDefaultAsync(x => x.Id == id);
                if (existingDailyQc == null)
                {
                    return null;
                }
                dbContext.DailyQc.Remove(existingDailyQc);
                await dbContext.SaveChangesAsync();
                return existingDailyQc;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the DailyQc data ID {id}.", ex);
            }
        }

        public async Task<List<DailyQc>> GetAllAsync(string? id = null, string? productName = null, string? incomingDate = null, string? testedDate = null, string? testStatus = null,
            string? year = null, string? month = null,
            string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var dailyQcs = dbContext.DailyQc.Include(x => x.Product).AsQueryable();

                // Filtering function
                if (!string.IsNullOrWhiteSpace(id) && int.TryParse(id, out var dailyQcId))
                    dailyQcs = dailyQcs.Where(x => x.Id == dailyQcId);

                if (!string.IsNullOrWhiteSpace(productName))
                    dailyQcs = dailyQcs.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(incomingDate) && DateTime.TryParse(incomingDate, out var date))
                    dailyQcs = dailyQcs.Where(x => x.IncomingDate == date);

                if (!string.IsNullOrWhiteSpace(testedDate) && DateTime.TryParse(testedDate, out var testDate))
                    dailyQcs = dailyQcs.Where(x => x.TestedDate == testDate);

                if (!string.IsNullOrWhiteSpace(testStatus))
                    dailyQcs = dailyQcs.Where(x => x.TestStatus.Equals(testStatus));

                if (!string.IsNullOrWhiteSpace(month))
                    dailyQcs = dailyQcs.Where(x => x.Month.Equals(month));

                if (!string.IsNullOrWhiteSpace(year))
                    dailyQcs = dailyQcs.Where(x => x.Year.Equals(year));

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        dailyQcs = isAscending ? dailyQcs.OrderBy(x => x.Product.Name) : dailyQcs.OrderByDescending(x => x.Product.Name);

                    if (sortBy.Equals("incomingDate", StringComparison.OrdinalIgnoreCase))
                        dailyQcs = isAscending ? dailyQcs.OrderBy(x => x.IncomingDate) : dailyQcs.OrderByDescending(x => x.IncomingDate);

                    if (sortBy.Equals("testedDate", StringComparison.OrdinalIgnoreCase))
                        dailyQcs = isAscending ? dailyQcs.OrderBy(x => x.TestedDate) : dailyQcs.OrderByDescending(x => x.TestedDate);

                    if (sortBy.Equals("testStatus", StringComparison.OrdinalIgnoreCase))
                        dailyQcs = isAscending ? dailyQcs.OrderBy(x => x.TestStatus) : dailyQcs.OrderByDescending(x => x.TestStatus);
                }

                return await dailyQcs.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting the DailyQc data from database.", ex);
            }
        }

        public async Task<DailyQc?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.DailyQc.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the DailyQc data ID {id}.", ex);
            }
        }

        public async Task<DailyQc?> UpdateAsync(int id, DailyQc dailyQc)
        {
            try
            {
                var existingDailyQc = await dbContext.DailyQc.FirstOrDefaultAsync(x => x.Id == id);
                if (existingDailyQc == null)
                {
                    return null;
                }
                existingDailyQc.ProductId = dailyQc.ProductId;
                existingDailyQc.IncomingDate = dailyQc.IncomingDate;
                existingDailyQc.Priority = dailyQc.Priority;
                existingDailyQc.Comment = dailyQc.Comment;
                existingDailyQc.Batches = dailyQc.Batches;
                existingDailyQc.StdReqd = dailyQc.StdReqd;
                existingDailyQc.Extras = dailyQc.Extras;
                existingDailyQc.MixesReqd = dailyQc.MixesReqd;
                existingDailyQc.Mixed = dailyQc.Mixed;
                existingDailyQc.TestStatus = dailyQc.TestStatus;
                existingDailyQc.LastLabel = dailyQc.LastLabel;
                existingDailyQc.LastBatch = dailyQc.LastBatch;
                existingDailyQc.TestedDate = dailyQc.TestedDate;
                existingDailyQc.Year = dailyQc.Year;
                existingDailyQc.Month = dailyQc.Month;
                await dbContext.SaveChangesAsync();

                return existingDailyQc;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the DailyQc data ID {id}.", ex);
            }
        }
    }
}
