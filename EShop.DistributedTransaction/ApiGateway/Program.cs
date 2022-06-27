using ApiGateway.EventBus;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MassTransit and RabbieMQ
var rabbitMq = new RabbitMqOption();
builder.Configuration.GetSection("rabbitmq").Bind(rabbitMq);
builder.Services.AddMassTransit(x => {
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitMq.ConnectionString), hostcfg => {
            hostcfg.Username(rabbitMq.Username);
            hostcfg.Password(rabbitMq.Password);
        });
        cfg.ConfigureEndpoints(provider);
    }));
    x.AddRequestClient<Order.Api.Models.Order>();
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
