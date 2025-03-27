using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Report;
using ChemsonLab.API.Repositories.ReportRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository reportRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ReportController> logger;

        public ReportController(IReportRepository reportRepository, IMapper mapper, ILogger<ReportController> logger)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? createBy, [FromQuery] string? createDate, [FromQuery] string? status)
        {
            try
            {
                var reportDomain = await reportRepository.GetAllAsync(createBy, createDate, status);

                return Ok(mapper.Map<List<ReportDTO>>(reportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Report data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var reportDomain = await reportRepository.GetByIdAsync(id);
                if (reportDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<ReportDTO>(reportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Report data ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddReportRequestDTO addReportRequestDTO)
        {
            try
            {
                var reportDomain = mapper.Map<Report>(addReportRequestDTO);
                reportDomain = await reportRepository.CreateAsync(reportDomain);
                var reportDto = mapper.Map<ReportDTO>(reportDomain);

                return CreatedAtAction(nameof(GetById), new { id = reportDto.Id }, reportDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Report item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReportRequestDTO updateReportRequestDTO)
        {
            try
            {
                var reportDomain = mapper.Map<Report>(updateReportRequestDTO);
                reportDomain = await reportRepository.UpdateAsync(id, reportDomain);
                if (reportDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<ReportDTO>(reportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Report data ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var reportDomain = await reportRepository.DeleteAsync(id);
                if (reportDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<ReportDTO>(reportDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Report data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
