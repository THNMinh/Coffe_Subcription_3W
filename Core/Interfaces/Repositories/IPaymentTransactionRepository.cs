using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IPaymentTransactionRepository : IGenericRepository<PaymentTransaction, int>
    {
        public Task<PaymentTransaction> GetByOrderIdAsync(string id);
    }
}
