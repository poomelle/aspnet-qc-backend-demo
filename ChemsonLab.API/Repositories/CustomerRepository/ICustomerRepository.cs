using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.CustomerRepository
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync(string? name = null, string? status = null, string? sortBy = null, bool isAscending = true);
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer?> UpdateAsync(int id, Customer customer);
        Task<Customer?> DeleteAsync(int id);
    }
}
