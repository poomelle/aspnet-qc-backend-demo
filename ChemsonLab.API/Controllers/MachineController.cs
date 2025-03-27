using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Machine;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Repositories.MachineRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMachineRepository machineRepository;
        private readonly IMapper mapper;
        private readonly ILogger<MachineController> logger;

        public MachineController(IMachineRepository machineRepository, IMapper mapper, ILogger<MachineController> logger)
        {
            this.machineRepository = machineRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? status, [FromQuery] string? sortBy, [FromQuery] bool isAscending) 
        {
            try
            {
                var machineDomain = await machineRepository.GetAllAsync(name, status, sortBy, isAscending);
                return Ok(mapper.Map<List<MachineDTO>>(machineDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Machine data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var machineDomain = await machineRepository.GetByIdAsync(id);
                if (machineDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<MachineDTO>(machineDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Machine data ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddMachineReuqestDTO addMachineReuqestDTO)
        {
            try
            {
                var machineDomain = mapper.Map<Machine>(addMachineReuqestDTO);
                machineDomain = await machineRepository.CreateAsync(machineDomain);
                var machineDto = mapper.Map<MachineDTO>(machineDomain);

                return CreatedAtAction(nameof(GetById), new { id = machineDto.Id }, machineDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Machine item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMachineRequestDTO updateMachineRequestDTO)
        {
            try
            {
                var machineDomain = mapper.Map<Machine>(updateMachineRequestDTO);
                machineDomain = await machineRepository.UpdateAsync(id, machineDomain);
                if (machineDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<MachineDTO>(machineDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Machine data ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var machineDomain = await machineRepository.DeleteAsync(id);
                if (machineDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<MachineDTO>(machineDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Machine data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
