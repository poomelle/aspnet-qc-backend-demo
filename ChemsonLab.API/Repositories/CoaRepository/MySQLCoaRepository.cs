﻿using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.CoaRepository
{
    public class MySQLCoaRepository : ICoaRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLCoaRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Coa> CreateAsync(Coa coa)
        {
            try
            {
                await dbContext.Coa.AddAsync(coa);
                await dbContext.SaveChangesAsync();
                return coa;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Coa item.", ex);
            }
        }

        public async Task<Coa?> DeleteAsync(int id)
        {
            try
            {
                var existingCoa = await dbContext.Coa.FirstOrDefaultAsync(x => x.Id == id);
                if (existingCoa == null)
                {
                    return null;
                }
                dbContext.Coa.Remove(existingCoa);
                await dbContext.SaveChangesAsync();
                return existingCoa;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Coa data ID {id}", ex);
            }
        }

        public async Task<List<Coa>> GetAllAsync(string? productName = null, string? batchName = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                var coas = dbContext.Coa.Include(x => x.Product).AsQueryable();

                if (!string.IsNullOrWhiteSpace(productName))
                    coas = coas.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(batchName))
                    coas = coas.Where(x => x.BatchName.Contains(batchName));

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("productName"))
                    {
                        coas = isAscending ? coas.OrderBy(x => x.Product.Name) : coas.OrderByDescending(x => x.Product.Name);
                    }
                    else if (sortBy.Equals("batchName"))
                    {
                        coas = isAscending ? coas.OrderBy(x => x.BatchName) : coas.OrderByDescending(x => x.BatchName);
                    }
                }

                return await coas.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting the Coa data.", ex);
            }
        }

        public async Task<Coa?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Coa.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the Coa data ID {id}", ex);
            }
        }

        public async Task<Coa?> UpdateAsync(int id, Coa coa)
        {
            try
            {
                var existingCoa = await dbContext.Coa.FirstOrDefaultAsync(x => x.Id == id);
                if (existingCoa == null)
                {
                    return null;
                }
                existingCoa.ProductId = coa.ProductId;
                existingCoa.BatchName = coa.BatchName;

                await dbContext.SaveChangesAsync();
                return existingCoa;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Coa data ID {id}", ex);
            }
        }
    }
}
