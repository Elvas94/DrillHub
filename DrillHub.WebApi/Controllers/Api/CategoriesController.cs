using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// Получить все категории с подкатегориями
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категории с вложенными подкатегориями</returns>
        [HttpGet]
        [Route("GetCategoriesWithSubCategories")]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        public async Task<List<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return await _categoryService.GetCategoriesWithSubCategoriesAsync();
        }

        /// <summary>
        /// Получить категории для выпадающего списка
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Список категорий</returns>
        [HttpGet]
        [Route("GetCategoriesForSelect")]
        [ProducesResponseType(typeof(List<SelectItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<SelectItem>> GetCategoriesForSelectAsync()
        {
            return await _categoryService.GetCategoriesForSelectAsync();
        }

        /// <summary>
        /// Получить все категории
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Список категории</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            return Ok(await _categoryService.GetCategoryDtosAsync());
        }

        /// <summary>
        /// Получить категорию по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категория</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

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
        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
        {
            await _categoryService.DeleteCategoryByIdAsync(id);
            return Ok();
        }

        /// <summary>
        /// Создать категорию, если ее нет, обновить категорию, если она уже была
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Категория с созданным Id, если его не было</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        public async Task<CategoryDto> InsertOrUpdateCategoryAsync(CategoryDto dto)
        {
            return await _categoryService.InsertOrUpdateCategoryAsync(dto);
        }
    }
}
