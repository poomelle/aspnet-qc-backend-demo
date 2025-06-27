using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.TestResultRepository
{
    public interface ITestResultRepository
    {
        Task<List<TestResult>> GetAllAsync();
        Task<TestResult?> GetByIdAsync(int id);
        Task<TestResult> CreateAsync(TestResult result);
        Task<TestResult?> UpdateAsync(int id, TestResult result);
        Task<TestResult?> DeleteAsync(int id);
    }
}
