using EShop.Order.DataProvider.Repository;

namespace EShop.Order.DataProvider.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<bool> CreateOrder(Infrastructure.Order.Order order)
        {
            order.Id = Guid.NewGuid();
            return await _orderRepository.CreateOrder(order);
        }

        public async Task<List<Infrastructure.Order.Order>> GetAllOrders(string UserId)
        {
            return await _orderRepository.GetAllOrders(UserId);
        }

        public async Task<Infrastructure.Order.Order> GetOrder(string OrderId)
        {
            return await _orderRepository.GetOrder(OrderId);
        }
    }
}
