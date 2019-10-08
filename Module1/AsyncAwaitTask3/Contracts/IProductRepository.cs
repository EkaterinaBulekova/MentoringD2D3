using FoodShopDAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<List<Product>> GetProductByIdAsync(int productId);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product dbProduct, Product product);
        Task DeleteProductAsync(Product product);
    }
}
