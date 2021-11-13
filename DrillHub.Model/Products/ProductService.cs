using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrillHub.Infrastructure;
using DrillHub.Model.Categories;
using DrillHub.Model.ProductImages;
using DrillHub.Model.Products.Dtos;
using DrillHub.Model.SubCategories;
using Microsoft.EntityFrameworkCore;

namespace DrillHub.Model.Products
{
    public class ProductService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<ProductImage, int> _imageRepository;
        private readonly IRepository<SubCategory, int> _subCategoryRepository;
        private readonly IRepository<Category, int> _categoryRepository;


        public ProductService(
            IRepository<Product, int> productRepository,
            IRepository<ProductImage, int> imageRepository,
            IRepository<SubCategory, int> subCategoryRepository,
            IRepository<Category, int> categoryRepository)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public Task<List<ProductDto>> GetProductsBySubCategoryIdAsync(int subCategoryId)
        {
            return GetProductDtos().Where(product => product.SubCategoryId == subCategoryId).ToListAsync();
        }

        public async Task<ProductDto> GetProductDtoByIdAsync(int id)
        {
            var product = await GetProductDtos().FirstOrDefaultAsync(item => item.Id == id);
            product.ProductImages = await _imageRepository.Search(item => item.ProductId == product.Id).AsNoTracking().ToListAsync();
            return product;
        }

        public Task<List<ProductDto>> GetProductDtosByIdsAsync(List<int> ids)
        {
            return GetProductDtos().Where(item => ids.Contains(item.Id)).ToListAsync();
        }

        public async Task<ProductDto> InsertOrUpdateAsync(ProductDto dto)
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
            await _productRepository.SaveChangesAsync();

            dto.Id = product.Id;

            foreach (var image in dto.ProductImages)
            {
                var test = new ProductImage
                {
                    Content = image.Content,
                    Extension = image.Extension,
                    Id = image.Id,
                    Name = image.Name,
                    ProductId = dto.Id,
                    Size = image.Size
                };
                _imageRepository.InsertOrUpdate(test);
            }

            await _productRepository.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteProductByIdAsync(int id)
        {
            _productRepository.DeleteByKey(id);
            await _productRepository.SaveChangesAsync();
        }

        private IQueryable<ProductDto> GetProductDtos()
        {
            return (from product in _productRepository.Query()
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
                   }).AsNoTracking();
        }
    }
}
