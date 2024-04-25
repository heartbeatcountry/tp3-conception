using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Persistence.DbContext;
using CineQuebec.Persistence.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace CineQuebec.Persistence;

public static class ConfigureServices
{
    private const string ConnectionStringName = "DefaultConnection";
    private const string DefaultDatabaseName = "TP2DB";

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services
            .AddMongoDb()
            .AddApplicationDbContextFactory()
            .AddUnitOfWorkFactory();
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMongoDatabase>(sp =>
            {
                IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                string? connectionString = configuration.GetConnectionString(ConnectionStringName);
                MongoUrl mongoUrl = new(connectionString);
                MongoClient mongoClient = new(mongoUrl);
                IMongoDatabase? mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName ?? DefaultDatabaseName);
                return mongoDatabase;
            })
            .AddSingleton<DbContextOptions<ApplicationDbContext>>(sp =>
            {
                IMongoDatabase mongoDatabase = sp.GetRequiredService<IMongoDatabase>();
                DbContextOptions<ApplicationDbContext> dbContextOptions =
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseMongoDB(mongoDatabase.Client, mongoDatabase.DatabaseNamespace.DatabaseName)
                        .UseLazyLoadingProxies()
                        .Options;
                return dbContextOptions;
            });
    }

    private static IServiceCollection AddApplicationDbContextFactory(this IServiceCollection services)
    {
        return services.AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory>();
    }

    private static IServiceCollection AddUnitOfWorkFactory(this IServiceCollection services)
    {
        return services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
    }
}