using DrillHub.Model;
using DrillHub.Model.Categories;
using DrillHub.Model.SubCategories;
using DrillHub.Model.SubCategories.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DrillHub.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly SubCategoryService _subCategoryService;

        public SubCategoriesController(SubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        /// <summary>
        /// Получить все подкатегории
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Подкатегории</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<SubCategoryDto>), StatusCodes.Status200OK)]
        public async Task<List<SubCategoryDto>> GetSubCategoriesAsync()
        {
            return await _subCategoryService.GetSubCategoryDtosAsync();
        }

        /// <summary>
        /// Получить подкатегорию по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Подкатегория</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _subCategoryService.GetSubCategoryDtoByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Получить подкатегории для выпадающего списка
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Список подкатегорий</returns>
        [HttpGet]
        [Route("GetSubCategoriesForSelect")]
        [ProducesResponseType(typeof(List<SelectItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<SelectItem>> GetSubCategoriesForSelectAsync()
        {
            return await _subCategoryService.GetSubCategoriesForSelectAsync();
        }

        /// <summary>
        /// Удалить подкатегорию
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
        {
            await _subCategoryService.DeleteSubCategoryByIdAsync(id);
            return Ok();
        }

        /// <summary>
        /// Создать подкатегорию, если ее нет, обновить категорию, если она уже была
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Подкатегория с созданным Id, если его не было</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status200OK)]
        public async Task<SubCategoryDto> InsertOrUpdateCategoryAsync(SubCategoryDto dto)
        {
            await _subCategoryService.InsertOrUpdateSubCategoryAsync(dto);
            return dto;
        }
    }
}
