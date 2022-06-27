using Inventory.Api.Command;

namespace Inventory.Api.Services
{
    public interface IInventoryService
    {
        Task ReleaseStocks(ReleaseProduct stock);
        Task AddStocks(AllocateProduct stock);
    }
}
