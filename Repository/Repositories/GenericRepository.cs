using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class where TKey : IEquatable<TKey>
    {

        private readonly CoffeSubContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CoffeSubContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            IQueryable<T> query = _dbSet;
            return query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);
        

        public async ValueTask<T> CreateAsync(T t)
        {
            var result = await _dbSet.AddAsync(t);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }


        public async ValueTask<bool> UpdateAsync(T t)
        {
            _dbSet.Update(t);
            return await SaveChangesAsync();
        }

        public async ValueTask<bool> DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            return await SaveChangesAsync();
        }

        private async ValueTask<bool> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
