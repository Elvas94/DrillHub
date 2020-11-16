using System.Collections.Generic;
using System.Linq;
using DrillHub.Infrastructure;
using DrillHub.Model.Products.Dtos;

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

        public ProductDto GetProductDtoById(int id)
        {
            var productInDb = _productRepository.FirstOrDefault(item => item.Id == id);
            return productInDb != null
                ? new ProductDto
                {
                    Id = productInDb.Id,
                    SubCategoryId = productInDb.SubCategoryId,
                    VendorCode = productInDb.VendorCode,
                    DisplayName = productInDb.DisplayName,
                    UnitType = productInDb.UnitType,
                    QuantityInStock = productInDb.QuantityInStock,
                    Price = productInDb.Price,
                    Description = productInDb.Description
                }
                : null;
        }

        public Product SaveProduct(ProductOnSavingDto dto)
        {
            var product = new Product
            {
                Id = dto.Id,
                SubCategoryId = dto.SubCategoryId,
                VendorCode = dto.VendorCode,
                OriginalName = dto.OriginalName,
                DisplayName = dto.DisplayName,
                UnitType = dto.UnitType,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock,
                Description = dto.Description
            };

            _productRepository.InsertOrUpdate(product);
            _productRepository.SaveChanges();

            return product;
        }

        public void DeleteProductById(int id)
        {
            _productRepository.DeleteByKey(id);
            _productRepository.SaveChanges();
        }
    }
}
