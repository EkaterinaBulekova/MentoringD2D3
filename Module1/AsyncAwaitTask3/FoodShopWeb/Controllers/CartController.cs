using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using FoodShopDAL.Models;
using FoodShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FoodShopWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public CartController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{id}", Name = "cartById")]
        public async Task<IActionResult> GetCartId(string id)
        {
            try
            {
                var cart = new ShoppingCart(await _repository.Cart.GetCartsByCartIdAsync(id));
                return new JsonResult(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetCartId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("cartid={cartid}", Name = "cartsByCartId")]
        public async Task<IActionResult> GetCartsByCartId(string cartId)
        {
            try
            {
                var carts = await _repository.Cart.GetCartsByCartIdAsync(cartId);
                var json = new JsonResult(carts);
                return  json;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetCartsByCartId action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Post([FromBody]NewCart newCart)
        {
            try
            {
                await _repository.Cart.AddToCartAsync(newCart.CartId, newCart.ProductId);
                var cart = new ShoppingCart(await _repository.Cart.GetCartsByCartIdAsync(newCart.CartId));
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddToCart action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]DeletedCart cart)
        {
            try
            {
                var deletedcart = await _repository.Cart.GetCartByIdAsync(cart.Id);
                await _repository.Cart.DeleteCartAsync(deletedcart.SingleOrDefault());
                var shoppingcart = new ShoppingCart(await _repository.Cart.GetCartsByCartIdAsync(cart.CartId));

                return Ok(shoppingcart);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteFromCart action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}