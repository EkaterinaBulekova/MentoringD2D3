using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Extensions;
using FoodShopDAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodShopWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IEnumerable<Product> products = new List<Product>
            {
                new Product{ Id = 1, Name = "pizza", CategoryId = 3, Price = 2000 },
                new Product{ Id = 2, Name = "cacke", CategoryId = 3, Price = 5000 },
                new Product{ Id = 3, Name = "fish", CategoryId = 2, Price = 2000 },
                new Product{ Id = 4, Name = "lamb", CategoryId = 2, Price = 2500 },
            };
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
                //var products = await _repository.Product.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllusers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("category={id}", Name = "productByCategory")]
        public async Task<IActionResult> GetByCategory(int id)
        {
            products = products.Where(_ => _.CategoryId == id);

            try
            {
                //var products = await _repository.Product.GetProductsByCategoryAsync(id);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllusers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "productById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //var products = await _repository.Product.GetProductByIdAsync(id);
                products = products.Where(_ => _.Id == id);
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
                _logger.LogError($"Something went wrong inside GetuserById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}