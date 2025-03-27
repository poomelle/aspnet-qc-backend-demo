using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.MachineRepository
{
    public interface IMachineRepository
    {
        Task<List<Machine>> GetAllAsync(string? name = null, string? status = null, string? sortBy = null, bool isAscending = true);
        Task<Machine?> GetByIdAsync(int id);
        Task<Machine> CreateAsync(Machine machine);
        Task<Machine?> UpdateAsync(int id, Machine machine);
        Task<Machine?> DeleteAsync(int id);


    }
}
