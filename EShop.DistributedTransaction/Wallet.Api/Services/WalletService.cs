using Wallet.Api.Commands;
using Wallet.Api.Repository;

namespace Wallet.Api.Services
{
    public class WalletService : IWalletService
    {
        private IWalletRepository _walletRepository;
        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task DeductFunds(DeductFunds funds)
        {
            await _walletRepository.DeductFunds(funds);
        }
        public async Task AddFunds(AddFund funds)
        {
            await _walletRepository.AddFunds(funds);
        }
    }
}
