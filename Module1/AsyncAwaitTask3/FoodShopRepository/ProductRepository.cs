using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Extensions;
using FoodShopDAL.Models;
using FoodShopDAL;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductByIdAsync(int ProductId)
        {
            return await FindByCondition(o => o.Id.Equals(ProductId)).ToListAsync();
        }

        public async Task CreateProductAsync(Product Product)
        {
            Create(Product);
            await SaveAsync();
        }

        public async Task UpdateProductAsync(Product dbProduct, Product Product)
        {
            dbProduct.Map(Product);
            Update(dbProduct);
            await SaveAsync();
        }

        public async Task DeleteProductAsync(Product Product)
        {
            Delete(Product);
            await SaveAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await FindByCondition(o => o.CategoryId.Equals(categoryId)).ToListAsync();
        }
    }
}