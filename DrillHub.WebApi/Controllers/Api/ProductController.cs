using System.Collections.Generic;
using DrillHub.Model.Products;
using DrillHub.Model.Products.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DrillHub.WebApi.Controllers.Api
{
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public List<ProductDto> GetProductsBySubCategoryId(int id)
        {
            return _productService.GetProductsBySubCategoryId(id);
        }
    }
}
