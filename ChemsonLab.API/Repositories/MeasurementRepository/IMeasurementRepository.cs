using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.MeasurementRepository
{
    public interface IMeasurementRepository
    {
        Task<List<Measurement>> GetAllAsync(string? testResultId = null);
        Task<Measurement?> GetByIdAsync(int id);
        Task<Measurement> CreateAsync(Measurement measurement);
        Task<Measurement?> UpdateAsync(int id, Measurement measurement);
        Task<Measurement?> DeleteAsync(int id);
    }
}
