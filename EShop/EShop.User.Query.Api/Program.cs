using EShop.Infrastructure.Authentication;
using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Infrastructure.Security;
using EShop.User.DataProvider.Repositories;
using EShop.User.DataProvider.Services;
using EShop.User.Query.Api.Handlers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IEncrypter, Encrypter>();
builder.Services.AddSingleton<LoginUserHangler>();
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddMongoDb(builder.Configuration.GetSection("mongo").Get<MongoConfig>());
var rabbitMQOption = builder.Configuration.GetSection("rabbitmq").Get<RabbitMQOption>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LoginUserHangler>();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
