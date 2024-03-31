using CineQuebec.Application.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace CineQuebec.Persistence.DbContext;

public class UnitOfWorkFactory(IMongoDatabase database) : IUnitOfWorkFactory
{
	private readonly DbContextOptions<ApplicationDbContext> _options =
		new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
			.UseLazyLoadingProxies()
			.Options;

	public IUnitOfWork Create()
	{
		return new UnitOfWork(_options);
	}
}