using FoodShopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShopWeb.Models
{
    public class NewOrder
    {
        public string CartId { get; set; }
        public Order Order { get; set; }
    }
}
