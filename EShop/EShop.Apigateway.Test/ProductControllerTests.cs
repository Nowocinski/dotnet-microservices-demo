using EShop.ApiGateway.Controllers;
using EShop.Infrastructure.Command.Product;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace EShop.Apigateway.Test
{
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
    }
}