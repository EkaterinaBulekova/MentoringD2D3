using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodShopDAL.Models
{
    [Table("Categories")]
    public class Category : IEntity
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        [DisplayName("Category ID")]
        public int Id { get; set; }

        [DisplayName("Category")]
        [StringLength(15)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
