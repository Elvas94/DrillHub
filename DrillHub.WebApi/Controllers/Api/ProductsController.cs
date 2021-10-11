using System.Collections.Generic;
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
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProductsBySubCategoryId/{id}")]
        public List<ProductDto> GetProductsBySubCategoryId([FromRoute] int id)
        {
            return _productService.GetProductsBySubCategoryId(id);
        }

        /// <summary>
        /// Получить продукт по id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id продукта</param>
        /// <returns>Продукт</returns>
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var product = _productService.GetProductDtoById(id);
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
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetProductsByIds([FromQuery] List<int> ids)
        {
            var products = _productService.GetProductDtosByIds(ids);

            return Ok(products);
        }

        /// <summary>
        /// Удалить продукт по Id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id продукта</param>
        /// <returns></returns>
        [Route("{id}")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public IActionResult DeleteProductById(int id)
        {
            _productService.DeleteProductById(id);
            return Ok();
        }

        /// <summary>
        /// Создать продукт, если его нет, обновить продукт, если он уже был создан
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Продукт с созданным Id, если его не было</returns>
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [HttpPost]
        public IActionResult InsertOrUpdate(ProductDto dto)
        {
            var result = _productService.InsertOrUpdate(dto);
            return Ok(result);
        }


    }
}
