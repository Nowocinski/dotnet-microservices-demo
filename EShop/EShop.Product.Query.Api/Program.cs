using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Product.DataProvider.Repositories;
using EShop.Product.DataProvider.Services;
using EShop.Product.Query.Api.Handlers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddMongoDb(builder.Configuration.GetSection("mongo").Get<MongoConfig>());
var rabbitMQOption = builder.Configuration.GetSection("rabbitmq").Get<RabbitMQOption>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GetProductByIdHandler>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitMQOption.ConnectionString), hostcfg =>
        {
            hostcfg.Username(rabbitMQOption.Username);
            hostcfg.Password(rabbitMQOption.Password);
        });
        cfg.ConfigureEndpoints(provider);
    }));
});

var app = builder.Build();

var bus = app.Services.GetService<IBusControl>();
bus.Start();

var dbInitializerAsync = app.Services.GetService<IDatabaseInitializer>();
await dbInitializerAsync.InitializeAsync();

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
