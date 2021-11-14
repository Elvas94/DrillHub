using DrillHub.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrillHub.Model.ProductImages
{
    public class ProductImageService
    {
        private readonly IRepository<ProductImage, int> _imageRepository;

        public ProductImageService(IRepository<ProductImage, int> imageRepository)
        {

            _imageRepository = imageRepository;
        }

        public Task<List<ProductImageDto>> GetProductImagesDtoByProductIdAsync(int productId)
        {
            return _imageRepository.Search(item => item.ProductId == productId)
                                   .Select(item => new ProductImageDto
                                   {
                                       Id = item.Id,
                                       Content = item.Content,
                                       Extension = item.Extension,
                                       Name = item.Name,
                                       ProductId = item.ProductId,
                                       Size = item.Size
                                   })
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<ProductImageDto> GetProductImageDtoByIdAsync(int imageId)
        {
            var imageInDb = await _imageRepository.FirstOrDefaultAsync(item => item.Id == imageId);

            return imageInDb != null
                    ? new ProductImageDto
                    {
                        Id = imageInDb.Id,
                        Content = imageInDb.Content,
                        Extension = imageInDb.Extension,
                        Name = imageInDb.Name,
                        ProductId = imageInDb.ProductId,
                        Size = imageInDb.Size
                    }
                    : null;
        }

        public async Task SaveProductImagesAsync(IEnumerable<ProductImageDto> images)
        {
            if (images == null || !images.Any())
            {
                return;
            }

            var productId = images.First().ProductId;

            var imagesByProductId = _imageRepository.Search(item => item.ProductId == productId);

            _imageRepository.DeleteRange(imagesByProductId);

            await _imageRepository.SaveChangesAsync();

            var newImages = images.Select(image => new ProductImage
            {
                Content = image.Content,
                Extension = image.Extension,
                Id = image.Id,
                Name = image.Name,
                ProductId = productId,
                Size = image.Size
            });

            _imageRepository.InsertRange(newImages);

            await _imageRepository.SaveChangesAsync();
        }

        public async Task DeleteProductImageByIdAsync(int id)
        {
            _imageRepository.DeleteByKey(id);
            await _imageRepository.SaveChangesAsync();
        }
    }
}
