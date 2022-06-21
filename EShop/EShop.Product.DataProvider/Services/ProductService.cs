using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using EShop.Product.DataProvider.Repositories;

namespace EShop.Product.DataProvider.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductCreated> GetProduct(Guid productId)
        {
            return await _productRepository.GetProduct(productId);
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            product.ProductId = Guid.NewGuid();
            return await _productRepository.AddProduct(product);
        }
    }
}
