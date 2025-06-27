using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.QcAveTestTimeKpi;
using ChemsonLab.API.Repositories.QcAveTestTimeKpiRepository;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QcAveTestTimeKpiController : Controller
    {
        private readonly IQcAveTestTimeKpiRepository qcAveTestTimeKpiRepository;
        private readonly IMapper mapper;
        private readonly ILogger<QcAveTestTimeKpiController> logger;

        public QcAveTestTimeKpiController(IQcAveTestTimeKpiRepository qcAveTestTimeKpiRepository, IMapper mapper, ILogger<QcAveTestTimeKpiController> logger)
        {
            this.qcAveTestTimeKpiRepository = qcAveTestTimeKpiRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all QcAveTestTimeKpi items with optional filtering and sorting parameters.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="machineName"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? productName, [FromQuery] string? machineName, [FromQuery] string? year, [FromQuery] string? month, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var qcAveTestTimeKpiDomain = await qcAveTestTimeKpiRepository.GetAllAsync(productName, machineName, year, month, sortBy, isAscending);
                return Ok(mapper.Map<List<QcAveTestTimeKpiDTO>>(qcAveTestTimeKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving QcAveTestTimeKpi data from the database.");
            }
        }

        /// <summary>
        /// Retrieves a QcAveTestTimeKpi item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var qcAveTestTimeKpiDomain = await qcAveTestTimeKpiRepository.GetByIdAsync(id);
                if (qcAveTestTimeKpiDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<QcAveTestTimeKpiDTO>(qcAveTestTimeKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving QcAveTestTimeKpi data for ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new QcAveTestTimeKpi item in the database.
        /// </summary>
        /// <param name="addQcAveTestTimeKpiRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddQcAveTestTimeKpiRequestDTO addQcAveTestTimeKpiRequestDTO)
        {
            try
            {
                var qcAveTestTimeKpiDomain = mapper.Map<QcAveTestTimeKpi>(addQcAveTestTimeKpiRequestDTO);
                qcAveTestTimeKpiDomain = await qcAveTestTimeKpiRepository.CreateAsync(qcAveTestTimeKpiDomain);
                var qcAveTestTimeKpiDTO = mapper.Map<QcAveTestTimeKpiDTO>(qcAveTestTimeKpiDomain);
                return CreatedAtAction(nameof(GetById), new { id = qcAveTestTimeKpiDTO.Id }, qcAveTestTimeKpiDTO);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating QcAveTestTimeKpi data in the database.");
            }
        }

        /// <summary>
        /// Deletes a QcAveTestTimeKpi item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var qcAveTestTimeKpiDomain = await qcAveTestTimeKpiRepository.DeleteAsync(id);
                if (qcAveTestTimeKpiDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<QcAveTestTimeKpiDTO>(qcAveTestTimeKpiDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting QcAveTestTimeKpi data for ID {id}.");
            }
        }   

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
