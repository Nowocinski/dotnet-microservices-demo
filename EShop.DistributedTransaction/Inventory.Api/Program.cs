using Inventory.Api.Context;
using Inventory.Api.EventBus;
using Inventory.Api.Handlers;
using Inventory.Api.Repository;
using Inventory.Api.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
// Databse in memory
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseInMemoryDatabase("InMemoryInventor"));
// Databse in memory
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseInMemoryDatabase("InMemoryWallet"));
var rabbitmqConfig = new RabbitMqOption();
builder.Configuration.GetSection("rabbitmq").Bind(rabbitmqConfig);
builder.Services.AddMassTransit(x => {
    x.AddConsumersFromNamespaceContaining<AllocateProductConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => {
        cfg.Host(new Uri(rabbitmqConfig.ConnectionString), hostConfig =>
        {
            hostConfig.Username(rabbitmqConfig.Username);
            hostConfig.Password(rabbitmqConfig.Password);
        });

        cfg.ReceiveEndpoint("allocate_product", ep => {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<AllocateProductConsumer>(provider);
        });

        cfg.ReceiveEndpoint("release_product", ep => {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<ReleaseProductConsumer>(provider);
        });

    }));
});

var app = builder.Build();

var busControl = app.Services.GetService<IBusControl>();
busControl.Start();

// Seeder
InventorDataSeeder.PerpPopulation(app);

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
