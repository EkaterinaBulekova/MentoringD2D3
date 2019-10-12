using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using Entities.Extensions;
using FoodShopDAL;
using FoodShopDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await FindAll()
                .OrderBy(x => x.CartId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartByIdAsync(int cartId)
        {
            return await FindByCondition(o => o.Id.Equals(cartId)).ToListAsync();
        }

        public async Task CreateCartAsync(Cart cart)
        {
            Create(cart);
            await SaveAsync();
        }

        public async Task UpdateCartAsync(Cart dbCart, Cart cart)
        {
            dbCart.Map(cart);
            Update(dbCart);
            await SaveAsync();
        }

        public async Task DeleteCartAsync(Cart cart)
        {
            Delete(cart);
            await SaveAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartsByCartIdAsync(string cartId)
        {
            return await FindByCondition(o => o.CartId == cartId).Include(c => c.Product).ToListAsync();
        }

        public async Task AddToCartAsync(string cartId, int productId)
        {
            var dbCart = await FindByCondition(c => c.CartId == cartId && c.ProductId == productId).SingleOrDefaultAsync();
            if (dbCart != null)
            {
                var cart = new Cart() { Count = dbCart.Count + 1 };
                await UpdateCartAsync(dbCart, cart);
            }
            else
                await CreateCartAsync(new Cart() { CartId = cartId, Count = 1, ProductId = productId });
        }

        public async Task<ShoppingCart> GetCartTotalsAsync(string cartId)
        {
            var carts = await FindByCondition(o => o.CartId == cartId).Include(c => c.Product).ToListAsync(); 
            return new ShoppingCart(carts); 
        }
    }
}