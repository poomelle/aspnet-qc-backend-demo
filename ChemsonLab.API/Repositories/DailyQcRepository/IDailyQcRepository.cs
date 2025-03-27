using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.DailyQcRepository
{
    public interface IDailyQcRepository
    {
        Task<List<DailyQc>> GetAllAsync(string? id = null, string? productName = null, string? incomingDate = null, string? testedDate = null, string? testStatus = null,
            string? year = null, string? month = null,
            string? sortBy = null, bool isAscending = true);
        Task<DailyQc?> GetByIdAsync(int id);
        Task<DailyQc> CreateAsync(DailyQc dailyQc);
        Task<DailyQc?> UpdateAsync(int id, DailyQc dailyQc);
        Task<DailyQc?> DeleteAsync(int id);
    }
}
