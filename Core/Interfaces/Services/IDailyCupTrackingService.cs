using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.DTOs.Request;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IDailyCupTrackingService
    {
        Task<List<DailyCupTrackingDTO>> GetAllDailyCupTrackingsAsync();
        Task<DailyCupTrackingDTO?> GetByIdAsync(int id);

        Task<DailyCupTracking> CreateAsync(DailyCupTracking tracking);

        Task<bool> UpdateAsync(DailyCupTracking tracking);

        Task<bool> DeleteAsync(int id);
     }
}
