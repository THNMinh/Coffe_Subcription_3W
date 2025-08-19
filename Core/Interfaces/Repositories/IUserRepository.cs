using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<User> GetByEmailAsync(string email, string? includeProperties = null, CancellationToken cancellationToken = default);
        Task<User> GetAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default);
        Task<bool> UpdatePassword(User user);
    }
}
