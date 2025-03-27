using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.QcLabelRepository
{
    public interface IQcLabelRepository
    {
        Task<List<QcLabel>> GetAllAsync(string? batchName = null, string? productName = null, string? printed = null, string? year = null, string? month = null);
        Task<QcLabel?> GetByIdAsync(int id);
        Task<QcLabel> CreateAsync(QcLabel qcLabel);
        Task<QcLabel?> UpdateAsync(int id, QcLabel qcLabel);
        Task<QcLabel?> DeleteAsync(int id);
    }
}
