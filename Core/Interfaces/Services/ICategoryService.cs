using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Request;
using Core.DTOs;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<bool> UpdateAsync(Category category);

        Task<bool> DeleteAsync(int id);

        Task<(IEnumerable<CategoryDTO>, int totalItems)> GetAllCategoriesAsync(Search searchCondition, PageInfoRequestDTO pageInfo);
    }
}
