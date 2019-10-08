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
    public class CategoryController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IEnumerable<Category> categories = new List<Category>
            {
                new Category{ Id = 1, Name = "Category1" },
                new Category{ Id = 2, Name = "Category2" },
                new Category{ Id = 3, Name = "Category3" }
            };
        public CategoryController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var categories = await _repository.Category.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllusers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}