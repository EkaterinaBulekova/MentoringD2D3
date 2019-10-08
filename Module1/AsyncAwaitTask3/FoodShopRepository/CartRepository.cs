using System.Collections.Generic;
using System.Linq;
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
                .OrderBy(x => x.Product.Name)
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
            return await FindByCondition(o => o.CartId == cartId).ToListAsync();
        }
    }
}