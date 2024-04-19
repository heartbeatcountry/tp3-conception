using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Persistence.DbContext;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace CineQuebec.Persistence;

public static class ConfigureServices
{
    private const string ConnectionStringName = "DefaultConnection";
    private const string DefaultDatabaseName = "TP2DB";

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddMongoDb(configuration)
            .AddUnitOfWorkFactory();
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(ConnectionStringName);
        MongoUrl mongoUrl = new(connectionString);
        MongoClient mongoClient = new(mongoUrl);
        IMongoDatabase? mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName ?? DefaultDatabaseName);

        return services.AddSingleton(mongoDatabase);
    }

    private static IServiceCollection AddUnitOfWorkFactory(this IServiceCollection services)
    {
        return services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
    }
}