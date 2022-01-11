using System.Collections.Generic;
using System.Threading.Tasks;
using DrillHub.Model.ProductImages;
using DrillHub.Model.Products.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrillHub.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly ProductImageService _productImageService;

        public ProductImagesController(ProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        /// <summary>
        /// Получить все изображения продукта по eго id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id продукта</param>
        /// <returns>Изображения продукта</returns>
        [HttpGet]
        [Route("GetImagesByProductId/{id}")]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        public async Task<List<ProductImageDto>> GetProductsBySubCategoryIdAsync([FromRoute] int id)
        {
            return await _productImageService.GetProductImagesDtoByProductIdAsync(id);
        }

        /// <summary>
        /// Получить изображение продукта по id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id изображения</param>
        /// <returns>Изображение продукта</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductImageByIdAsync([FromRoute] int id)
        {
            var product = await _productImageService.GetProductImageDtoByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Удалить изображение продукта по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id изображения</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<NoContentResult> DeleteProductImageByIdAsync(int id)
        {
            await _productImageService.DeleteProductImageByIdAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Добавить изображения для продукта
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status204NoContent)]

        public async Task<NoContentResult> InsertOrUpdateAsync(List<ProductImageDto> images)
        {
            await _productImageService.SaveProductImagesAsync(images);
            return NoContent();
        }
    }
}
