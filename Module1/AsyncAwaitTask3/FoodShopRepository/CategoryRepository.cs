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
    public class CategoryRepository : RepositoryBase<Category>, ICategorytRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoryByIdAsync(int categoryId)
        {
            return await FindByCondition(o => o.Id.Equals(categoryId)).ToListAsync();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            Create(category);
            await SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category dbCategory, Category category)
        {
            dbCategory.Map(category);
            Update(dbCategory);
            await SaveAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            Delete(category);
            await SaveAsync();
        }
    }
}