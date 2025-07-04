﻿using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Repositories.EvaluationRepository
{
    public class MySQLEvaluationRepository : IEvaluationRepository
    {
        private readonly ChemsonLabDbContext dbContext;

        public MySQLEvaluationRepository(ChemsonLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new Evaluation item in the database.
        /// </summary>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Evaluation> CreateAsync(Evaluation evaluation)
        {
            try
            {
                await dbContext.Evaluation.AddAsync(evaluation);
                await dbContext.SaveChangesAsync();
                return evaluation;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating the Evaluation item.", ex);
            }
        }

        /// <summary>
        /// Deletes an Evaluation item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Evaluation?> DeleteAsync(int id)
        {
            try
            {
                var existingEvaluation = await dbContext.Evaluation.FirstOrDefaultAsync(x => x.Id == id);
                if (existingEvaluation == null)
                {
                    return null;
                }
                dbContext.Evaluation.Remove(existingEvaluation);
                await dbContext.SaveChangesAsync();
                return existingEvaluation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the Evaluation data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all Evaluation items from the database, with optional filtering by TestResultId and PointName.
        /// </summary>
        /// <param name="testResultId"></param>
        /// <param name="pointName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Evaluation>> GetAllAsync(string? testResultId = null, string? pointName = null)
        {
            try
            {
                var evaluations = dbContext.Evaluation.Include(x => x.TestResult).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Machine).AsQueryable();

                // Filtering function
                if (!string.IsNullOrWhiteSpace(testResultId) && int.TryParse(testResultId, out var resultId))
                    evaluations = evaluations.Where(x => x.TestResultId == resultId);

                if (!string.IsNullOrWhiteSpace(pointName) && char.TryParse(pointName, out var point))
                    evaluations = evaluations.Where(x => x.PointName == point);

                return await evaluations.ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Error retrieving the Evaluation data from database.", ex);
            }
        }

        /// <summary>
        /// Retrieves an Evaluation item by its ID, including related TestResult and Product data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Evaluation?> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Evaluation.Include(x => x.TestResult).ThenInclude(x => x.Product).Include(x => x.TestResult).ThenInclude(x => x.Machine).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving the Evaluation data ID {id}.", ex);
            }
        }

        /// <summary>
        /// Updates an existing Evaluation item in the database by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Evaluation?> UpdateAsync(int id, Evaluation evaluation)
        {
            try
            {
                var existingEvaluation = await dbContext.Evaluation.FirstOrDefaultAsync(x => x.Id == id);
                if (existingEvaluation == null)
                {
                    return null;
                }

                existingEvaluation.TestResultId = evaluation.TestResultId;
                existingEvaluation.Point = evaluation.Point;
                existingEvaluation.PointName = evaluation.PointName;
                existingEvaluation.TimeEval = evaluation.TimeEval;
                existingEvaluation.Torque = evaluation.Torque;
                existingEvaluation.Bandwidth = evaluation.Bandwidth;
                existingEvaluation.StockTemp = evaluation.StockTemp;
                existingEvaluation.Speed = evaluation.Speed;
                existingEvaluation.Energy = evaluation.Energy;
                existingEvaluation.TimeRange = evaluation.TimeRange;
                existingEvaluation.TorqueRange = evaluation.TorqueRange;
                existingEvaluation.TimeEvalInt = evaluation.TimeEvalInt;
                existingEvaluation.TimeRangeInt = evaluation.TimeRangeInt;
                existingEvaluation.FileName = evaluation.FileName;

                await dbContext.SaveChangesAsync();
                return existingEvaluation;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating the Evaluation data ID {id}.", ex);
            }
        }
    }
}
