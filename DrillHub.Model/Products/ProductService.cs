using DrillHub.Model.Products;
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
    }
}
