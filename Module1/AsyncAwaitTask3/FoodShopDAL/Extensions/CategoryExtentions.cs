using FoodShopDAL.Models;

namespace Entities.Extensions
{
    public static class CategoryExtentions
    {
        public static void Map(this Category dbCategory, Category category)
        {
            dbCategory.Name = category.Name;
        }

    }
}