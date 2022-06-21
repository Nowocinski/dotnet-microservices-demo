using EShop.Infrastructure.Query.Product;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.EventBus
{
    public static class Extension
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, RabbitMQOption rabbitMQOption)
        {
            // establish connection with RabbitMQ...
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(rabbitMQOption.ConnectionString), hostcfg =>
                    {
                        hostcfg.Username(rabbitMQOption.Username);
                        hostcfg.Password(rabbitMQOption.Password);
                    });
                    cfg.ConfigureEndpoints(provider);
                }));
                x.AddRequestClient<GetProductById>();
            });

            return services;
        }
    }
}
