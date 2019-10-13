using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }

        ICategorytRepository Category { get; }

        ICartRepository Cart { get; }

        IOrderRepository Order { get; }

        IOrderDetailRepository OrderDetail { get; }
    }
}
