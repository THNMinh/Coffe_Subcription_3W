using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Core.DTOs.Request;

namespace Repository.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : Entity where TKey : IEquatable<TKey>
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

        public Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Where(x => x.IsDelete == false).CountAsync(cancellationToken);
        }     
        
        public async Task<IEnumerable<T>> GetWithPaginationAsync(PageInfoRequestDTO pageInfo, 
            Expression<Func<T, bool>>? filter = null, string? includeProperties = null, CancellationToken cancellationToken = default)
        {
            int pageNum = pageInfo.PageNum;
            int pageSize = pageInfo.PageSize;
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (pageNum > 0 && pageSize > 0)
            {
                query = query.Skip((pageNum - 1) * pageSize).Take(pageSize);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var incluProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluProp);
                }
            }
            return await query.ToListAsync(cancellationToken);
        }
    }
}
