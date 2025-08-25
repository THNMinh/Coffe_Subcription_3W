using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.Request;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Utils;
using Mapster;
using Repository.Repositories;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<Category> CreateAsync(Category cate)
        {
            return await _repo.CreateAsync(cate);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            return await _repo.UpdateAsync(category);
        }
        public async Task<(IEnumerable<CategoryDTO>, int totalItems)> GetAllCategoriesAsync(Search searchCondition, PageInfoRequestDTO pageInfo)
        {
            // Start with a base filter that is always true
            Expression<Func<Category, bool>> filter = u => true;
            // Apply filters dynamically
            if (!string.IsNullOrEmpty(searchCondition.Keyword))
            {
                string keyword = searchCondition.Keyword.ToLower();
                filter = ExpressionUtils.AddFilter(filter, c =>
                    c.CategoryName.ToLower().Contains(keyword) ||
                    (c.Description != null && c.Description.ToLower().Contains(keyword)));
            }
            filter = ExpressionUtils.AddFilter(filter, c => c.IsDelete == searchCondition.IsDelete);

            var items = await _repo.GetWithPaginationAsync(pageInfo, filter);
            int totalItems = await _repo.CountAsync(filter);

            List<CategoryDTO> itemDTOs = items.Select(items => items.Adapt<CategoryDTO>()).ToList();

            return (itemDTOs, totalItems);
        }
    }
}
