using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
        }
        public async Task<User> GetByEmailAsync(string email, string? includeProperties = null, CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _set;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var incluProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluProp);
                }
            }
            return await query.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
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
            existingUser.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
