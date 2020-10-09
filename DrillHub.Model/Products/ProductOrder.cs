using DrillHub.Infrastructure;
using DrillHub.Model.Orders;

namespace DrillHub.Model.Products
{
    public class ProductOrder : IAggregateRoot<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
