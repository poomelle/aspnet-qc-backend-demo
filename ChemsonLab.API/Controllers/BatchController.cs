using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Batch;
using ChemsonLab.API.Repositories.BatchRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatchRepository batchRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BatchController> logger;

        public BatchController(IBatchRepository batchRepository, IMapper mapper, ILogger<BatchController> logger)
        {
            this.batchRepository = batchRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? batchName, [FromQuery] string? productName, [FromQuery] string? suffix, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var batchDomain = await batchRepository.GetAllAsync(batchName, productName, suffix, sortBy, isAscending);
                return Ok(mapper.Map<List<BatchDTO>>(batchDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving batch data from the database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var batchDomain = await batchRepository.GetByIdAsync(id);
                if (batchDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<BatchDTO>(batchDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving batch data for ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddBatchRequestDTO addBatchRequestDTO)
        {
            try
            {
                var batchDomain = mapper.Map<Batch>(addBatchRequestDTO);
                batchDomain = await batchRepository.CreateAsync(batchDomain);
                var batchDto = mapper.Map<BatchDTO>(batchDomain);
                return CreatedAtAction(nameof(GetById), new { id  = batchDto.Id }, batchDto);
            }
            catch(Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the batch item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBatchRequestDTO updateBatchRequestDTO)
        {
            try
            {
                var batchDomain = mapper.Map<Batch>(updateBatchRequestDTO);
                batchDomain = await batchRepository.UpdateAsync(id, batchDomain);
                if (batchDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<BatchDTO>(batchDomain));
            }
            catch(Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the batch item with ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var batchDomain = await batchRepository.DeleteAsync(id);
                if (batchDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<BatchDTO>(batchDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the batch item with ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
