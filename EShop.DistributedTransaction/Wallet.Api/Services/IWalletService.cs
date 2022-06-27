using Wallet.Api.Commands;

namespace Wallet.Api.Services
{
    public interface IWalletService
    {
        Task DeductFunds(DeductFunds funds);
        Task AddFunds(AddFund funds);
    }
}
