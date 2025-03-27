using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.BatchTestResultRepository
{
    public interface IBatchTestResultRepository
    {
        Task<List<BatchTestResult>> GetAllAsync(string? productName = null, string? batchName = null, string? testDate = null, string? batchGroup = null, string? testNumber = null, string? machineName = null, string? exactBatchName=null, string? testResultId = null ,string? sortBy = null, bool isAscending = true);
        Task<BatchTestResult?> GetByIdAsync(int id);
        Task<BatchTestResult> CreateAsync(BatchTestResult batchTestResult);
        Task<BatchTestResult?> UpdateAsync(int id, BatchTestResult batchTestResult);
        Task<BatchTestResult?> DeleteAsync(int id);
    }
}
