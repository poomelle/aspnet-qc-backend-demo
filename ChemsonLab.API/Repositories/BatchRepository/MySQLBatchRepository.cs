using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.BatchRepository
{
    public class MySQLBatchRepository : IBatchRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLBatchRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new Batch item in the database.
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Batch> CreateAsync(Batch batch)
        {
            try
            {
                await dbContext.Batch.AddAsync(batch);
                await dbContext.SaveChangesAsync();
                return batch;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Batch item.", ex);
            }
        }

        /// <summary>
        /// Deletes a Batch item by its ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Batch?> DeleteAsync(int id)
        {
            try
            {
                var existingBatch = await dbContext.Batch.FirstOrDefaultAsync(x => x.Id == id);
                if (existingBatch == null)
                {
                    return null;
                }
                dbContext.Batch.Remove(existingBatch);
                await dbContext.SaveChangesAsync();
                return existingBatch;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Batch data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Batch items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="productName"></param>
        /// <param name="suffix"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Batch>> GetAllAsync(string? batchName = null, string? productName = null, string? suffix = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var batches = dbContext.Batch.Include(x => x.Product).AsQueryable();

                // Filtering function
                if (!string.IsNullOrWhiteSpace(batchName))
                    batches = batches.Where(x => x.BatchName.Contains(batchName));

                if (!string.IsNullOrWhiteSpace(productName))
                    batches = batches.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(suffix))
                    batches = batches.Where(x => x.Suffix.Equals(suffix));

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("batchName", StringComparison.OrdinalIgnoreCase))
                        batches = isAscending ? batches.OrderBy(x => x.BatchName) : batches.OrderByDescending(x => x.BatchName);

                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        batches = isAscending ? batches.OrderBy(x => x.Product.Name) : batches.OrderByDescending(x => x.Product.Name);
                }

                return await batches.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the Batch data.", ex);
            }
        }

        /// <summary>
        /// Retrieves a Batch item by its ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Batch?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Batch.Include(x => x.Product).FirstOrDefaultAsync(x =>x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Batch data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Batch item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Batch?> UpdateAsync(int id, Batch batch)
        {
            try
            {
                var existingBatch = await dbContext.Batch.FirstOrDefaultAsync(y => y.Id == id);
                if (existingBatch == null)
                {
                    return null;
                }

                existingBatch.ProductId = batch.ProductId;
                existingBatch.BatchName = batch.BatchName;
                existingBatch.SampleBy = batch.SampleBy;
                existingBatch.Suffix = batch.Suffix;

                await dbContext.SaveChangesAsync();
                return existingBatch;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Batch data ID {id}.", ex);
            }
        }
    }
}
