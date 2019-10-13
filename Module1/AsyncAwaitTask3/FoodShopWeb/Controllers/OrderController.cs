using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using FoodShopDAL.Models;
using FoodShopWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodShopWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public OrderController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{id}", Name = "byOrderId")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var orders = await _repository.Order.GetOrderByIdAsync(id);
                if (!orders.Any())
                {
                    _logger.LogError($"order with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                var order = orders.Single();
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetCartId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("detailsId={orderid}", Name = "detailsByOrderId")]
        public async Task<IActionResult> GetCartsByCartId(int orderId)
        {
            try
            {
                var details = await _repository.OrderDetail.GetOrderDetailsByOrderIdAsync(orderId);
                return Ok(details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetCartsByCartId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> CreateNewOrderWithDetails([FromBody]NewOrder newOrder)
        {
            try
            {
                newOrder.Order.OrderDate = DateTime.Today;
                var order = await _repository.Order.CreateOrderAsync(newOrder.Order);
                var carts = await _repository.Cart.GetCartsByCartIdAsync(newOrder.CartId);
                await _repository.OrderDetail.CreateRangeOrderDetailsAsync(carts.Select(c => new OrderDetail() 
                { 
                    OrderId = order.Id,
                    Count = c.Count, 
                    ProductName = c.Product.Name, 
                    UnitPrice = c.Product.Price})
                    .ToList());
                await _repository.Cart.DeleteCartsRangeAsync(carts);
                return Ok(order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddToCart action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}