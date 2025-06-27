using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.CustomerOrder;
using ChemsonLab.API.Repositories.CustomerOrderRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        private readonly ICustomerOrderRepository customerOrderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CustomerOrderController> logger;

        public CustomerOrderController(ICustomerOrderRepository customerOrderRepository, IMapper mapper, ILogger<CustomerOrderController> logger)
        {
            this.customerOrderRepository = customerOrderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all CustomerOrder items with optional filtering and sorting parameters.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="productName"></param>
        /// <param name="status"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? customerName, [FromQuery] string? productName, [FromQuery] string? status, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var customerOrderDomain = await customerOrderRepository.GetAllAsync(customerName, productName, status, sortBy, isAscending);
                return Ok(mapper.Map<List<CustomerOrderDTO>>(customerOrderDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving CustomerOrder data from database.");
            }
        }

        /// <summary>
        /// Retrieves a CustomerOrder item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var customerOrderDomain = await customerOrderRepository.GetByIdAsync(id);
                if (customerOrderDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CustomerOrderDTO>(customerOrderDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the CustomerOrder data ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new CustomerOrder item.
        /// </summary>
        /// <param name="addCustomerOrderRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddCustomerOrderRequestDTO addCustomerOrderRequestDTO)
        {
            try
            {
                var customerOrderDomain = mapper.Map<CustomerOrder>(addCustomerOrderRequestDTO);
                customerOrderDomain = await customerOrderRepository.CreateAsync(customerOrderDomain);
                var customerOrderDto = mapper.Map<CustomerOrderDTO>(customerOrderDomain);
                return CreatedAtAction(nameof(GetById), new {id = customerOrderDto.Id}, customerOrderDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the CustomerOrder item.");
            }
        }

        /// <summary>
        /// Updates an existing CustomerOrder item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCustomerOrderRequestDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCustomerOrderRequestDTO updateCustomerOrderRequestDTO)
        {
            try
            {
                var customerOrderDomain = mapper.Map<CustomerOrder>(updateCustomerOrderRequestDTO);
                customerOrderDomain = await customerOrderRepository.UpdateAsync(id, customerOrderDomain);
                if (customerOrderDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CustomerOrderDTO>(customerOrderDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the CustomerOrder item ID {id}.");
            }
        }

        /// <summary>
        /// Deletes a CustomerOrder item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var customerOrderDomain = await customerOrderRepository.DeleteAsync(id);
                if (customerOrderDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CustomerOrderDTO>(customerOrderDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the CustomerOrder item ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
