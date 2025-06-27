using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.ProductSpecification;
using ChemsonLab.API.Repositories.ProductSpecificationRepository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSpecificationController : ControllerBase
    {
        private readonly IProductSpecificationRepository productSpecificationRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ProductSpecificationController> logger;

        public ProductSpecificationController(IProductSpecificationRepository productSpecificationRepository, IMapper mapper, ILogger<ProductSpecificationController> logger)
        {
            this.productSpecificationRepository = productSpecificationRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all Product Specification items with optional filtering and sorting parameters.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="machineName"></param>
        /// <param name="inUse"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? productName, [FromQuery] string? machineName, [FromQuery] string? inUse, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var productSpecificationDomain = await productSpecificationRepository.GetAllAsync(productName, machineName, inUse, sortBy, isAscending);

                return Ok(mapper.Map<List<ProductSpecificationDTO>>(productSpecificationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Product Specification data from database.");
            }
        }

        /// <summary>
        /// Retrieves a Product Specification item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var productSpecificationDomain = await productSpecificationRepository.GetByIdAsync(id);
                if (productSpecificationDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<ProductSpecificationDTO>(productSpecificationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Product Specification data ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new Product Specification item.
        /// </summary>
        /// <param name="addProductSpecificationRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddProductSpecificationRequestDTO addProductSpecificationRequestDTO)
        {
            try
            {
                var productSpecificationDomain = mapper.Map<ProductSpecification>(addProductSpecificationRequestDTO);
                productSpecificationDomain = await productSpecificationRepository.CreateAsync(productSpecificationDomain);
                var productSpecificationDto = mapper.Map<ProductSpecificationDTO>(productSpecificationDomain);
                return CreatedAtAction(nameof(GetById), new { id = productSpecificationDto.Id }, productSpecificationDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Product Specification item.");
            }
        }

        /// <summary>
        /// Updates an existing Product Specification item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProductSpecificationRequestDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductSpecificationRequestDTO updateProductSpecificationRequestDTO)
        {
            try
            {
                var productSpecificationDomain = mapper.Map<ProductSpecification>(updateProductSpecificationRequestDTO);
                productSpecificationDomain = await productSpecificationRepository.UpdateAsync(id, productSpecificationDomain);
                if (productSpecificationDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<ProductSpecificationDTO>(productSpecificationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Product Specification data ID {id}.");
            }
        }

        /// <summary>
        /// Deletes a Product Specification item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var productSpecificationDomain = await productSpecificationRepository.DeleteAsync(id);
                if (productSpecificationDomain == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<ProductSpecificationDTO>(productSpecificationDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Product Specification data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
