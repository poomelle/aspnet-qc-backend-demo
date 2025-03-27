using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Customer;
using ChemsonLab.API.Repositories.CustomerRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerController> logger)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? status, [FromQuery] string? sortBy, [FromQuery] bool isAscending) 
        {
            try
            {
                var customerDomain = await customerRepository.GetAllAsync(name, status, sortBy, isAscending);
                return Ok(mapper.Map<List<CustomerDTO>>(customerDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Customer data from database.");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var customerDomain = await customerRepository.GetByIdAsync(id);
                if (customerDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CustomerDTO>(customerDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Customer data ID {id}.");
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddCustomerRequestDTO addCustomerRequestDTO)
        {
            try
            {
                var customerDomain = mapper.Map<Customer>(addCustomerRequestDTO);
                customerDomain = await customerRepository.CreateAsync(customerDomain);
                var customerDto = mapper.Map<CustomerDTO>(customerDomain);
                return CreatedAtAction(nameof(GetById), new {id = customerDto.Id}, customerDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Customer item.");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCustomerRequestDTO updateCustomerRequestDTO)
        {
            try
            {
                var customerDomain = mapper.Map<Customer>(updateCustomerRequestDTO);
                customerDomain = await customerRepository.UpdateAsync(id, customerDomain);
                if (customerDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CustomerDTO>(customerDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Customer item ID {id}.");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var customerDomain = await customerRepository.DeleteAsync(id);
                if (customerDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CustomerDTO?>(customerDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Customer item ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
