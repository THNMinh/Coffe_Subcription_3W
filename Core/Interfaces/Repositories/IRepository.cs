using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IRepository<T>
    {
        //Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(int id, string? includeProperties = null, CancellationToken cancellationToken = default);

        Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, CancellationToken cancellationToken = default);

        //Task<IEnumerable<T>> GetWithPaginationAsync(PageInfoRequest pageInfo, Expression<Func<T, bool>>? filter = null, string? includeProperties = null, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);

        bool Update(T entity);

        bool UpdateRange(IEnumerable<T> entities);

        bool Delete(params T[] entities);
    }
}
