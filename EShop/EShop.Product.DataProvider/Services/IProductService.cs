using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;

namespace EShop.Product.DataProvider.Services
{
    public interface IProductService
    {
        Task<ProductCreated> GetProduct(Guid productId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
