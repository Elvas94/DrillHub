using DrillHub.Model;
using DrillHub.Model.Categories;
using DrillHub.Model.SubCategories;
using DrillHub.Model.SubCategories.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [ProducesResponseType(typeof(List<SubCategoryDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult GetSubCategories()
        {
            return Ok(_subCategoryService.GetSubCategoryDtos());
        }

        /// <summary>
        /// Получить подкатегорию по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Подкатегория</returns>
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _subCategoryService.GetSubCategoryDtoById(id);

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
        [ProducesResponseType(typeof(List<SelectItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetSubCategoriesForSelect")]
        public IActionResult GetCategoriesForSelect()
        {
            var result = _subCategoryService.GetSubCategoriesForSelect();
            return Ok(result);
        }

        /// <summary>
        /// Удалить подкатегорию
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [Route("{id}")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public IActionResult DeleteCategoryById(int id)
        {
            _subCategoryService.DeleteSubCategoryById(id);
            return Ok();
        }

        /// <summary>
        /// Создать подкатегорию, если ее нет, обновить категорию, если она уже была
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Подкатегория с созданным Id, если его не было</returns>
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status200OK)]
        [HttpPost]
        public IActionResult InsertOrUpdateCategory(SubCategoryDto dto)
        {
            _subCategoryService.InsertOrUpdateSubCategory(dto);
            return Ok(dto);
        }
    }
}
