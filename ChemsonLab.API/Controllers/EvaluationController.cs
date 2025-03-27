using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Evaluation;
using ChemsonLab.API.Repositories.EvaluationRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationRepository evaluationRepository;
        private readonly IMapper mapper;
        private readonly ILogger<EvaluationController> logger;

        public EvaluationController(IEvaluationRepository evaluationRepository, IMapper mapper, ILogger<EvaluationController> logger)
        {
            this.evaluationRepository = evaluationRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? testResultId, [FromQuery] string? pointName)
        {
            try
            {
                var evaluationDomain = await evaluationRepository.GetAllAsync(testResultId, pointName);
                return Ok(mapper.Map<List<EvaluationDTO>>(evaluationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Evaluation data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var evaluationDomain = await evaluationRepository.GetByIdAsync(id);
                if (evaluationDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<EvaluationDTO>(evaluationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Evaluation data ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddEvaluationRequestDTO addEvaluationRequestDTO)
        {
            try
            {
                var evaluationDomain = mapper.Map<Evaluation>(addEvaluationRequestDTO);
                evaluationDomain = await evaluationRepository.CreateAsync(evaluationDomain);
                var evaluationDto = mapper.Map<EvaluationDTO>(evaluationDomain);
                return CreatedAtAction(nameof(GetById), new {id = evaluationDto.Id}, evaluationDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Evaluation item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEvaluationRequestDTO updateEvaluationRequestDTO)
        {
            try
            {
                var evaluationDomain = mapper.Map<Evaluation>(updateEvaluationRequestDTO);
                evaluationDomain = await evaluationRepository.UpdateAsync(id, evaluationDomain);
                if (evaluationDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<EvaluationDTO>(evaluationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Evaluation item.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var evaluationDomain = await evaluationRepository.DeleteAsync(id);
                if (evaluationDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<EvaluationDTO>(evaluationDomain));
            }
            catch(Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Evaluation item ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
