using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.CustomerOrderRepository
{
    public class MySQLCustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLCustomerOrderRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new CustomerOrder item in the database.
        /// </summary>
        /// <param name="customerOrder"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CustomerOrder> CreateAsync(CustomerOrder customerOrder)
        {
            try
            {
                await dbContext.CustomerOrder.AddAsync(customerOrder);
                await dbContext.SaveChangesAsync();
                return customerOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the CustomerOrder item.", ex);
            }
        }

        /// <summary>
        /// Deletes a CustomerOrder item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CustomerOrder?> DeleteAsync(int id)
        {
            try
            {
                var existingCustomerOrder = await dbContext.CustomerOrder.FirstOrDefaultAsync(x => x.Id == id);
                if (existingCustomerOrder == null)
                {
                    return null;
                }
                dbContext.CustomerOrder.Remove(existingCustomerOrder);
                await dbContext.SaveChangesAsync();
                return existingCustomerOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the CustomerOrder data ID {id}", ex);
            }
        }

        /// <summary>
        /// Retrieves all CustomerOrder items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="productName"></param>
        /// <param name="status"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<CustomerOrder>> GetAllAsync(string? customerName = null, string? productName = null, string? status = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var customerOrders = dbContext.CustomerOrder.Include(x => x.Customer).Include(x => x.Product).AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(customerName))
                    customerOrders = customerOrders.Where(x => x.Customer.Name.Contains(customerName));

                if (!string.IsNullOrWhiteSpace(productName))
                    customerOrders = customerOrders.Where(x => x.Product.Name.Contains(productName));

                if (!string.IsNullOrWhiteSpace(status) && bool.TryParse(status, out bool statusBool))
                    customerOrders = customerOrders.Where(x => x.Status == statusBool);

                // sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("customerName", StringComparison.OrdinalIgnoreCase))
                        customerOrders = isAscending ? customerOrders.OrderBy(x => x.Customer.Name) : customerOrders.OrderByDescending(x => x.Customer.Name);

                    if (sortBy.Equals("productName", StringComparison.OrdinalIgnoreCase))
                        customerOrders = isAscending ? customerOrders.OrderBy(x => x.Product.Name) : customerOrders.OrderByDescending(x => x.Product.Name);
                }

                return await customerOrders.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the CustomerOrder data from database.", ex);
            }
        }

        /// <summary>
        /// Retrieves a CustomerOrder item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CustomerOrder?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.CustomerOrder.Include(x => x.Customer).Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the CustomerOrder data ID {id}.", ex);
            }
        }

        public async Task<CustomerOrder?> UpdateAsync(int id, CustomerOrder customerOrder)
        {
            try
            {
                var existingCustomerOrder = await dbContext.CustomerOrder.FirstOrDefaultAsync(y => y.Id == id);
                if (existingCustomerOrder == null)
                {
                    return null;
                }
                existingCustomerOrder.CustomerId = customerOrder.CustomerId;
                existingCustomerOrder.ProductId = customerOrder.ProductId;
                existingCustomerOrder.Status = customerOrder.Status;

                await dbContext.SaveChangesAsync();
                return existingCustomerOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the CustomerOrder data ID {id}.", ex);
            }
        }
    }
}
