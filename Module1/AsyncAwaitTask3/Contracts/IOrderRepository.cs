using FoodShopDAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrderByIdAsync(int orderId);

        Task<Order> CreateOrderAsync(Order order);

        Task DeleteOrderAsync(Order order);
    }    
}