using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Coa;
using ChemsonLab.API.Repositories.CoaRepository;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoaController : Controller
    {
        private readonly ICoaRepository coaRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CoaController> logger;

        public CoaController(ICoaRepository coaRepository, IMapper mapper, ILogger<CoaController> logger)
        {
            this.coaRepository = coaRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? productName, [FromQuery] string? batchName, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var coaDomain = await coaRepository.GetAllAsync(productName, batchName, sortBy, isAscending);
                return Ok(mapper.Map<List<CoaDTO>>(coaDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Coa data from database.");
            }
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var coaDomain = await coaRepository.GetByIdAsync(id);
                if (coaDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CoaDTO>(coaDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Coa data ID {id}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCoaRequestDTO addCoaRequestDTO)
        {
            try
            {
                var coaDomain = mapper.Map<Coa>(addCoaRequestDTO);
                coaDomain = await coaRepository.CreateAsync(coaDomain);
                var coaDto = mapper.Map<CoaDTO>(coaDomain);

                return CreatedAtAction(nameof(GetById), new { id = coaDto.Id }, coaDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Coa data.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCoaRequestDTO updateCoaRequestDTO)
        {
            try
            {
                var coaDomain = mapper.Map<Coa>(updateCoaRequestDTO);
                coaDomain = await coaRepository.UpdateAsync(id, coaDomain);
                if (coaDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CoaDTO>(coaDomain));

            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Coa data ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var coaDomain = await coaRepository.DeleteAsync(id);
                if (coaDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CoaDTO>(coaDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Coa data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
