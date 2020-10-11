using System.Collections.Generic;
using DrillHub.Infrastructure;
using DrillHub.Model.SubCategories;

namespace DrillHub.Model.Products
{
    public class Product : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string OriginalName { get; set; }
        public string DisplayName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }
        public string VendorCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IList<ProductOrder> Orders { get; set; }
        public UnitType UnitType { get; set; }
    }
}
