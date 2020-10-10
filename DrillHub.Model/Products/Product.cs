using System.Collections.Generic;
using DrillHub.Infrastructure;

namespace DrillHub.Model.Products
{
    public class Product : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int SubcategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }
        public string VendorCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IList<ProductOrder> Orders { get; set; }
        public UnitType UnitType { get; set; }
    }
}
