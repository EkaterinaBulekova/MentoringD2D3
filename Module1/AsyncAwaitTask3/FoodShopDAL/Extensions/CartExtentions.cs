using FoodShopDAL.Models;

namespace Entities.Extensions
{
    public static class CartExtentions
    {
        public static void Map(this Cart dbCart, Cart cart)
        {
            dbCart.Count = cart.Count;
        }

    }
}