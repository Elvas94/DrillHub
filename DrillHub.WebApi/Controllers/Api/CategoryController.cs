using System.Collections.Generic;
using DrillHub.Model.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrillHub.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Получить все категории с подкатегориями
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категории с вложенными подкатегориями</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        [HttpGet]
        public List<Category> GetCategoriesWithSubCategories()
        {
            return _categoryService.GetCategoriesWithSubCategories();
        }
    }
}
