using EShop.Infrastructure.EventBus;
using EShop.Infrastructure.Mongo;
using EShop.Infrastructure.Security;
using EShop.User.Api.Handlers;
using EShop.User.DataProvider.Repositories;
using EShop.User.DataProvider.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongoDb(builder.Configuration.GetSection("mongo").Get<MongoConfig>());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddSingleton<IEncrypter, Encrypter>();

var rabbitMQOption = builder.Configuration.GetSection("rabbitmq").Get<RabbitMQOption>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateUserHandler>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitMQOption.ConnectionString), hostcfg =>
        {
            hostcfg.Username(rabbitMQOption.Username);
            hostcfg.Password(rabbitMQOption.Password);
        });
        cfg.ReceiveEndpoint("add_user", endPoint =>
        {
            endPoint.PrefetchCount = 16;
            endPoint.UseMessageRetry(retryConfig =>
            {
                retryConfig.Interval(2, 100);
            });
            endPoint.ConfigureConsumer<CreateUserHandler>(provider);
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
