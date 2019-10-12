using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FoodShopWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        
        public ProductController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _repository.Product.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllProducts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("category={id}", Name = "productByCategory")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            try
            {
                var products = await _repository.Product.GetProductsByCategoryAsync(id);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetProductsByCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "productById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var products = await _repository.Product.GetProductByIdAsync(id);
                if (products.Count() == 0)
                {
                    _logger.LogError($"product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var product = products.Single();

                _logger.LogInfo($"Returned product with id: {id}");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetProductById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}