using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Wallet.Api.Handlers;
using EShop.Wallet.DataProvider.Repository;
using EShop.Wallet.DataProvider.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();
// MongoDB
builder.Services.AddMongoDb(builder.Configuration.GetSection("mongo").Get<MongoConfig>());
// MassTransit and RabbitMQ
var rabbitMQOption = builder.Configuration.GetSection("rabbitmq").Get<RabbitMQOption>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AddFundsConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitMQOption.ConnectionString), hostcfg =>
        {
            hostcfg.Username(rabbitMQOption.Username);
            hostcfg.Password(rabbitMQOption.Password);
        });
        cfg.ReceiveEndpoint("add_funds", endPoint =>
        {
            endPoint.PrefetchCount = 16;
            endPoint.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(2, 100);
            });
            endPoint.ConfigureConsumer<AddFundsConsumer>(provider);
        });
        cfg.ReceiveEndpoint("deduct_funds", ep => {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<DeductFundsConsumer>(provider);
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
