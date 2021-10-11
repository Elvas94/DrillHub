using DrillHub.Infrastructure;
using DrillHub.Model.Categories;
using DrillHub.Model.SubCategories.Dtos;
using System.Collections.Generic;
using System.Linq;

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

        public List<SubCategoryDto> GetSubCategoryDtos()
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
                    }).ToList();
        }

        public SubCategoryDto GetSubCategoryDtoById(int id)
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
                    }).FirstOrDefault(item => item.Id == id);
        }

        public List<SelectItem> GetSubCategoriesForSelect()
        {
            return _subCategoryRepository.Query()
                    .Select(item => new SelectItem
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToList();
        }

        public SubCategoryDto InsertOrUpdateSubCategory(SubCategoryDto dto)
        {
            var subCategory = new SubCategory
            {
                Id = dto.Id,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsCategory = dto.IsCategory
            };

            _subCategoryRepository.InsertOrUpdate(subCategory);
            _subCategoryRepository.SaveChanges();

            dto.Id = subCategory.Id;

            return dto;
        }

        public void DeleteSubCategoryById(int id)
        {
            _subCategoryRepository.DeleteByKey(id);
            _subCategoryRepository.SaveChanges();
        }
    }
}
