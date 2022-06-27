using Inventory.Api.Command;

namespace Inventory.Api.Repository
{
    public interface IInventoryRepository
    {
        Task ReleaseStocks(ReleaseProduct stock);
        Task AddStocks(AllocateProduct stock);
    }
}
