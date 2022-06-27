using EShop.Infrastructure.Command.Wallet;
using MassTransit;

namespace EShop.Wallet.Api.Handlers
{
    public class DeductFundsConsumer : IConsumer<DeductFunds>
    {
        public Task Consume(ConsumeContext<DeductFunds> context)
        {
            throw new NotImplementedException();
        }
    }
}
