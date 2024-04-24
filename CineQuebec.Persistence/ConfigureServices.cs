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

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddMongoDb(configuration)
            .AddApplicationDbContextFactory()
            .AddUnitOfWorkFactory();
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(ConnectionStringName);
        MongoUrl mongoUrl = new(connectionString);
        MongoClient mongoClient = new(mongoUrl);
        IMongoDatabase? mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName ?? DefaultDatabaseName);

        DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMongoDB(mongoDatabase.Client, mongoDatabase.DatabaseNamespace.DatabaseName)
            .UseLazyLoadingProxies()
            .Options;

        return services
            .AddSingleton(mongoDatabase)
            .AddSingleton(dbContextOptions);
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