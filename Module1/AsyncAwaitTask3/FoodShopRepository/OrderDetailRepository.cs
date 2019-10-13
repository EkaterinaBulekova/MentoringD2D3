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
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await FindByCondition(o => o.OrderId.Equals(orderId)).ToListAsync();
        }

        public async Task CreateRangeOrderDetailsAsync(IEnumerable<OrderDetail> orderDetails)
        {
            CreateRange(orderDetails);
            await SaveAsync();
        }


        public async Task DeleteOrderDetailsByOrderIdtAsync(int orderId)
        {
            var orders = await FindByCondition(o => o.OrderId.Equals(orderId)).ToListAsync();
            DeleteRange(orders);
            await SaveAsync();
        }
    }
}