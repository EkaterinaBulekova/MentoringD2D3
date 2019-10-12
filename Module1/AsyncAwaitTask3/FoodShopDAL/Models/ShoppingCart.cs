using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodShopDAL.Models
{
    public class ShoppingCart
    {
        private readonly List<Cart> carts;

        public ShoppingCart(IEnumerable<Cart> carts)
        {
             this.carts = carts.ToList();
        }

        public string Id => carts.Any() ? carts[0].CartId : Guid.NewGuid().ToString();

        public int TotalCount => carts.Any() ? carts.Select(c => c.Count).Sum() : 0;

        public decimal TotalPrice => carts.Any() ? carts.Select(c => c.Count * c.Product.Price).Sum() : 0;
    }
}
