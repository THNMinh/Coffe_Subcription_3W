using System.Linq.Expressions;
using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User> GetAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default);
        Task<bool> UpdatePassword(User user);
    }
}
