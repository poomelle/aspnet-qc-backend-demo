﻿using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.QcLabelRepository
{
    public class MySQLQcLabelRepository : IQcLabelRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLQcLabelRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<QcLabel> CreateAsync(QcLabel qcLabel)
        {
            try
            {
                await dbContext.QcLabel.AddAsync(qcLabel);
                await dbContext.SaveChangesAsync();
                return qcLabel;

            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the QcLabel item.", ex);
            }
        }

        public async Task<QcLabel?> DeleteAsync(int id)
        {
            try
            {
                var existingQcLabel = await dbContext.QcLabel.FirstOrDefaultAsync(x => x.Id == id);
                if (existingQcLabel == null)
                {
                    return null;
                }
                dbContext.QcLabel.Remove(existingQcLabel);
                await dbContext.SaveChangesAsync();

                return existingQcLabel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the QcLabel data ID {id}.", ex);
            }
        }

        public async Task<List<QcLabel>> GetAllAsync(string? batchName = null, string? productName = null, string? printed = null, string? year = null, string? month = null)
        {
            try
            {
                var qcLabels = dbContext.QcLabel.Include(x => x.Product).AsQueryable();

                // filtering function
                if (!string.IsNullOrWhiteSpace(productName))
                    qcLabels = qcLabels.Where(x => x.Product.Name.Equals(productName));

                if (!string.IsNullOrWhiteSpace(batchName))
                    qcLabels = qcLabels.Where(x => x.BatchName.Equals(batchName));


                if (!string.IsNullOrWhiteSpace(printed) && bool.TryParse(printed, out var isPrinted))
                    qcLabels = qcLabels.Where(x => x.Printed == isPrinted);

                if (!string.IsNullOrWhiteSpace(year))
                    qcLabels = qcLabels.Where(x => x.Year.Equals(year));

                if (!string.IsNullOrWhiteSpace(month))
                    qcLabels = qcLabels.Where(x => x.Month.Equals(month));

                return await qcLabels.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting the QcLabel data.", ex);
            }
        }

        public async Task<QcLabel?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.QcLabel.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the QcLabel data ID {id}.", ex);
            }
        }

        public async Task<QcLabel?> UpdateAsync(int id, QcLabel qcLabel)
        {
            try
            {
                var existingQcLabel = await dbContext.QcLabel.FirstOrDefaultAsync(x => x.Id == id);

                if (existingQcLabel == null)
                {
                    return null;
                }

                existingQcLabel.ProductId = qcLabel.ProductId;
                existingQcLabel.BatchName = qcLabel.BatchName;
                existingQcLabel.Printed = qcLabel.Printed;

                await dbContext.SaveChangesAsync();
                return existingQcLabel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the QcLabel data ID {id}.", ex);
            }
        }
    }
}
