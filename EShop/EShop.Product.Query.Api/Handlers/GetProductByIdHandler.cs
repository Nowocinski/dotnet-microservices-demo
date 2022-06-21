using EShop.Infrastructure.Query.Product;
using EShop.Product.DataProvider.Services;
using MassTransit;

namespace EShop.Product.Query.Api.Handlers
{
    public class GetProductByIdHandler : IConsumer<GetProductById>
    {
        private readonly IProductService _productService;
        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task Consume(ConsumeContext<GetProductById> context)
        {
            var product = await _productService.GetProduct(context.Message.ProductId);
            await context.RespondAsync(product);
        }
    }
}
