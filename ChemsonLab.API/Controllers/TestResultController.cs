using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.TestResult;
using ChemsonLab.API.Repositories.TestResultRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly ITestResultRepository testResultRepository;
        private readonly IMapper mapper;
        private readonly ILogger<TestResultController> logger;

        public TestResultController(ITestResultRepository testResultRepository, IMapper mapper, ILogger<TestResultController> logger)
        {
            this.testResultRepository = testResultRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var testResultDomain = await testResultRepository.GetAllAsync();
                return Ok(mapper.Map<List<TestResultDTO>>(testResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the TestResult data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var testResultDomain = await testResultRepository.GetByIdAsync(id);
                if (testResultDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<TestResultDTO>(testResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the TestResult data ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddTestResultRequestDTO addTestResultRequestDTO)
        {
            try
            {
                var testResultDomain = mapper.Map<TestResult>(addTestResultRequestDTO);
                testResultDomain = await testResultRepository.CreateAsync(testResultDomain);
                var testResultDto = mapper.Map<TestResultDTO>(testResultDomain);
                return CreatedAtAction(nameof(GetById), new {id = testResultDto.Id}, testResultDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the TestResult item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTestResultRequestDTO updateTestResultRequestDTO)
        {
            try
            {
                var testResultDomain = mapper.Map<TestResult>(updateTestResultRequestDTO);
                if (testResultDomain == null)
                {
                    return NotFound();
                }
                testResultDomain = await testResultRepository.UpdateAsync(id, testResultDomain);
                return Ok(mapper.Map<TestResultDTO>(testResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the TestResult data ID {id}. {ex}");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var testResultDomain = await testResultRepository.DeleteAsync(id);
                if (testResultDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<TestResultDTO>(testResultDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the TestResult data ID {id},");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
