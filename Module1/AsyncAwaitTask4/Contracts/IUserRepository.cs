using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<List<User>> GetUserByIdAsync(Guid userId);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User dbUser, User user);
        Task DeleteUserAsync(User user);
    }
}
