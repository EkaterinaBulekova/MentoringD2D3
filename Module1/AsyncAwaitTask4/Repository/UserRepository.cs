using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .OrderBy(x => x.FirstName)
                .ToListAsync();
        }

        public async Task<List<User>> GetUserByIdAsync(Guid userId)
        {
            return await FindByCondition(o => o.Id.Equals(userId)).ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            Create(user);
            await SaveAsync();
        }

        public async Task UpdateUserAsync(User dbUser, User user)
        {
            dbUser.Map(user);
            Update(dbUser);
            await SaveAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            Delete(user);
            await SaveAsync();
        }
    }
}