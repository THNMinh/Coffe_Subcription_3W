using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;

        public PaymentTransactionService(IPaymentTransactionRepository paymentTransactionRepository)
        {
            _paymentTransactionRepository = paymentTransactionRepository;
        }

        public async Task<PaymentTransaction> CreateAsync(PaymentTransaction transaction)
        {
            await _paymentTransactionRepository.CreateAsync(transaction);
            return transaction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _paymentTransactionRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<PaymentTransaction>> GetAllPaymentTransactionPlansAsync()
        {
            var transactions = await _paymentTransactionRepository.GetAllAsync();
            return transactions;
        }

        public async Task<PaymentTransaction?> GetByIdAsync(int id)
        {
            return await _paymentTransactionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(PaymentTransaction transaction)
        {
            await _paymentTransactionRepository.UpdateAsync(transaction);
            return true;
        }

        public async Task<PaymentTransaction?> GetByOrderIdAsync(string id)
        {
            return await _paymentTransactionRepository.GetByOrderIdAsync(id);

        }

    }
}
