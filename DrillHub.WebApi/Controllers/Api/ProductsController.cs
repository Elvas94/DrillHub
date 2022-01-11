using System.Collections.Generic;
using System.Threading.Tasks;
using DrillHub.Model.Products;
using DrillHub.Model.Products.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrillHub.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Получить все продукты в подкатегории по eё id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id подкатегории</param>
        /// <returns>Продукты в подкатегории</returns>
        [HttpGet]
        [Route("GetProductsBySubCategoryId/{id}")]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        public async Task<List<ProductDto>> GetProductsBySubCategoryIdAsync([FromRoute] int id)
        {
            return await _productService.GetProductsBySubCategoryIdAsync(id);
        }

        /// <summary>
        /// Получить продукт по id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id продукта</param>
        /// <returns>Продукт</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] int id)
        {
            var product = await _productService.GetProductDtoByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        /// <summary>
        /// Получить продукты по списку ids
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ids">Ids продукта</param>
        /// <returns>Продукты</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<ProductDto>> GetProductsByIdsAsync([FromQuery] List<int> ids)
        {
            return await _productService.GetProductDtosByIdsAsync(ids);
        }

        /// <summary>
        /// Удалить продукт по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id продукта</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            await _productService.DeleteProductByIdAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Создать продукт, если его нет, обновить продукт, если он уже был создан
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Продукт с созданным Id, если его не было</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        
        public async Task<ProductDto> InsertOrUpdateAsync(ProductDto dto)
        {
            return await _productService.InsertOrUpdateAsync(dto);
        }
    }
}
