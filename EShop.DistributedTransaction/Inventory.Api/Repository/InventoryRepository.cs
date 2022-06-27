using Inventory.Api.Command;
using Inventory.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataBaseContext _context;
        public InventoryRepository(DataBaseContext context)
        {
            _context = context;
        }
        public async Task ReleaseStocks(ReleaseProduct stock)
        {
            stock.Items.ForEach(async item =>
            {
                var product = await _context.Inventors.FirstOrDefaultAsync(i => i.ProductId == item.ProductId);
                if (product is null)
                {
                    throw new NotImplementedException();
                }
                product.Quantity -= item.Quantity;
                _context.Update(product);
            });

            await _context.SaveChangesAsync();
        }
        public async Task AddStocks(AllocateProduct stock)
        {
            stock.Items.ForEach(async item =>
            {
                var product = await _context.Inventors.FirstOrDefaultAsync(i => i.ProductId == item.ProductId);
                if (product is null)
                {
                    throw new NotImplementedException();
                }
                product.Quantity += item.Quantity;
                _context.Update(product);
            });

            await _context.SaveChangesAsync();
        }
    }
}
