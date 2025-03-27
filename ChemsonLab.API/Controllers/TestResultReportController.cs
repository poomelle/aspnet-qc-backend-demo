using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.TestResultReport;
using ChemsonLab.API.Repositories.TestResultReportRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultReportController : ControllerBase
    {
        private readonly ITestResultReportRepository testResultReportRepository;
        private readonly IMapper mapper;
        private readonly ILogger<TestResultReportController> logger;

        public TestResultReportController(ITestResultReportRepository testResultReportRepository, IMapper mapper, ILogger<TestResultReportController> logger)
        {
            this.testResultReportRepository = testResultReportRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? createBy, [FromQuery] string? testDate, [FromQuery] string? productName, [FromQuery] string? batchName, [FromQuery] string? result, [FromQuery] string? batchTestResultId, [FromQuery] string? exactBatchName, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var testResultReportDomain = await testResultReportRepository.GetAllAsync(createBy, testDate, productName, batchName, result, batchTestResultId, exactBatchName, sortBy, isAscending);
                return Ok(mapper.Map<List<TestResultReportDTO>>(testResultReportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the TestResultReport data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var testResultReportDomain = await testResultReportRepository.GetByIdAsync(id);
                if (testResultReportDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<TestResultReportDTO>(testResultReportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the TestResultReport data ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddTestResultReportRequestDTO requestDTO)
        {
            try
            {
                var testResultReportDomain = mapper.Map<TestResultReport>(requestDTO);
                testResultReportDomain = await testResultReportRepository.CreateAsync(testResultReportDomain);
                var testResultReportDto = mapper.Map<TestResultReportDTO>(testResultReportDomain);
                return CreatedAtAction(nameof(GetById), new {id = testResultReportDto.Id}, testResultReportDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating the TestResultReport item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTestResultReportRquestDTO requestDTO)
        {
            try
            {
                var testResultReportDomain = mapper.Map<TestResultReport>(requestDTO);
                testResultReportDomain = await testResultReportRepository.UpdateAsync(id, testResultReportDomain);
                if (testResultReportDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<TestResultReportDTO>(testResultReportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the TestResultReport data ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var testResultReportDomain = await testResultReportRepository.DeleteAsync(id);
                if (testResultReportDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<TestResultReportDTO>(testResultReportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the TestResultReport data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
