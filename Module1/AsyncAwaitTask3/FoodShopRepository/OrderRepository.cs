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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrderByIdAsync(int orderId)
        {
            return await FindByCondition(o => o.Id.Equals(orderId)).ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            Create(order);
            await SaveAsync();
            return order;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            Delete(order);
            await SaveAsync();
        }
    }
}