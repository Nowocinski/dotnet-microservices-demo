using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Order.Api.Handlers;
using EShop.Order.DataProvider.Repository;
using EShop.Order.DataProvider.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
// MongoDB
builder.Services.AddMongoDb(builder.Configuration.GetSection("mongo").Get<MongoConfig>());
// RabbitMQ and MassTransit
var rabbitMQOption = builder.Configuration.GetSection("rabbitmq").Get<RabbitMQOption>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateOrderHandler>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitMQOption.ConnectionString), hostcfg =>
        {
            hostcfg.Username(rabbitMQOption.Username);
            hostcfg.Password(rabbitMQOption.Password);
        });
        cfg.ReceiveEndpoint("create_order", endPoint =>
        {
            endPoint.PrefetchCount = 16;
            endPoint.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(2, 100);
            });
            endPoint.ConfigureConsumer<CreateOrderHandler>(provider);
        });
    }));
});

var app = builder.Build();

var busControl = app.Services.GetService<IBusControl>();
busControl.Start();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
