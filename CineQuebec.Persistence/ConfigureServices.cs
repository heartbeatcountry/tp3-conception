using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Persistence.DbContext;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace CineQuebec.Persistence;

public static class ConfigureServices
{
    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        MongoUrl mongoUrl = new(connectionString);
        MongoClient mongoClient = new(mongoUrl);
        IMongoDatabase? mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName ?? "TP2DB");

        return services.AddSingleton(mongoDatabase);
    }

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddMongoDb(configuration)
            .AddUnitOfWorkFactory();
    }

    private static IServiceCollection AddUnitOfWorkFactory(this IServiceCollection services)
    {
        return services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
    }
}