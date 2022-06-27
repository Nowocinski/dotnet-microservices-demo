using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Context;
using Order.Api.EventBus;
using Order.Api.Handlers;
using Order.Api.Repository;
using Order.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
// Databse in memory
builder.Services.AddDbContext<DataBaseContext>(options =>
    options.UseInMemoryDatabase("InMemoryOrder"));
// MassTransit and RabbieMQ
var rabbitMq = new RabbitMqOption();
builder.Configuration.GetSection("rabbitmq").Bind(rabbitMq);
builder.Services.AddMassTransit(x => {
    x.AddConsumer<PlaceOrderHandler>();
    x.SetKebabCaseEndpointNameFormatter();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitMq.ConnectionString), hostconfig =>
        {
            hostconfig.Username(rabbitMq.Username);
            hostconfig.Password(rabbitMq.Password);
        });

        cfg.ConfigureEndpoints(provider);
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
