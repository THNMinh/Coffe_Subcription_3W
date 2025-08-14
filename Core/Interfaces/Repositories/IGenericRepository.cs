using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
    public interface IGenericRepository<T, TKey> where T : class where TKey : IEquatable<TKey>
    {
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(TKey id);

        public ValueTask<T> CreateAsync(T t);

        public ValueTask<bool> UpdateAsync(T t);

        public ValueTask<bool> DeleteAsync(TKey id);

     }
}
