using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.DailyQc;
using ChemsonLab.API.Repositories.DailyQcRepository;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyQcController : ControllerBase
    {
        private readonly IDailyQcRepository dailyQcRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DailyQcController> logger;

        public DailyQcController(IDailyQcRepository dailyQcRepository, IMapper mapper, ILogger<DailyQcController> logger)
        {
            this.dailyQcRepository = dailyQcRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? id, [FromQuery] string? productName, 
            [FromQuery] string? incomingDate, [FromQuery] string? testedDate, [FromQuery] string? testStatus,
            [FromQuery] string? year, [FromQuery] string ? month,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var dailyQcDomain = await dailyQcRepository.GetAllAsync(id, productName, incomingDate, testedDate, testStatus, year, month, sortBy, isAscending);
                return Ok(mapper.Map<List<DailyQcDTO>>(dailyQcDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving DailyQc data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var dailyQcDomain = await dailyQcRepository.GetByIdAsync(id);
                if (dailyQcDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<DailyQcDTO>(dailyQcDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving DailyQc data for ID {id}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDailyQcRequestDTO addDailyQcRequestDTO)
        {
            try
            {
                var dailyQcDomain = mapper.Map<DailyQc>(addDailyQcRequestDTO);
                dailyQcDomain = await dailyQcRepository.CreateAsync(dailyQcDomain);
                var dailyQcDto = mapper.Map<DailyQcDTO>(dailyQcDomain);

                return CreatedAtAction(nameof(GetById), new { id = dailyQcDto.Id }, dailyQcDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the DailyQc data.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDailyQcRequestDTO updateDailyQcRequestDTO)
        {
            try
            {
                var dailyQcDomain = mapper.Map<DailyQc>(updateDailyQcRequestDTO);
                dailyQcDomain = await dailyQcRepository.UpdateAsync(id, dailyQcDomain);
                if (dailyQcDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<DailyQcDTO>(dailyQcDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the DailyQc data for ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var dailyQcDomain = await dailyQcRepository.DeleteAsync(id);
                if (dailyQcDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<DailyQcDTO>(dailyQcDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the DailyQc data for ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
