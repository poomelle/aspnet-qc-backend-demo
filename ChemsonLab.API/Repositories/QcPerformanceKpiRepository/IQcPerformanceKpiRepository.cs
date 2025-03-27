using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.QcPerformanceKpiRepository
{
    public interface IQcPerformanceKpiRepository
    {
        Task<List<QcPerformanceKpi>> GetAllAsync(string? productName = null, string? machineName = null, string? year = null, string? month = null, string? sortBy = null, bool isAscending = true);
        Task<QcPerformanceKpi?> GetByIdAsync(int id);
        Task<QcPerformanceKpi> CreateAsync(QcPerformanceKpi qcPerformanceKpi);
        Task<QcPerformanceKpi?> UpdateAsync(int id, QcPerformanceKpi qcPerformanceKpi);
        Task<QcPerformanceKpi?> DeleteAsync(int id);
    }
}
