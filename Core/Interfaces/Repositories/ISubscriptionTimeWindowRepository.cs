using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repository;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface ISubscriptionTimeWindowRepository : IGenericRepository<SubscriptionTimeWindow, int>
    {
    }
}
