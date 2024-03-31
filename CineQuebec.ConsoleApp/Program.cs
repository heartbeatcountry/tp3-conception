using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CineQuebec.ConsoleApp;

internal class UOWTest
{
	public UOWTest(IUnitOfWorkFactory unitOfWorkFactory)
	{
		var unitOfWork = unitOfWorkFactory.Create();
		var filmRepository = unitOfWork.FilmRepository;
	}
}

internal static class Program
{
	public static void Main()
	{
		var builder = Host.CreateApplicationBuilder();
		builder.Configuration.AddJsonFile("appsettings.local.json", true);

		builder.Services.AddPersistenceServices(builder.Configuration)
			.AddSingleton<UOWTest>();

		using var host = builder.Build();

		var service = host.Services.GetRequiredService<UOWTest>();

		host.Run();
	}
}