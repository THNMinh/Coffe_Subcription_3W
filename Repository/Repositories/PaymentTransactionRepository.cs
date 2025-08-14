using Core.Interfaces.Repositories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class PaymentTransactionRepository : GenericRepository<PaymentTransaction, int>, IPaymentTransactionRepository
    {
        private readonly CoffeSubContext _context;
        public PaymentTransactionRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
    }
}
