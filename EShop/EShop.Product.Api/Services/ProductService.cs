using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;

namespace EShop.Product.Api.Services
{
    public class ProductService : IProductService
    {
        public Task<ProductCreated> GetProduct(Guid productId)
        {
            return null;
        }

        public Task<ProductCreated> AddProduct(CreateProduct product)
        {
            return null;
        }
    }
}
