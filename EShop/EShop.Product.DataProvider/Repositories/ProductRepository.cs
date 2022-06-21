using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using MongoDB.Driver;

namespace EShop.Product.DataProvider.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CreateProduct> _collection;
        public ProductRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<CreateProduct>("product");
        }

        public async Task<ProductCreated> GetProduct(Guid productId)
        {
            var product = _collection.AsQueryable().Where(x => x.ProductId == productId).FirstOrDefault();
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            await Task.CompletedTask;
            return new ProductCreated
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<ProductCreated> AddProduct(CreateProduct product)
        {
            await _collection.InsertOneAsync(product);
            return new ProductCreated
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
