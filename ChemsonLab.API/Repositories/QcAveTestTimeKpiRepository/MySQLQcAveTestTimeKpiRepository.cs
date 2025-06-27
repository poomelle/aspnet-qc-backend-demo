using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.QcAveTestTimeKpiRepository
{
    public class MySQLQcAveTestTimeKpiRepository : IQcAveTestTimeKpiRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLQcAveTestTimeKpiRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new QcAveTestTimeKpi item in the database.
        /// </summary>
        /// <param name="qcAveTestTimeKpi"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcAveTestTimeKpi> CreateAsync(QcAveTestTimeKpi qcAveTestTimeKpi)
        {
            try
            {
                await dbContext.QcAveTestTimeKpi.AddAsync(qcAveTestTimeKpi);
                await dbContext.SaveChangesAsync();
                return qcAveTestTimeKpi;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the QcAveTestTimeKpi item.", ex);
            }
        }

        /// <summary>
        /// Deletes a QcAveTestTimeKpi item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcAveTestTimeKpi?> DeleteAsync(int id)
        {
            try
            {
                var existingQcAveTestTimeKpi = await dbContext.QcAveTestTimeKpi.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcAveTestTimeKpi == null)
                {
                    return null;
                }
                dbContext.QcAveTestTimeKpi.Remove(existingQcAveTestTimeKpi);
                await dbContext.SaveChangesAsync();
                return existingQcAveTestTimeKpi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the QcAveTestTimeKpi data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all QcAveTestTimeKpi items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="machineName"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<QcAveTestTimeKpi>> GetAllAsync(string? productName = null, string? machineName = null, string? year = null, string? month = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var qcAveTestTimeKpis = dbContext.QcAveTestTimeKpi.Include(x => x.Product).Include(x => x.Machine).AsQueryable();

                if (!string.IsNullOrWhiteSpace(productName))
                    qcAveTestTimeKpis = qcAveTestTimeKpis.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(machineName))
                    qcAveTestTimeKpis = qcAveTestTimeKpis.Where(x => x.Machine.Name.Equals(machineName));

                if (!string.IsNullOrWhiteSpace(year))
                    qcAveTestTimeKpis = qcAveTestTimeKpis.Where(x => x.Year.Equals(year));

                if (!string.IsNullOrWhiteSpace(month))
                    qcAveTestTimeKpis = qcAveTestTimeKpis.Where(x => x.Month.Equals(month));

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        qcAveTestTimeKpis = isAscending ? qcAveTestTimeKpis.OrderBy(x => x.Product.Name) : qcAveTestTimeKpis.OrderByDescending(x => x.Product.Name);

                    if (sortBy.Equals("machineName", StringComparison.OrdinalIgnoreCase))
                        qcAveTestTimeKpis = isAscending ? qcAveTestTimeKpis.OrderBy(x => x.Machine.Name) : qcAveTestTimeKpis.OrderByDescending(x => x.Machine.Name);

                    if (sortBy.Equals("year", StringComparison.OrdinalIgnoreCase))
                        qcAveTestTimeKpis = isAscending ? qcAveTestTimeKpis.OrderBy(x => x.Year) : qcAveTestTimeKpis.OrderByDescending(x => x.Year);

                    if (sortBy.Equals("month", StringComparison.OrdinalIgnoreCase))
                        qcAveTestTimeKpis = isAscending ? qcAveTestTimeKpis.OrderBy(x => x.Month) : qcAveTestTimeKpis.OrderByDescending(x => x.Month);
                }

                return await qcAveTestTimeKpis.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting the QcAveTestTimeKpi data.", ex);
            }

        }

        /// <summary>
        /// Retrieves a QcAveTestTimeKpi item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcAveTestTimeKpi?> GetByIdAsync(int id)
        {
            try
            {
                var existingQcAveTestTimeKpi = await dbContext.QcAveTestTimeKpi.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcAveTestTimeKpi == null)
                {
                    return null;
                }
                return existingQcAveTestTimeKpi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the QcAveTestTimeKpi data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing QcAveTestTimeKpi item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qcAveTestTimeKpi"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QcAveTestTimeKpi?> UpdateAsync(int id, QcAveTestTimeKpi qcAveTestTimeKpi)
        {
            try
            {
                var existingQcAveTestTimeKpi = await dbContext.QcAveTestTimeKpi.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcAveTestTimeKpi == null)
                {
                    return null;
                }

                existingQcAveTestTimeKpi.ProductId = qcAveTestTimeKpi.ProductId;
                existingQcAveTestTimeKpi.MachineId = qcAveTestTimeKpi.MachineId;
                existingQcAveTestTimeKpi.Year = qcAveTestTimeKpi.Year;
                existingQcAveTestTimeKpi.Month = qcAveTestTimeKpi.Month;
                existingQcAveTestTimeKpi.TotalTest = qcAveTestTimeKpi.TotalTest;
                existingQcAveTestTimeKpi.AveTestTime = qcAveTestTimeKpi.AveTestTime;

                await dbContext.SaveChangesAsync();
                return existingQcAveTestTimeKpi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the QcAveTestTimeKpi data ID {id}", ex);
            }
        }
    }
}
