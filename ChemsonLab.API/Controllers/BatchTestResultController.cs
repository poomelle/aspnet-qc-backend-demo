using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.BatchTestResult;
using ChemsonLab.API.Repositories.BatchTestResultRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchTestResultController : ControllerBase
    {
        private readonly IBatchTestResultRepository batchTestResultRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BatchTestResultController> logger;

        public BatchTestResultController(IBatchTestResultRepository batchTestResultRepository, IMapper mapper, ILogger<BatchTestResultController> logger)
        {
            this.batchTestResultRepository = batchTestResultRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all BatchTestResult items with optional filtering and sorting parameters.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="batchName"></param>
        /// <param name="testDate"></param>
        /// <param name="batchGroup"></param>
        /// <param name="testNumber"></param>
        /// <param name="machineName"></param>
        /// <param name="exactBatchName"></param>
        /// <param name="testResultId"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? productName, [FromQuery] string? batchName, [FromQuery] string? testDate, [FromQuery] string? batchGroup, [FromQuery] string? testNumber, [FromQuery] string? machineName, [FromQuery] string? exactBatchName, [FromQuery] string? testResultId, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var batchTestResultDomain = await batchTestResultRepository.GetAllAsync(productName, batchName, testDate, batchGroup, testNumber, machineName, exactBatchName, testResultId, sortBy, isAscending);
                return Ok(mapper.Map<List<BatchTestResultDTO>>(batchTestResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving BatchTestResult data from database.");
            }
        }

        /// <summary>
        /// Retrieves a BatchTestResult item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var batchTestResultDomain = await batchTestResultRepository.GetByIdAsync(id);
                if (batchTestResultDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<BatchTestResultDTO>(batchTestResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving BatchTestResult data for ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new BatchTestResult item.
        /// </summary>
        /// <param name="addBatchTestResultRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddBatchTestResultRequestDTO addBatchTestResultRequestDTO)
        {
            try
            {
                var batchTestResultDomain = mapper.Map<BatchTestResult>(addBatchTestResultRequestDTO);
                batchTestResultDomain = await batchTestResultRepository.CreateAsync(batchTestResultDomain);
                var batchTestResultDto = mapper.Map<BatchTestResultDTO>(batchTestResultDomain);
                return CreatedAtAction(nameof(GetById), new {id = batchTestResultDto.Id}, batchTestResultDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the BatchTestResult item.");
            }
        }

        /// <summary>
        /// Updates an existing BatchTestResult item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateBatchTestResultRequestDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBatchTestResultRequestDTO updateBatchTestResultRequestDTO)
        {
            try
            {
                var batchTestResultDomain = mapper.Map<BatchTestResult>(updateBatchTestResultRequestDTO);
                batchTestResultDomain = await batchTestResultRepository.UpdateAsync(id, batchTestResultDomain);
                if (batchTestResultDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<BatchTestResultDTO>(batchTestResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the BatchTestResult item ID {id}.");
            }
        }

        /// <summary>
        /// Deletes a BatchTestResult item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var batchTestResultDomain = await batchTestResultRepository.DeleteAsync(id);
                if (batchTestResultDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<BatchTestResultDTO>(batchTestResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the BatchTestResult item ID {id}.");
            }
        }
        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
