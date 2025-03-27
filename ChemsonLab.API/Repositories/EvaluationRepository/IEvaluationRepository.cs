using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.EvaluationRepository
{
    public interface IEvaluationRepository
    {
        Task<List<Evaluation>> GetAllAsync(string? testResultId = null, string? pointName = null);
        Task<Evaluation?> GetByIdAsync(int id);
        Task<Evaluation> CreateAsync(Evaluation evaluation);
        Task<Evaluation?> UpdateAsync(int id, Evaluation evaluation);
        Task<Evaluation?> DeleteAsync(int id);
    }
}
