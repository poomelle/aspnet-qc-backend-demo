using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.TestResultReportRepository
{
    public interface ITestResultReportRepository
    {
        Task<List<TestResultReport>> GetAllAsync(string? createBy = null, string? createDate = null, string? productName = null, string? batchName = null, string? result = null, string? batchTestResultId = null, string? exactBatchName = null, string? sortBy = null, bool isAscending = true);
        Task<TestResultReport?> GetByIdAsync(int id);
        Task<TestResultReport> CreateAsync(TestResultReport resultReport);
        Task<TestResultReport?> UpdateAsync(int id, TestResultReport resultReport);
        Task<TestResultReport?> DeleteAsync(int id);
    }
}
