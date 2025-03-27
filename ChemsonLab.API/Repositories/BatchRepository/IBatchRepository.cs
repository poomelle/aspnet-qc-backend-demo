using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.BatchRepository
{
    public interface IBatchRepository
    {
        Task<List<Batch>> GetAllAsync(string? batchName = null, string? productName = null, string? suffix = null, string? sortBy = null, bool isAscending = true);
        Task<Batch?> GetByIdAsync(int id);
        Task<Batch> CreateAsync(Batch batch);
        Task<Batch?> UpdateAsync(int id, Batch batch);
        Task<Batch?> DeleteAsync(int id);
    }
}
