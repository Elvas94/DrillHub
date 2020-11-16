namespace DrillHub.Model.Products.Dtos
{
    public class ProductOnSavingDto
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
        public UnitType UnitType { get; set; }
    }
}
