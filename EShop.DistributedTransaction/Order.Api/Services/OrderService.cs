using Order.Api.Repository;

namespace Order.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task CreateOrder(Models.Order order)
        {
            await _orderRepository.CreateOrder(order);
        }
    }
}
