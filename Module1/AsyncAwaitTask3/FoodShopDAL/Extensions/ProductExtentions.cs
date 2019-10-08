using FoodShopDAL.Models;

namespace Entities.Extensions
{
    public static class ProductExtentions
    {
        public static void Map(this Product dbProduct, Product product)
        {
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.CategoryId = product.CategoryId;
        }

    }
}