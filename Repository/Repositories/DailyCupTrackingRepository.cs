using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class DailyCupTrackingRepository : GenericRepository<DailyCupTracking, int>, IDailyCupTrackingRepository
    {
        private readonly CoffeSubContext _context;
        public DailyCupTrackingRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
    }
}