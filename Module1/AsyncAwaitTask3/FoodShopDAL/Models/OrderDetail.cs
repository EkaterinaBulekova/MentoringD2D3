using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FoodShopDAL.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int Count { get; set; }

        public virtual Order Order { get; set; }

    }
}
