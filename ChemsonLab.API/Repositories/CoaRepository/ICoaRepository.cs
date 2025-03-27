using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.CoaRepository
{
    public interface ICoaRepository
    {
        Task<List<Coa>> GetAllAsync(string? productName = null, string? batchName = null, string? sortBy = null, bool isAscending = true);
        Task<Coa?> GetByIdAsync(int id);
        Task<Coa> CreateAsync(Coa coa);
        Task<Coa?> UpdateAsync(int id, Coa coa);
        Task<Coa?> DeleteAsync(int id);
    }
}
