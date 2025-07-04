﻿using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace ChemsonLab.API.Repositories.CustomerRepository
{
    public class MySQLCustomerRepository : ICustomerRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLCustomerRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new Customer item in the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Customer> CreateAsync(Customer customer)
        {
            try
            {
                await dbContext.Customer.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Customer item.");
            }
        }

        /// <summary>
        /// Deletes a Customer item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Customer?> DeleteAsync(int id)
        {
            try
            {
                var existingCustomer = await dbContext.Customer.FirstOrDefaultAsync(x => x.Id == id);
                if (existingCustomer == null)
                {
                    return null;
                }

                dbContext.Customer.Remove(existingCustomer);
                await dbContext.SaveChangesAsync();
                return existingCustomer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Customer data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Customer items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Customer>> GetAllAsync(string? name = null, string? status = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var customers = dbContext.Customer.AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(name))
                    customers = customers.Where(x => x.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(status) && bool.TryParse(status, out bool boolStatus))
                    customers = customers.Where(x => x.Status == boolStatus);

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                        customers = isAscending ? customers.OrderBy(x => x.Name) : customers.OrderByDescending(x => x.Name);
                }

                return await customers.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the Customer data from database.", ex);
            }
        }

        /// <summary>
        /// Retrieves a Customer item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Customer?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Customer.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Customer data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Customer item in the database by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Customer?> UpdateAsync(int id, Customer customer)
        {
            try
            {
                var existingCustomer = await dbContext.Customer.FirstOrDefaultAsync(x => x.Id == id);
                if (existingCustomer == null)
                {
                    return null;
                }

                existingCustomer.Name = customer.Name;
                existingCustomer.Email = customer.Email;
                existingCustomer.Status = customer.Status;

                await dbContext.SaveChangesAsync();
                return existingCustomer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Customer data ID {id}.", ex);
            }
        }
    }
}
