using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrillHub.Infrastructure;
using DrillHub.Model.Categories.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DrillHub.Model.Categories
{
    public class CategoryService
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CategoryService(IRepository<Category, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<List<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return _categoryRepository.Query()
                .Include(item => item.SubCategories).ToListAsync();
        }

        public Task<List<CategoryDto>> GetCategoryDtosAsync()
        {
            return _categoryRepository.Query()
                    .Select(item => new CategoryDto
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var categoryDb = await _categoryRepository.FirstOrDefaultAsync(item => item.Id == id);
            return categoryDb != null
                ? new CategoryDto
                {
                    Id = categoryDb.Id,
                    Name = categoryDb.Name
                }
                : null;
        }

        public Task<List<SelectItem>> GetCategoriesForSelectAsync()
        {
            return _categoryRepository.Query()
                    .Select(item => new SelectItem
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToListAsync();
        }

        public async Task<CategoryDto> InsertOrUpdateCategoryAsync(CategoryDto dto)
        {
            var category = new Category
            {
                Id = dto.Id,
                Name = dto.Name
            };

            _categoryRepository.InsertOrUpdate(category);
            await _categoryRepository.SaveChangesAsync();

            dto.Id = category.Id;

            return dto;
        }

        public async Task DeleteCategoryByIdAsync(int id)
        {
            _categoryRepository.DeleteByKey(id);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
