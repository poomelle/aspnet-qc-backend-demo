
using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.ProductRepository
{
    public class MySQLProductRepository : IProductRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLProductRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            try
            {
                await dbContext.Product.AddAsync(product);
                await dbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Product item.", ex);
            }
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            try
            {
                var exixtingProduct = await dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);

                if (exixtingProduct == null)
                {
                    return null;
                }

                dbContext.Product.Remove(exixtingProduct);
                await dbContext.SaveChangesAsync();

                return exixtingProduct;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Product data ID {id}.", ex);
            }

        }

        public async Task<List<Product>> GetAllAsync(string? name = null, string? status = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var products = dbContext.Product.AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(name))
                    products = products.Where(x => x.Name.Equals(name));

                if (!string.IsNullOrWhiteSpace(status) && bool.TryParse(status, out bool boolStatus))
                    products = products.Where(x => x.Status == boolStatus);

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                        products = isAscending ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                }

                return await products.ToListAsync();
            }
            catch (Exception ex) 
            { 
                throw new Exception("Error retrieving the Product data from database.", ex); 
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Product data ID {id}.", ex);
            }
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            try
            {
                var existingProduct = await dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);

                if (existingProduct == null)
                {
                    return null;
                }

                existingProduct.Name = product.Name;
                existingProduct.Status = product.Status;
                existingProduct.DBDate = product.DBDate;
                existingProduct.SampleAmount = product.SampleAmount;
                existingProduct.Comment = product.Comment;
                existingProduct.COA = product.COA;
                existingProduct.Colour = product.Colour;
                existingProduct.TorqueWarning = product.TorqueWarning;
                existingProduct.TorqueFail = product.TorqueFail;
                existingProduct.FusionWarning = product.FusionWarning;
                existingProduct.FusionFail = product.FusionFail;
                existingProduct.UpdateDate = product.UpdateDate;
                existingProduct.BulkWeight = product.BulkWeight;
                existingProduct.PaperBagWeight = product.PaperBagWeight;
                existingProduct.PaperBagNo = product.PaperBagNo;
                existingProduct.BatchWeight = product.BatchWeight;
                await dbContext.SaveChangesAsync();

                return existingProduct;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Product data ID {id}.", ex);
            }
        }
    }
}
