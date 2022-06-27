using MassTransit;

namespace Order.Api.Handlers
{
    public class PlaceOrderHandler : IConsumer<Models.Order>
    {
        public Task Consume(ConsumeContext<Models.Order> context)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
