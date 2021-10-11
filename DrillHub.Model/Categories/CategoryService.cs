using System.Collections.Generic;
using System.Linq;
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

        public List<Category> GetCategoriesWithSubCategories()
        {
            return _categoryRepository.Query()
                .Include(item => item.SubCategories).ToList();
        }

        public List<CategoryDto> GetCategoryDtos()
        {
            return _categoryRepository.Query()
                    .Select(item => new CategoryDto
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToList();
        }

        public CategoryDto GetCategoryById(int id)
        {
            var categoryDb = _categoryRepository.FirstOrDefault(item => item.Id == id);
            return categoryDb != null
                ? new CategoryDto
                {
                    Id = categoryDb.Id,
                    Name = categoryDb.Name
                }
                : null;
        }

        public List<SelectItem> GetCategoriesForSelect()
        {
            return _categoryRepository.Query()
                    .Select(item => new SelectItem
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToList();
        }

        public CategoryDto InsertOrUpdateCategory(CategoryDto dto)
        {
            var category = new Category
            {
                Id = dto.Id,
                Name = dto.Name
            };

            _categoryRepository.InsertOrUpdate(category);
            _categoryRepository.SaveChanges();

            dto.Id = category.Id;

            return dto;
        }

        public void DeleteCategoryById(int id)
        {
            _categoryRepository.DeleteByKey(id);
            _categoryRepository.SaveChanges();
        }
    }
}
