using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.CustomerOrderRepository
{
    public interface ICustomerOrderRepository
    {
        Task<List<CustomerOrder>> GetAllAsync(string? customerName = null, string? productName = null, string? status = null, string? sortBy = null, bool isAscending = true);
        Task<CustomerOrder?> GetByIdAsync(int id);
        Task<CustomerOrder> CreateAsync(CustomerOrder customerOrder);
        Task<CustomerOrder?> UpdateAsync(int id, CustomerOrder customerOrder);
        Task<CustomerOrder?> DeleteAsync(int id);
    }
}
