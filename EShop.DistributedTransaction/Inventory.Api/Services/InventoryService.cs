using Inventory.Api.Command;
using Inventory.Api.Repository;

namespace Inventory.Api.Services
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepository _inventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task ReleaseStocks(ReleaseProduct stock)
        {
            await _inventoryRepository.ReleaseStocks(stock);
        }
        public async Task AddStocks(AllocateProduct stock)
        {
            await _inventoryRepository.AddStocks(stock);
        }
    }
}
