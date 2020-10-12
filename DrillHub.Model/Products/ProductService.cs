using System.Collections.Generic;
using System.Linq;
using DrillHub.Infrastructure;

namespace DrillHub.Model.Products
{
    public class ProductService
    {
        private readonly IRepository<Product, int> _productRepository;

        public ProductService(IRepository<Product, int> productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductDto> GetProductsBySubCategoryId(int subCategoryId)
        {
            return _productRepository
                .Search(item => item.SubCategoryId == subCategoryId)
                .Select(item => new ProductDto
                {
                    Id = item.Id,
                    SubCategoryId = item.SubCategoryId,
                    VendorCode = item.VendorCode,
                    DisplayName = item.DisplayName,
                    UnitType = item.UnitType,
                    QuantityInStock = item.QuantityInStock,
                    Price = item.Price,
                    Description = item.Description
                }).ToList();
        }
    }
}
