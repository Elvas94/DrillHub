using DrillHub.Infrastructure;
using DrillHub.Model.SubCategories.Dtos;

namespace DrillHub.Model.SubCategories
{
    public class SubCategoryService
    {
        private readonly IRepository<SubCategory, int> _subCategoryRepository;

        public SubCategoryService(IRepository<SubCategory, int>  subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public SubCategoryDto GetSubCategoryDtoById(int id)
        {
            var subCategoryDb = _subCategoryRepository.FirstOrDefault(item => item.Id == id);
            return subCategoryDb != null
                ? new SubCategoryDto()
                {
                    Id = subCategoryDb.Id,
                    Name = subCategoryDb.Name,
                    CategoryId = subCategoryDb.CategoryId,
                    IsCategory = subCategoryDb.IsCategory
                }
                : null;
        }

        public SubCategory SaveSubCategory(SubCategoryOnSavingDto dto)
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

            return subCategory;
        }

        public void DeleteSubCategoryById(int id)
        {
            _subCategoryRepository.DeleteByKey(id);
            _subCategoryRepository.SaveChanges();
        }
    }
}
