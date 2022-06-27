using Microsoft.EntityFrameworkCore;
using Wallet.Api.Context;
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
    options.UseInMemoryDatabase("InMemory"));

var app = builder.Build();

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
