using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Repositories.ProductSpecificationRepository
{
    public interface IProductSpecificationRepository
    {
        Task<List<ProductSpecification>> GetAllAsync(string? productName = null, string? machineName = null, string? inUse = null, string? sortBy = null, bool isAscending = true);
        Task<ProductSpecification?> GetByIdAsync(int id);
        Task<ProductSpecification> CreateAsync(ProductSpecification productSpecification);
        Task<ProductSpecification?> UpdateAsync(int id, ProductSpecification productSpecification);
        Task<ProductSpecification?> DeleteAsync(int id);
    }
}
