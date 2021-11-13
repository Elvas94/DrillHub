using DrillHub.Infrastructure;

namespace DrillHub.Model.ProductImages
{
    public class ProductImage: IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
    }
}
