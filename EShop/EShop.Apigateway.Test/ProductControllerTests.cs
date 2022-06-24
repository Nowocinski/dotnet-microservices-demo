using EShop.ApiGateway.Controllers;
using EShop.Infrastructure.Command.Product;
using EShop.Infrastructure.Event.Product;
using EShop.Infrastructure.Query.Product;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace EShop.Apigateway.Test
{
    public class DerivedClass : Response<ProductCreated>
    {
        public ProductCreated Message => new ProductCreated()
        {
            ProductId = new Guid("a159d2f9-f975-4e01-93c4-08a01d749151"),
            ProductName = "testing product",
            CreatedAt = DateTime.UtcNow
        };

        public Guid? MessageId => throw new NotImplementedException();

        public Guid? RequestId => throw new NotImplementedException();

        public Guid? CorrelationId => throw new NotImplementedException();

        public Guid? ConversationId => throw new NotImplementedException();

        public Guid? InitiatorId => throw new NotImplementedException();

        public DateTime? ExpirationTime => throw new NotImplementedException();

        public Uri? SourceAddress => throw new NotImplementedException();

        public Uri? DestinationAddress => throw new NotImplementedException();

        public Uri? ResponseAddress => throw new NotImplementedException();

        public Uri? FaultAddress => throw new NotImplementedException();

        public DateTime? SentTime => throw new NotImplementedException();

        public Headers Headers => throw new NotImplementedException();

        public HostInfo Host => throw new NotImplementedException();

        object Response.Message => throw new NotImplementedException();
    }

    [TestFixture]
    public class ProductControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Add()
        {
            // Arrange
            var sendEndpoint = new Mock<ISendEndpoint>();
            var busControl = new Mock<IBusControl>();
            busControl.Setup(x => x.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(sendEndpoint.Object);
            var productController = new ProductController(busControl.Object, null);

            // Act
            var result = await productController.Add(It.IsAny<CreateProduct>());

            // Assert
            Assert.That(((AcceptedResult)result).StatusCode, Is.EqualTo((int?)HttpStatusCode.Accepted));
        }

        [Test]
        public async Task GetProductReturnsProductWithAcceptedResult()
        {
            // Arrange
            var expectedProduct = new ProductCreated()
            {
                ProductId = new Guid("a159d2f9-f975-4e01-93c4-08a01d749151"),
                ProductName = "testing product",
                CreatedAt = DateTime.UtcNow
            };
            var deriveResponse = new Mock<DerivedClass>();
            var requestClient = new Mock<IRequestClient<GetProductById>>();
            requestClient.Setup(x => x.GetResponse<ProductCreated>(It.IsAny<GetProductById>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                .ReturnsAsync(deriveResponse.Object);
            var productController = new ProductController(null, requestClient.Object);

            // Act
            var result = await productController.Get(new Guid("a159d2f9-f975-4e01-93c4-08a01d749151"));

            // Assert
            Assert.IsTrue((result as AcceptedResult).StatusCode == (int?)HttpStatusCode.Accepted);
            var recievedProduct = ((result as AcceptedResult).Value as Response<ProductCreated>).Message;
            Assert.IsTrue(recievedProduct.ProductId == expectedProduct.ProductId);
        }
    }
}