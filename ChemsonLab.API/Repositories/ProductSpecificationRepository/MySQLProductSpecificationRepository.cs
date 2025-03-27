using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.ProductSpecificationRepository
{
    public class MySQLProductSpecificationRepository : IProductSpecificationRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLProductSpecificationRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ProductSpecification> CreateAsync(ProductSpecification productSpecification)
        {
            try
            {
                await dbContext.ProductSpecification.AddAsync(productSpecification);
                await dbContext.SaveChangesAsync();
                return productSpecification;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Product Specification item.", ex);
            }
        }

        public async Task<ProductSpecification?> DeleteAsync(int id)
        {
            try
            {
                var existingProductSpecification = await dbContext.ProductSpecification.FirstOrDefaultAsync(x => x.Id == id);
                if (existingProductSpecification == null)
                {
                    return null;
                }
                dbContext.ProductSpecification.Remove(existingProductSpecification);
                await dbContext.SaveChangesAsync();

                return existingProductSpecification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Product Specification data ID {id}.", ex);
            }
        }

        public async Task<List<ProductSpecification>> GetAllAsync(string? productName = null, string? machineName = null, string? inUse = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var productSpecifications = dbContext.ProductSpecification.Include(x => x.Machine).Include(x => x.Product).AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(productName))
                    productSpecifications = productSpecifications.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(machineName))
                    productSpecifications = productSpecifications.Where(x => x.Machine.Name.Contains(machineName));

                if (!string.IsNullOrWhiteSpace(inUse) && bool.TryParse(inUse, out bool inUseValue))
                        productSpecifications = productSpecifications.Where(x => x.InUse == inUseValue);

                // sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        productSpecifications = isAscending ? productSpecifications.OrderBy(x => x.Product.Name) : productSpecifications.OrderByDescending(x => x.Product.Name);

                    if (sortBy.Equals("machineName", StringComparison.OrdinalIgnoreCase))
                        productSpecifications = isAscending ? productSpecifications.OrderBy(x => x.Machine.Name) : productSpecifications.OrderByDescending(x => x.Machine.Name);
                }

                return await productSpecifications.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the Product Specification data from database.", ex);
            }
        }

        public async Task<ProductSpecification?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.ProductSpecification.Include(x => x.Machine).Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Product Specification data ID {id}.", ex);
            }
        }

        public async Task<ProductSpecification?> UpdateAsync(int id, ProductSpecification productSpecification)
        {
            try
            {
                var existingProductSpecification = await dbContext.ProductSpecification.FirstOrDefaultAsync(x => x.Id == id);
                if (existingProductSpecification == null)
                {
                    return null;
                }

                existingProductSpecification.ProductId = productSpecification.ProductId;
                existingProductSpecification.MachineId = productSpecification.MachineId;
                existingProductSpecification.InUse = productSpecification.InUse;
                existingProductSpecification.Temp = productSpecification.Temp;
                existingProductSpecification.Load = productSpecification.Load;
                existingProductSpecification.RPM = productSpecification.RPM;

                await dbContext.SaveChangesAsync();
                return existingProductSpecification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Product Specification data ID {id}.", ex);
            }
        }
    }
}
