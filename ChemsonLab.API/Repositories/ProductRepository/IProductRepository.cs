using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(string? name = null, string? status = null, string? sortBy = null, bool isAscending = true);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(int id, Product product);
        Task<Product?> DeleteAsync(int id);

    }
}
