using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Measurement;
using ChemsonLab.API.Repositories.MeasurementRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementRepository measurementRepository;
        private readonly IMapper mapper;
        private readonly ILogger<MeasurementController> logger;

        public MeasurementController(IMeasurementRepository measurementRepository, IMapper mapper, ILogger<MeasurementController> logger)
        {
            this.measurementRepository = measurementRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all Measurement items with optional filtering by testResultId.
        /// </summary>
        /// <param name="testResultId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? testResultId)
        {
            try
            {
                var measurementDomain = await measurementRepository.GetAllAsync(testResultId);
                return Ok(mapper.Map<List<MeasurementDTO>>(measurementDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Measurement data from database.");
            }
        }

        /// <summary>
        /// Retrieves a Measurement item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var measurementDomain = await measurementRepository.GetByIdAsync(id);
                if (measurementDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<MeasurementDTO>(measurementDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Measurement data ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new Measurement item based on the provided request data.
        /// </summary>
        /// <param name="addMeasurementRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddMeasurementRequestDTO addMeasurementRequestDTO)
        {
            try
            {
                var measurementDomain = mapper.Map<Measurement>(addMeasurementRequestDTO);
                measurementDomain = await measurementRepository.CreateAsync(measurementDomain);
                var measurementDto = mapper.Map<MeasurementDTO>(measurementDomain);
                return CreatedAtAction(nameof(GetById), new { id=measurementDto.Id }, measurementDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Measurement item.");
            }
        }

        /// <summary>
        /// Updates an existing Measurement item by its ID with the provided request data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateMeasurementRequestDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMeasurementRequestDTO updateMeasurementRequestDTO)
        {
            try
            {
                var measurementDomain = mapper.Map<Measurement>(updateMeasurementRequestDTO);
                measurementDomain = await measurementRepository.UpdateAsync(id, measurementDomain);
                if (measurementDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<MeasurementDTO>(measurementDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Measurement data ID {id}.");
            }
        }

        /// <summary>
        /// Deletes a Measurement item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var measurementDomain = await measurementRepository.DeleteAsync(id);
                if (measurementDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<MeasurementDTO>(measurementDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Measurement data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
