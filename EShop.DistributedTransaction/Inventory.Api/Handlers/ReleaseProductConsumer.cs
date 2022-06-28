using Inventory.Api.Command;
using MassTransit;

namespace Inventory.Api.Handlers
{
    public class ReleaseProductConsumer : IConsumer<ReleaseProduct>
    {
        public async Task Consume(ConsumeContext<ReleaseProduct> context)
        {
            // throw new NotImplementedException();
        }
    }
}
