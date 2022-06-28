using MassTransit;
using Wallet.Api.Commands;
using Wallet.Api.Services;

namespace Wallet.Api.Handlers
{
    public class AddFundsConsumer : IConsumer<AddFund>
    {
        private IWalletService _walletService;
        public AddFundsConsumer(IWalletService walletService)
        {
            _walletService = walletService;
        }
        public async Task Consume(ConsumeContext<AddFund> context)
        {
            //var isAdded = await _walletService.AddFunds(context.Message);

            //if (isAdded)
            //    await Task.CompletedTask;
            //else
            //    throw new Exception("New Funds are not added. Try after sometime.");
        }
    }
}
