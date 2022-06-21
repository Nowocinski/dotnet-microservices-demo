﻿using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace EShop.Infrastructure.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly IMongoDatabase _database;
        public MongoInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

            IConventionPack conventionPack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
            };
            ConventionRegistry.Register("EShop", conventionPack, c => true);
            _initialized = true;
            await Task.CompletedTask;
        }
    }
}
