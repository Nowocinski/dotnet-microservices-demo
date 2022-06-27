using Microsoft.EntityFrameworkCore;
using Wallet.Api.Commands;
using Wallet.Api.Context;

namespace Wallet.Api.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly DataBaseContext _context;
        public WalletRepository(DataBaseContext context)
        {
            _context = context;
        }
        public async Task DeductFunds(DeductFunds funds)
        {
            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(wallet => wallet.UserId == funds.UserId);
            if (userWallet is null)
            {
                throw new NotImplementedException();
            }

            userWallet.Fund -= funds.DebitAmount;

            _context.Wallets.Update(userWallet);
            await _context.SaveChangesAsync();
        }
        public async Task AddFunds(AddFund funds)
        {
            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(wallet => wallet.UserId == funds.UserId);
            if (userWallet is null)
            {
                throw new NotImplementedException();
            }

            userWallet.Fund += funds.CreditAmount;

            _context.Wallets.Update(userWallet);
            await _context.SaveChangesAsync();
        }
    }
}
