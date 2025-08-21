using System.Linq.Expressions;
using Core.DTOs.Request;

namespace Core.Interfaces.Repository
{
    public interface IGenericRepository<T, TKey> where T : class where TKey : IEquatable<TKey>
    {
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(TKey id);

        public ValueTask<T> CreateAsync(T t);

        public ValueTask<bool> UpdateAsync(T t);

        public ValueTask<bool> DeleteAsync(TKey id);

        public Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> GetWithPaginationAsync(PageInfoRequestDTO pageInfo,
            Expression<Func<T, bool>>? filter = null, string? includeProperties = null, CancellationToken cancellationToken = default);

    }
}
