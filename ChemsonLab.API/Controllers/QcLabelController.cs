using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.QcLabel;
using ChemsonLab.API.Repositories.QcLabelRepository;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QcLabelController : ControllerBase
    {
        private readonly IQcLabelRepository qcLabelRepository;
        private readonly IMapper mapper;
        private readonly ILogger<QcLabelController> logger;

        public QcLabelController(IQcLabelRepository qcLabelRepository, IMapper mapper, ILogger<QcLabelController> logger)
        {
            this.qcLabelRepository = qcLabelRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all QcLabel items with optional filtering by batchName, productName, printed status, year, and month.
        /// </summary>
        /// <param name="batchName"></param>
        /// <param name="productName"></param>
        /// <param name="printed"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? batchName, [FromQuery] string? productName, [FromQuery] string? printed, [FromQuery] string? year = null, [FromQuery] string? month = null)
        {
            try
            {
                var qcLabelDomain = await qcLabelRepository.GetAllAsync(batchName, productName, printed, year, month);
                return Ok(mapper.Map<List<QcLabelDTO>>(qcLabelDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the QcLabel data from database.");
            }
        }

        /// <summary>
        /// Retrieves a QcLabel item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var qcLabelDomain = await qcLabelRepository.GetByIdAsync(id);
                if (qcLabelDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<QcLabelDTO>(qcLabelDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the QcLabel data ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new QcLabel item based on the provided request data transfer object (DTO).
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddQcLabelRequestDTO requestDTO)
        {
            try
            {
                var qcLabelDomain = mapper.Map<QcLabel>(requestDTO);
                qcLabelDomain = await qcLabelRepository.CreateAsync(qcLabelDomain);
                var qcLabelDTO = mapper.Map<QcLabelDTO>(qcLabelDomain);
                return CreatedAtAction(nameof(GetById), new { id = qcLabelDTO.Id }, qcLabelDTO);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the QcLabel data.");
            }
        }

        /// <summary>
        /// Update an existing QcLabel item by its ID using the provided request data transfer object (DTO).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateQcLabelRequestDTO requestDTO)
        {
            try
            {
                var qcLabelDomain = mapper.Map<QcLabel>(requestDTO);
                qcLabelDomain = await qcLabelRepository.UpdateAsync(id, qcLabelDomain);
                if (qcLabelDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<QcLabelDTO>(qcLabelDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the QcLabel data ID {id}.");
            }
        }

        /// <summary>
        /// Deletes a QcLabel item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var qcLabelDomain = await qcLabelRepository.DeleteAsync(id);
                if (qcLabelDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<QcLabelDTO>(qcLabelDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the QcLabel data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
