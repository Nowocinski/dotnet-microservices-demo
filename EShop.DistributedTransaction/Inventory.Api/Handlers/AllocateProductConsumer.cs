using Inventory.Api.Command;
using MassTransit;

namespace Inventory.Api.Handlers
{
    public class AllocateProductConsumer : IConsumer<AllocateProduct>
    {
        public async Task Consume(ConsumeContext<AllocateProduct> context)
        {
            //throw new NotImplementedException();
        }
    }
}
