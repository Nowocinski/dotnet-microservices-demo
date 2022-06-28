using EventBus.Activities;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace Order.Api.Handlers
{
    public class PlaceOrderHandler : IConsumer<Models.Order>
    {
        private readonly IEndpointNameFormatter _endpointNameFormatter;
        public PlaceOrderHandler(IEndpointNameFormatter endpointNameFormatter)
        {
            _endpointNameFormatter = endpointNameFormatter;
        }
        public async Task Consume(ConsumeContext<Models.Order> context)
        {
            try
            {
                var slip = CreateRoutingSlip(context);
                await context.Execute(slip);
            }
            catch (Exception ex)
            {
            }
        }

        private RoutingSlip CreateRoutingSlip(ConsumeContext<Models.Order> context)
        {
            Models.Order order = context.Message;
            var routingSlipBuilder = new RoutingSlipBuilder(order.Id);

            routingSlipBuilder.AddVariable("RequestId", context.RequestId);
            routingSlipBuilder.AddVariable("ResponseAddress", context.ResponseAddress);
            routingSlipBuilder.AddVariable("PlacedOrder", order);

            // Wallet Activity 
            // Sposób nr 1
            //routingSlipBuilder.AddActivity("PROCESS_WALLET", new Uri($"queue:wallet_execute"),
            //        new { order.Id, /*order.Amount */});

            // Sposób nr 2
            string walletActivityQueueName = _endpointNameFormatter.ExecuteActivity<WalletActivity, TransactMoney>();
            routingSlipBuilder.AddActivity("PROCESS_WALLET", new Uri($"queue:{walletActivityQueueName}"),
                    new { order.Id, /*order.Amount*/ });


            return routingSlipBuilder.Build();
        }
    }
}
