using FoodShopDAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<IEnumerable<Cart>> GetCartByIdAsync(int cartId);
        Task<IEnumerable<Cart>> GetCartsByCartIdAsync(string cartId);
        Task CreateCartAsync(Cart cart);
        Task UpdateCartAsync(Cart dbCart, Cart cart);
        Task DeleteCartAsync(Cart cart);
        Task AddToCartAsync(string cartId, int productId);
        Task<ShoppingCart> GetCartTotalsAsync(string cartId);
    }    
}