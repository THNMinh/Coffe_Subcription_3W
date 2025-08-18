using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Mapster;

namespace Service.Services
{
    public class SubscriptionTimeWindowService : ISubscriptionTimeWindowService
    {
        private readonly ISubscriptionTimeWindowRepository _repo;
        public SubscriptionTimeWindowService(ISubscriptionTimeWindowRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SubscriptionTimeWindowDTO>> GetAllSubscriptionTimeWindowsAsync()
        {
            var trackings = await _repo.GetAllAsync();
            var dtos = trackings.Adapt<List<SubscriptionTimeWindowDTO>>();
            return dtos;
        }

        public async Task<SubscriptionTimeWindowDTO?> GetByIdAsync(int id)
        {
            var tracking = await _repo.GetByIdAsync(id);
            var dtos = tracking.Adapt<SubscriptionTimeWindowDTO>();
            return dtos;
        }

        public async Task<SubscriptionTimeWindowDTO> CreateAsync(SubscriptionTimeWindow timeWindow)
        {
            await _repo.CreateAsync(timeWindow);
            var dtos = timeWindow.Adapt<SubscriptionTimeWindowDTO>();
            return dtos;
        }

        public async Task<bool> UpdateAsync(SubscriptionTimeWindow timeWindow)
        {
            await _repo.UpdateAsync(timeWindow);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
