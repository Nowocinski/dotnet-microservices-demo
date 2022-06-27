using Inventory.Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController
    {
        private readonly DataBaseContext _context;
        public InventoryController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("GetInventory")]
        public async Task<object> Get(Guid productId)
        {
            var userWallet = await _context.Inventors
                .FirstOrDefaultAsync(wallet => wallet.ProductId == productId);
            if (userWallet is null)
            {
                throw new NotImplementedException();
            }

            return userWallet;
        }

        [HttpGet("GetInventories")]
        public async Task<object> Get()
        {
            return await _context.Inventors.ToListAsync(); ;
        }
    }
}
