using System.Collections.Generic;
using System.Linq;
using DrillHub.Infrastructure;
using DrillHub.Model.Categories;
using DrillHub.Model.Products.Dtos;
using DrillHub.Model.SubCategories;

namespace DrillHub.Model.Products
{
    public class ProductService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<SubCategory, int> _subCategoryRepository;
        private readonly IRepository<Category, int> _categoryRepository;

        public ProductService(
            IRepository<Product, int> productRepository,
            IRepository<SubCategory, int> subCategoryRepository,
            IRepository<Category, int> categoryRepository)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public List<ProductDto> GetProductsBySubCategoryId(int subCategoryId)
        {
            return GetProductDtos().Where(product => product.SubCategoryId == subCategoryId).ToList();
        }

        public ProductDto GetProductDtoById(int id)
        {
            return GetProductDtos().FirstOrDefault(item => item.Id == id);
        }

        public List<ProductDto> GetProductDtosByIds(List<int> ids)
        {
            return GetProductDtos().Where(item => ids.Contains(item.Id)).ToList();
        }

        public ProductDto InsertOrUpdate(ProductDto dto)
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

            dto.Id = product.Id;

            return dto;
        }

        public void DeleteProductById(int id)
        {
            _productRepository.DeleteByKey(id);
            _productRepository.SaveChanges();
        }

        private IQueryable<ProductDto> GetProductDtos()
        {
            return from product in _productRepository.Query()
                   join subCategory in _subCategoryRepository.Query() on product.SubCategoryId equals subCategory.Id
                   join category in _categoryRepository.Query() on subCategory.CategoryId equals category.Id
                   select
                   new ProductDto
                   {
                       Id = product.Id,
                       SubCategoryId = product.SubCategoryId,
                       VendorCode = product.VendorCode,
                       DisplayName = product.DisplayName,
                       UnitType = product.UnitType,
                       QuantityInStock = product.QuantityInStock,
                       Price = product.Price,
                       Description = product.Description,
                       CategoryName = category.Name,
                       OrderStatus = product.OrderStatus,
                       OriginalName = product.OriginalName,
                       SubCategoryName = subCategory.Name
                   };
        }
    }
}
