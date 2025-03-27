using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.ReportRepository
{
    public class MySQLReportRepository : IReportRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLReportRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Report> CreateAsync(Report report)
        {
            try
            {
                await dbContext.Report.AddAsync(report);
                await dbContext.SaveChangesAsync();
                return report;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Reprot item.", ex);
            }
        }

        public async Task<Report?> DeleteAsync(int id)
        {
            try
            {
                var existingReport = await dbContext.Report.FirstOrDefaultAsync(x => x.Id == id);
                if (existingReport == null)
                {
                    return null;
                }
                dbContext.Report.Remove(existingReport);
                await dbContext.SaveChangesAsync();

                return existingReport;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Report data ID {id}.", ex);
            }
        }

        public async Task<List<Report>> GetAllAsync(string? createBy = null, string? createDate = null, string? status = null)
        {
            try
            {
                var reports = dbContext.Report.AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(createBy))
                    reports = reports.Where(x => x.CreateBy.Contains(createBy));

                if (!string.IsNullOrWhiteSpace(createDate) && DateTime.TryParse(createDate, out var date))
                    reports = reports.Where(x => x.CreateDate.Date ==  date.Date);

                if (!string.IsNullOrWhiteSpace(status) && bool.TryParse(status, out var statusValue))
                    reports = reports.Where(reports => reports.Status == statusValue);

                return await reports.ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Error retrieving the Report data from database.", ex);
            }
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Report.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Report data ID {id}.", ex);
            }
        }

        public async Task<Report?> UpdateAsync(int id, Report report)
        {
            try
            {
                var existingReport = await dbContext.Report.FirstOrDefaultAsync(x => x.Id == id);
                if(existingReport == null)
                {
                    return null;
                }

                existingReport.CreateDate = report.CreateDate;
                existingReport.CreateBy = report.CreateBy;
                existingReport.Status = report.Status;

                await dbContext.SaveChangesAsync();
                return existingReport;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Report data ID {id}.", ex);
            }
        }
    }
}
