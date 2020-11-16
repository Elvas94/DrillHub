using DrillHub.Infrastructure;

namespace DrillHub.Model.Orders
{
    public class OrderService
    {
        private readonly IRepository<Order, int> _orderRepository;
        public OrderService(IRepository<Order, int> orderRepository)
        {
            _orderRepository = orderRepository; 
        }
    }
}
