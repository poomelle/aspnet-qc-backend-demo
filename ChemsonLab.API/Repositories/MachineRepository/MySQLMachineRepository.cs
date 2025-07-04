﻿using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.MachineRepository
{
    public class MySQLMachineRepository : IMachineRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLMachineRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new Machine item in the database.
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Machine> CreateAsync(Machine machine)
        {
            try
            {
                await dbContext.Machine.AddAsync(machine);
                await dbContext.SaveChangesAsync();
                return machine;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Machine item.", ex);
            }
        }

        /// <summary>
        /// Deletes a Machine item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Machine?> DeleteAsync(int id)
        {
            try
            {
                var existingMachine = await dbContext.Machine.FirstOrDefaultAsync(x => x.Id == id);
                if (existingMachine == null)
                {
                    return null;
                }

                dbContext.Machine.Remove(existingMachine);
                await dbContext.SaveChangesAsync();
                return existingMachine;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Machine data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Machine items from the database with optional filtering and sorting.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Machine>> GetAllAsync(string? name = null, string? status = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var machines = dbContext.Machine.AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(name))
                    machines = machines.Where(x => x.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(status) && bool.TryParse(status, out bool boolStatus))
                    machines = machines.Where(x => x.Status == boolStatus);

                // Sorting function
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                        machines = isAscending ? machines.OrderBy(x => x.Name) : machines.OrderByDescending(x => x.Name);
                }

                return await machines.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the Machine data from database.", ex);
            }
        }

        /// <summary>
        /// Retrieves a Machine item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Machine?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Machine.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Machine data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Machine item in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="machine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Machine?> UpdateAsync(int id, Machine machine)
        {
            try
            {
                var existingMachine = await dbContext.Machine.FirstOrDefaultAsync(x => x.Id == id);
                if (existingMachine == null)
                {
                    return null;
                }

                existingMachine.Name = machine.Name;
                existingMachine.Status = machine.Status;
                await dbContext.SaveChangesAsync();

                return existingMachine;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Machine data ID {id}.", ex);
            }
        }
    }
}
