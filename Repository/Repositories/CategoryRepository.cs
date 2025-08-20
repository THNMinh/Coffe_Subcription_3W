using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        private readonly CoffeSubContext _context;
        public CategoryRepository(CoffeSubContext context) : base(context)
        {
            _context = context;
        }
    }
}
