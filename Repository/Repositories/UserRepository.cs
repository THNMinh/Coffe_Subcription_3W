using System.Linq.Expressions;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        private readonly CoffeSubContext _context;
        private readonly DbSet<User> _set;
        public UserRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
            _set = context.Set<User>();
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email must not be empty.", nameof(email));

            return await _context.Set<User>()
                                 .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await _set.SingleOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<bool> UpdatePassword(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.PasswordHash = user.PasswordHash;
            existingUser.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);
            existingUser.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
