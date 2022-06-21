using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;

namespace EShop.Product.Api.Repositories
{
    public interface IProductRepository
    {
        Task<ProductCreated> GetProduct(Guid productId);
        Task<ProductCreated> AddProduct(CreateProduct product);
    }
}
