namespace Order.Api.Services
{
    public interface IOrderService
    {
        Task CreateOrder(Models.Order order);
    }
}
