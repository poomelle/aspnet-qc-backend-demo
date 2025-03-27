using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.ReportRepository
{
    public interface IReportRepository
    {
        Task<List<Report>> GetAllAsync(string? createBy = null, string? createDate = null, string? status = null);
        Task<Report?> GetByIdAsync(int id);
        Task<Report> CreateAsync(Report report);
        Task<Report?> UpdateAsync(int id, Report report);
        Task<Report?> DeleteAsync(int id);
    }
}
