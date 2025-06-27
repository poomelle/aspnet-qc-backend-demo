using AutoMapper;
using ChemsonLab.API.CustomActionFilter;
using ChemsonLab.API.Data;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Repositories.ProductRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace ChemsonLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ProductController> logger;

        public ProductController(IProductRepository productRepository, IMapper mapper, ILogger<ProductController> logger)
        {

            this.productRepository = productRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all Product items with optional filtering and sorting parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="sortBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? status, [FromQuery] string? sortBy, [FromQuery] bool isAscending)
        {
            try
            {
                var productsDomain = await productRepository.GetAllAsync(name, status, sortBy, isAscending);
                return Ok(mapper.Map<List<ProductDTO>>(productsDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Product data from database.");
            }
        }

        /// <summary>
        /// Retrieves a Product item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var productDomain = await productRepository.GetByIdAsync(id);
                if (productDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<ProductDTO>(productDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving the Product data ID {id}.");
            }
        }

        /// <summary>
        /// Creates a new Product item based on the provided request data.
        /// </summary>
        /// <param name="addProductRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddProductRequestDTO addProductRequestDTO)
        {
            try
            {
                var productDomain = mapper.Map<Product>(addProductRequestDTO);
                productDomain = await productRepository.CreateAsync(productDomain);
                var productsDto = mapper.Map<ProductDTO>(productDomain);

                return CreatedAtAction(nameof(GetById), new { id = productsDto.Id }, productsDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the Product item.");
            }
        }

        /// <summary>
        /// Updates an existing Product item based on the provided ID and request data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProductRequestDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequestDTO updateProductRequestDTO)
        {
            try
            {
                var productDomain = mapper.Map<Product>(updateProductRequestDTO);
                productDomain = await productRepository.UpdateAsync(id, productDomain);
                if (productDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<ProductDTO>(productDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating the Product data ID {id}.");
            }

        }

        /// <summary>
        /// Deletes a Product item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var productDomain = await productRepository.DeleteAsync(id);

                if (productDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<ProductDTO>(productDomain));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting the Product data ID {id}.");
            }
        }

        private void LogError(Exception exception)
        {
            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
        }
    }
}
