using System.Collections.Generic;
using DrillHub.Model;
using DrillHub.Model.Categories;
using DrillHub.Model.Categories.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrillHub.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Получить все категории
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Список категории</returns>
        [Route("GetCategoriesWithSubCategories")]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        [HttpGet]
        public List<Category> GetCategoriesWithSubCategories()
        {
            return _categoryService.GetCategoriesWithSubCategories();
        }

        /// <summary>
        /// Получить категории для выпадающего списка
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Список категорий</returns>
        [ProducesResponseType(typeof(List<SelectItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetCategoriesForSelect")]
        public IActionResult GetCategoriesForSelect()
        {
            var result = _categoryService.GetCategoriesForSelect();
            return Ok(result);
        }

        /// <summary>
        /// Получить все категории с подкатегориями
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категории с вложенными подкатегориями</returns>
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(_categoryService.GetCategoryDtos());
        }

        /// <summary>
        /// Получить категорию по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категория</returns>
        [Route("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [Route("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [HttpDelete]
        public IActionResult DeleteCategoryById(int id)
        {
            _categoryService.DeleteCategoryById(id);
            return Ok();
        }

        /// <summary>
        /// Создать категорию, если ее нет, обновить категорию, если она уже была
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категория с созданным Id, если его не было</returns>
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [HttpPost]
        public IActionResult InsertOrUpdateCategory(CategoryDto dto)
        {
            var category = _categoryService.InsertOrUpdateCategory(dto);
            return Ok(category);
        }
    }
}
