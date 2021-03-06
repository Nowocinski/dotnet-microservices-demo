using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EShop.Infrastructure.Mongo
{
    public static class Extension
    {
        public static void AddMongoDb(this IServiceCollection services, MongoConfig mongoConfig)
        {
            services.AddSingleton<IMongoClient>(client => {
                return new MongoClient(mongoConfig.ConnectionString);
            });
            services.AddSingleton<IMongoDatabase>(client => {
                var mongoClient = client.GetService<IMongoClient>();
                return mongoClient.GetDatabase(mongoConfig.Database);
            });

            services.AddSingleton<IDatabaseInitializer, MongoInitializer>();
        }
    }
}
