using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IPaymentTransactionService
    {
        Task<List<PaymentTransaction>> GetAllPaymentTransactionPlansAsync();
        Task<PaymentTransaction?> GetByIdAsync(int id);

        Task<PaymentTransaction> CreateAsync(PaymentTransaction transaction);

        Task<bool> UpdateAsync(PaymentTransaction transaction);

        Task<bool> DeleteAsync(int id);

        Task<PaymentTransaction?> GetByOrderIdAsync(string id);

    }
}
