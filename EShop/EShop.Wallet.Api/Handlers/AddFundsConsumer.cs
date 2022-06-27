using EShop.Infrastructure.Command.Wallet;
using MassTransit;

namespace EShop.Wallet.Api.Handlers
{
    public class AddFundsConsumer : IConsumer<AddFunds>
    {
        public Task Consume(ConsumeContext<AddFunds> context)
        {
            throw new NotImplementedException();
        }
    }
}
