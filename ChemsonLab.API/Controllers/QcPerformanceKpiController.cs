using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.QcPerformanceKpi;
using ChemsonLab.API.Repositories.QcPerformanceKpiRepository;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QcPerformanceKpiController : Controller
    {
        private readonly IQcPerformanceKpiRepository qcPerformanceKpiRepository;
        private readonly IMapper mapper;
        private readonly ILogger<QcPerformanceKpiController> logger;

        public QcPerformanceKpiController(IQcPerformanceKpiRepository qcPerformanceKpiRepository, IMapper mapper, ILogger<QcPerformanceKpiController> logger)
        {
            this.qcPerformanceKpiRepository = qcPerformanceKpiRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? productName, [FromQuery] string? machineName, [FromQuery] string? year, [FromQuery] string? month, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var qcPerformanceKpiDomain = await qcPerformanceKpiRepository.GetAllAsync(productName, machineName, year, month, sortBy, isAscending);
                return Ok(mapper.Map<List<QcPerformanceKpiDTO>>(qcPerformanceKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving QcPerformanceKpi data from the database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var qcPerformanceKpiDomain = await qcPerformanceKpiRepository.GetByIdAsync(id);
                if (qcPerformanceKpiDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<QcPerformanceKpiDTO>(qcPerformanceKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving QcPerformanceKpi data for ID {id}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddQcPerformanceKpiRequestDTO addQcPerformanceKpiRequestDTO)
        {
            try
            {
                var qcPerformanceKpiDomain = mapper.Map<QcPerformanceKpi>(addQcPerformanceKpiRequestDTO);
                qcPerformanceKpiDomain =  await qcPerformanceKpiRepository.CreateAsync(qcPerformanceKpiDomain);
                var qcPerformanceKpiDTO = mapper.Map<QcPerformanceKpiDTO>(qcPerformanceKpiDomain);
                return CreatedAtAction(nameof(GetById), new { id = qcPerformanceKpiDTO.Id }, qcPerformanceKpiDTO);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating QcPerformanceKpi data in the database.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateQcPerformanceKpiRequestDTO updateQcPerformanceKpiRequestDTO)
        {
            try
            {
                var qcPerformanceKpiDomain = mapper.Map<QcPerformanceKpi>(updateQcPerformanceKpiRequestDTO);
                qcPerformanceKpiDomain = await qcPerformanceKpiRepository.UpdateAsync(id, qcPerformanceKpiDomain);
                if (qcPerformanceKpiDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<QcPerformanceKpiDTO>(qcPerformanceKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating QcPerformanceKpi data for ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var qcPerformanceKpiDomain = await qcPerformanceKpiRepository.DeleteAsync(id);
                if (qcPerformanceKpiDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<QcPerformanceKpiDTO>(qcPerformanceKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting QcPerformanceKpi data for ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
