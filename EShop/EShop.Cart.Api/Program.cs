using EShop.Cart.Api.Handlers;
using EShop.Cart.DataProvider.Repository;
using EShop.Cart.DataProvider.Services;
using EShop.Infrastructure.EventBus;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis
builder.Services.AddStackExchangeRedisCache((cfg) =>
{
    cfg.Configuration = $"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}";
});
// DI
builder.Services.AddSingleton<ICartRepository, CartRepository>();
builder.Services.AddSingleton<ICartService, CartService>();
// RabbitMQ and MassTransit
var options = new RabbitMQOption();
builder.Configuration.GetSection("rabbitmq").Bind(options);
builder.Services.AddMassTransit(x => {
    x.AddConsumer<AddCartItemConsumer>();
    x.AddConsumer<RemoveCartItemConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => {
        cfg.Host(new Uri(options.ConnectionString), hostConfig => {
            hostConfig.Username(options.Username);
            hostConfig.Username(options.Password);
        });

        cfg.ReceiveEndpoint("add_cart", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<AddCartItemConsumer>(provider);
        });

        cfg.ReceiveEndpoint("remove_cart", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<RemoveCartItemConsumer>(provider);
        });
    }));
});

var app = builder.Build();

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
