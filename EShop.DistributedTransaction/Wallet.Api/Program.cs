using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Context;
using Wallet.Api.EventBus;
using Wallet.Api.Handlers;
using Wallet.Api.Repository;
using Wallet.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();
// Databse in memory
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseInMemoryDatabase("InMemoryWallet"));
var rabbitmqConfig = new RabbitMqOption();
builder.Configuration.GetSection("rabbitmq").Bind(rabbitmqConfig);
builder.Services.AddMassTransit(x => {
    x.AddConsumersFromNamespaceContaining<AddFundsConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => {
        cfg.Host(new Uri(rabbitmqConfig.ConnectionString), hostConfig =>
        {
            hostConfig.Username(rabbitmqConfig.Username);
            hostConfig.Password(rabbitmqConfig.Password);
        });

        cfg.ReceiveEndpoint("add_funds", ep => {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<AddFundsConsumer>(provider);
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

// Seeder
WalletDataSeeder.PerpPopulation(app);

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
