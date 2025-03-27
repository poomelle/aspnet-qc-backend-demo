using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.QcAveTestTimeKpiRepository
{
    public interface IQcAveTestTimeKpiRepository
    {
        Task<List<QcAveTestTimeKpi>> GetAllAsync(string? productName = null, string? machineName = null, string? year = null, string? month = null, string? sortBy = null, bool isAscending = true);
        Task<QcAveTestTimeKpi?> GetByIdAsync(int id);
        Task<QcAveTestTimeKpi> CreateAsync(QcAveTestTimeKpi qcAveTestTimeKpi);
        Task<QcAveTestTimeKpi?> UpdateAsync(int id, QcAveTestTimeKpi qcAveTestTimeKpi);
        Task<QcAveTestTimeKpi?> DeleteAsync(int id);
    }
}
