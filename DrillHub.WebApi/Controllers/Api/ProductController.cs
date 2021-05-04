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
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("GetProductsBySubCategoryId/{id}")]
        public List<ProductDto> GetProductsBySubCategoryId([FromRoute]int id)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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
    }
}
