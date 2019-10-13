using FoodShopDAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICategorytRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task CreateCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category dbCategory, Category category);

        Task DeleteCategoryAsync(Category category);
    }
}