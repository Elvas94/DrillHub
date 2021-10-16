using DrillHub.Infrastructure;
using DrillHub.Model.Categories;
using DrillHub.Model.SubCategories.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrillHub.Model.SubCategories
{
    public class SubCategoryService
    {
        private readonly IRepository<SubCategory, int> _subCategoryRepository;
        private readonly IRepository<Category, int> _categoryRepository;

        public SubCategoryService(
            IRepository<SubCategory, int> subCategoryRepository,
            IRepository<Category, int> categoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public Task<List<SubCategoryDto>> GetSubCategoryDtosAsync()
        {
            return (from subCategory in _subCategoryRepository.Query()
                    from category in _categoryRepository.Query()
                    where category.Id == subCategory.CategoryId
                    select new SubCategoryDto
                    {
                        CategoryId = category.Id,
                        Id = category.Id,
                        CategoryName = category.Name,
                        IsCategory = subCategory.IsCategory,
                        Name = subCategory.Name
                    }).ToListAsync();
        }

        public Task<SubCategoryDto> GetSubCategoryDtoByIdAsync(int id)
        {
            return (from subCategory in _subCategoryRepository.Query()
                    from category in _categoryRepository.Query()
                    where category.Id == subCategory.CategoryId
                    select new SubCategoryDto
                    {
                        CategoryId = category.Id,
                        Id = category.Id,
                        CategoryName = category.Name,
                        IsCategory = subCategory.IsCategory,
                        Name = subCategory.Name
                    }).FirstOrDefaultAsync(item => item.Id == id);
        }

        public Task<List<SelectItem>> GetSubCategoriesForSelectAsync()
        {
            return _subCategoryRepository.Query()
                    .Select(item => new SelectItem
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToListAsync();
        }

        public async Task<SubCategoryDto> InsertOrUpdateSubCategoryAsync(SubCategoryDto dto)
        {
            var subCategory = new SubCategory
            {
                Id = dto.Id,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsCategory = dto.IsCategory
            };

            _subCategoryRepository.InsertOrUpdate(subCategory);
            await _subCategoryRepository.SaveChangesAsync();

            dto.Id = subCategory.Id;

            return dto;
        }

        public async Task DeleteSubCategoryByIdAsync(int id)
        {
            _subCategoryRepository.DeleteByKey(id);
            await _subCategoryRepository.SaveChangesAsync();
        }
    }
}
