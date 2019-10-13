using FoodShopDAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);

        Task CreateRangeOrderDetailsAsync(IEnumerable<OrderDetail> orderDetails);

        Task DeleteOrderDetailsByOrderIdtAsync(int orderId);
    }    
}