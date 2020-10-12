using System.Collections.Generic;
using System.Linq;
using DrillHub.Infrastructure;
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
    }
}
