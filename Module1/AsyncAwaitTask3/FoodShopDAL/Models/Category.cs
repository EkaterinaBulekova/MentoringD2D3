using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShopDAL.Models
{
    [Table("Categories")]
    public class Category : IEntity
    {
        [Key]
        [DisplayName("Category ID")]
        public int Id { get; set; }

        [DisplayName("Category")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
