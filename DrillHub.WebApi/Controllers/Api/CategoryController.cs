using System.Collections.Generic;
using DrillHub.Model.Categories;
using Microsoft.AspNetCore.Mvc;

namespace DrillHub.WebApi.Controllers.Api
{
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> GetCategoriesWithSubCategories()
        {
            return _categoryService.GetCategoriesWithSubCategories();
        }
    }
}
