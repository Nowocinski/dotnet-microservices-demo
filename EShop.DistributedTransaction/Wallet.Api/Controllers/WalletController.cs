using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Context;

namespace Wallet.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController
    {
        private readonly DataBaseContext _context;
        public WalletController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("GetWallet")]
        public async Task<object> Get(Guid userId)
        {
            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(wallet => wallet.UserId == userId);
            if (userWallet is null)
            {
                throw new NotImplementedException();
            }

            return userWallet;
        }

        [HttpGet("GetWallet")]
        public async Task<object> Get()
        {
            return await _context.Wallets.ToListAsync(); ;
        }
    }
}
