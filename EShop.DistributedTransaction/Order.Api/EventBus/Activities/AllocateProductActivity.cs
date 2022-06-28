using MassTransit;

namespace EventBus.Activities
{
    using Inventory.Api.Command;
    using System.Threading.Tasks;

    public class AllocateProductActivity : IActivity<AllocateProduct, OrderLog>
    {
        public async Task<CompensationResult> Compensate(CompensateContext<OrderLog> context)
        {
            try
            {
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/allocate_product"));
                //var allocateProduct = JsonConvert.DeserializeObject<AllocateProduct>(context.Message.Variables["PlacedOrder"].ToString());
                await endpoint.Send(/*allocateProduct*/new AllocateProduct());

                return context.Compensated();
            }
            catch (Exception)
            {
                return context.Failed();
            }
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<AllocateProduct> context)
        {
            try
            {
                var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/release_product"));
                //var order = JsonConvert.DeserializeObject<ReleaseProduct>(context.Message.Variables["PlacedOrder"].ToString());
                var order = new ReleaseProduct();

                await endpoint.Send(order);

                return context.CompletedWithVariables<ReleaseProduct>(order, new { });
            }
            catch (Exception)
            {
                return context.Faulted();
            }
        }
    }

    public class OrderLog
    {
        public Order.Api.Models.Order Order { get; set; }
        public string Message { get; set; }
    }
}

namespace Inventory.Api.Command
{
    public class AllocateProduct
    {
        public List<Models.Inventor> Items { get; set; }
    }
}

namespace Inventory.Api.Models
{
    public class Inventor
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}

namespace Inventory.Api.Command
{
    public class ReleaseProduct
    {
        public List<Models.Inventor> Items { get; set; }
    }
}