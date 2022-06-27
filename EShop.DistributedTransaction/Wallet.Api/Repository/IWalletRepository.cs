using Wallet.Api.Commands;

namespace Wallet.Api.Repository
{
    public interface IWalletRepository
    {
        Task DeductFunds(DeductFunds funds);
        Task AddFunds(AddFund funds);
    }
}
