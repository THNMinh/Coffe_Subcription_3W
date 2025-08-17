using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.DTOs.Request;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Mapster;
using Repository.Repositories;

namespace Service.Services
{
    public class DailyCupTrackingService : IDailyCupTrackingService
    {
        private readonly IDailyCupTrackingRepository _repo;
        public DailyCupTrackingService(IDailyCupTrackingRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DailyCupTrackingDTO>> GetAllDailyCupTrackingsAsync()
        {
            var trackings = await _repo.GetAllAsync();
            var dtos = trackings.Adapt<List<DailyCupTrackingDTO>>();
            return dtos;
        }

        public async Task<DailyCupTrackingDTO?> GetByIdAsync(int id)
        {
            var tracking = await _repo.GetByIdAsync(id);
            var dtos = tracking.Adapt<DailyCupTrackingDTO>();
            return dtos;
        }

        public async Task<DailyCupTrackingDTO> CreateAsync(DailyCupTracking tracking)
        {
            await _repo.CreateAsync(tracking);
            var dtos = tracking.Adapt<DailyCupTrackingDTO>();
            return dtos;
        }

        public async Task<bool> UpdateAsync(DailyCupTracking tracking)
        {
            await _repo.UpdateAsync(tracking);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
