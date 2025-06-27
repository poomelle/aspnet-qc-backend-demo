using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.QcPerformanceKpiRepository
{
    public class MySQLQcPerformanceKpiRepository : IQcPerformanceKpiRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLQcPerformanceKpiRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new QcPerformanceKpi item in the database.
        /// </summary>
        /// <param name="qcPerformanceKpi"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcPerformanceKpi> CreateAsync(QcPerformanceKpi qcPerformanceKpi)
        {
            try
            {
                await dbContext.QcPerformanceKpi.AddAsync(qcPerformanceKpi);
                await dbContext.SaveChangesAsync();
                return qcPerformanceKpi;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the QcPerformanceKpi item.", ex);
            }
        }

        /// <summary>
        /// Deletes a QcPerformanceKpi item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcPerformanceKpi?> DeleteAsync(int id)
        {
            try
            {
                var existingQcPerformanceKpi = await dbContext.QcPerformanceKpi.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcPerformanceKpi == null)
                {
                    return null;
                }
                dbContext.QcPerformanceKpi.Remove(existingQcPerformanceKpi);
                await dbContext.SaveChangesAsync();
                return existingQcPerformanceKpi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the QcPerformanceKpi data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all QcPerformanceKpi items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="machineName"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<QcPerformanceKpi>> GetAllAsync(string? productName = null, string? machineName = null, string? year = null, string? month = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var qcPerformanceKpis = dbContext.QcPerformanceKpi.Include(x => x.Product).Include(x => x.Machine).AsQueryable();

                if (!string.IsNullOrWhiteSpace(productName))
                    qcPerformanceKpis = qcPerformanceKpis.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(machineName))
                    qcPerformanceKpis = qcPerformanceKpis.Where(x => x.Machine.Name.Equals(machineName));

                if (!string.IsNullOrWhiteSpace(year))
                    qcPerformanceKpis = qcPerformanceKpis.Where(x => x.Year.Equals(year));

                if (!string.IsNullOrWhiteSpace(month))
                    qcPerformanceKpis = qcPerformanceKpis.Where(x => x.Month.Equals(month));

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        qcPerformanceKpis = isAscending ? qcPerformanceKpis.OrderBy(x => x.Product.Name) : qcPerformanceKpis.OrderByDescending(x => x.Product.Name);

                    if (sortBy.Equals("machineName", StringComparison.OrdinalIgnoreCase))
                        qcPerformanceKpis = isAscending ? qcPerformanceKpis.OrderBy(x => x.Machine.Name) : qcPerformanceKpis.OrderByDescending(x => x.Machine.Name);

                    if (sortBy.Equals("year", StringComparison.OrdinalIgnoreCase))
                        qcPerformanceKpis = isAscending ? qcPerformanceKpis.OrderBy(x => x.Year) : qcPerformanceKpis.OrderByDescending(x => x.Year);

                    if (sortBy.Equals("month", StringComparison.OrdinalIgnoreCase))
                        qcPerformanceKpis = isAscending ? qcPerformanceKpis.OrderBy(x => x.Month) : qcPerformanceKpis.OrderByDescending(x => x.Month);
                }

                return await qcPerformanceKpis.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting the QcPerformanceKpi data.", ex);
            }
        }

        /// <summary>
        /// Retrieves a QcPerformanceKpi item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcPerformanceKpi?> GetByIdAsync(int id)
        {
            try
            {
                var existingQcPerformanceKpi = await dbContext.QcPerformanceKpi.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcPerformanceKpi == null)
                {
                    return null;
                }
                return existingQcPerformanceKpi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the QcPerformanceKpi data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing QcPerformanceKpi item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qcPerformanceKpi"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcPerformanceKpi?> UpdateAsync(int id, QcPerformanceKpi qcPerformanceKpi)
        {
            try
            {
                var existingQcPerformanceKpi = await dbContext.QcPerformanceKpi.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcPerformanceKpi == null)
                {
                    return null;
                }
                existingQcPerformanceKpi.ProductId = qcPerformanceKpi.ProductId;
                existingQcPerformanceKpi.MachineId = qcPerformanceKpi.MachineId;
                existingQcPerformanceKpi.Year = qcPerformanceKpi.Year;
                existingQcPerformanceKpi.Month = qcPerformanceKpi.Month;
                existingQcPerformanceKpi.FirstPass = qcPerformanceKpi.FirstPass;
                existingQcPerformanceKpi.SecondPass = qcPerformanceKpi.SecondPass;
                existingQcPerformanceKpi.ThirdPass = qcPerformanceKpi.ThirdPass;

                await dbContext.SaveChangesAsync();
                return existingQcPerformanceKpi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the QcPerformanceKpi data ID {id}.", ex);
            }
        }
    }
}
