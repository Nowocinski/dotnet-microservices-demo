namespace Order.Api.Repository
{
    public interface IOrderRepository
    {
        Task CreateOrder(Models.Order order);
    }
}
